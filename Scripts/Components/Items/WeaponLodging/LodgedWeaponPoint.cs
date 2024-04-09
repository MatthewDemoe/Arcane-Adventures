using com.AlteredRealityLabs.ArcaneAdventures.UnityWrappers;
using UnityEngine;
using static com.AlteredRealityLabs.ArcaneAdventures.Combat.StrikeCalculator;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class LodgedWeaponPoint : MonoBehaviour
    {
        private const float PostLodgeFreezeTime = 0.5f;
        private const float PostDislodgeIgnoreTime = 0.5f;
        private const float MinimumLodgingDepenetrationDistance = 0.025f;
        private const float MinimumGapDifferenceForAnchorUpdate = 0.05f;

        private Phase phase = Phase.NotInUse;
        private float phaseTimer = 0;
        private ConfigurableJoint joint;
        private WeaponLodgeSurface weaponLodgeSurface;
        private SetPhysicsIgnoreCommand setPhysicsIgnoreCommand = new SetPhysicsIgnoreCommand();
        private Collider weaponCollider;
        private ContactPoint contactPoint;
        private Identifiers.InGameMaterial physicalWeaponPartInGameMaterial;
        private float gapOnLastAnchorUpdate;
        private bool hasTriedToUpdateAnchorAtLeastOnce = false;
        private StrikeMovement strikeMovementOnEntry;
        private LodgeAxis lodgeAxis;

        public PhysicalWeapon physicalWeapon { get; private set; }

        private bool IsMaterialStoppingStrike(float depenetrationDistance)
            => depenetrationDistance > weaponLodgeSurface.material.strikeStoppingDepenetrationDistance;

        public bool isInUse => !phase.Equals(Phase.NotInUse);

        private void FixedUpdate()
        {
            switch (phase)
            {
                case Phase.Lodging: TryLodge(); break;
                case Phase.PostLodgeFreeze: MoveOnToDislodgableStateIfPastTimeLimit(); break;
                case Phase.Dislodgable: TryDislodge(); break;
                case Phase.PostDislodgeIgnore: DestroyIfPastTimeLimit(); break;
            }
        }

        public bool TryLodge(bool ignoreStriking = false)
        {
            if (!phase.Equals(Phase.Lodging))
            {
                return false;
            }

            var depenetrationDistance = GetBiggestDepenetrationDistance();

            if (!ignoreStriking && physicalWeapon.isStriking && !IsMaterialStoppingStrike(depenetrationDistance))
            {
                return false;
            }

            if (depenetrationDistance > MinimumLodgingDepenetrationDistance && 
                (
                    strikeMovementOnEntry.Equals(StrikeMovement.Stab) 
                    || !weaponLodgeSurface.material.identifier.Equals(Identifiers.InGameMaterial.Flesh)
                )//TODO: Not proper solution, use the game materials instead.
            )
            {
                Lodge();

                return true;
            }
            else
            {
                var position = weaponLodgeSurface.GetOverlappingPosition(contactPoint.point, contactPoint.otherCollider, contactPoint.thisCollider);
                weaponLodgeSurface.PlayContactEffect(physicalWeaponPartInGameMaterial, position, 1);//TODO: Send in sensible values.
                Destroy(this);

                return false;
            }
        }

        private float GetBiggestDepenetrationDistance()
        {
            var biggestDistance = 0f;

            foreach (var weaponLodgeSurfaceCollider in weaponLodgeSurface.colliders)
            {
                if (Physics.ComputePenetration(weaponLodgeSurfaceCollider, weaponLodgeSurface.transform.position, weaponLodgeSurface.transform.rotation, weaponCollider, weaponCollider.transform.position, weaponCollider.transform.rotation, out var direction, out var distance))
                {
                    if (distance > biggestDistance)
                    {
                        biggestDistance = distance;
                    }
                }
            }

            return biggestDistance;
        }

        private void Lodge()
        {
            lodgeAxis = strikeMovementOnEntry.Equals(StrikeMovement.Stab) ? LodgeAxis.X :
                strikeMovementOnEntry.Equals(StrikeMovement.Slash) ? LodgeAxis.Y :
                LodgeAxis.NotSet;

            if (lodgeAxis.Equals(LodgeAxis.NotSet))
            {
                lodgeAxis = LodgeAxis.X;//TODO: Determine best axis for this case (at the time of writing, the only case for this is throwing).
            }

            joint = physicalWeapon.gameObject.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = true;

            joint.xMotion = lodgeAxis.Equals(LodgeAxis.X) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
            joint.yMotion = lodgeAxis.Equals(LodgeAxis.Y) ? ConfigurableJointMotion.Limited : ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;
            joint.angularXMotion = ConfigurableJointMotion.Locked;
            joint.angularYMotion = ConfigurableJointMotion.Locked;
            joint.angularZMotion = ConfigurableJointMotion.Locked;
            joint.linearLimit = new SoftJointLimit
            {
                limit = 0.01f
            };
            joint.linearLimitSpring = new SoftJointLimitSpring
            {
                spring = 25000,
                damper = 10000
            };
            joint.enableCollision = true;

            joint.connectedBody = GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            joint.anchor = physicalWeapon.transform.InverseTransformPoint(transform.TransformPoint(joint.connectedAnchor));

            phaseTimer = 0;
            phase = Phase.PostLodgeFreeze;
            var position = weaponLodgeSurface.GetOverlappingPosition(contactPoint.point, contactPoint.otherCollider, contactPoint.thisCollider);
            weaponLodgeSurface.PlayContactEffect(physicalWeaponPartInGameMaterial, position, 1);//TODO: Send in sensible values.
        }

        private bool TryUpdateAnchor()
        {
            var positionDifference = physicalWeapon.transform.InverseTransformPoint(transform.TransformPoint(joint.connectedAnchor));
            var gap = lodgeAxis.Equals(LodgeAxis.X) ? positionDifference.x : positionDifference.y;

            if (!hasTriedToUpdateAnchorAtLeastOnce)
            {
                hasTriedToUpdateAnchorAtLeastOnce = true;
                gapOnLastAnchorUpdate = gap;

                return false;
            }

            var gapDifferenceWithLastUpdate = gap - gapOnLastAnchorUpdate;
            var updateAnchor = Mathf.Abs(gapDifferenceWithLastUpdate) > MinimumGapDifferenceForAnchorUpdate;

            if (updateAnchor)
            {
                gapOnLastAnchorUpdate = gap;
                var x = lodgeAxis.Equals(LodgeAxis.X) ? joint.anchor.x + gapDifferenceWithLastUpdate : 0;
                var y = lodgeAxis.Equals(LodgeAxis.Y) ? joint.anchor.y + gapDifferenceWithLastUpdate : 0;

                joint.anchor = new Vector3(x, y, 0);
            }

            return updateAnchor;
        }

        private void TryDislodge()
        {
            if (TryUpdateAnchor() && GetBiggestDepenetrationDistance() == 0)
            {
                joint.xMotion = ConfigurableJointMotion.Free;
                joint.yMotion = ConfigurableJointMotion.Free;
                joint.zMotion = ConfigurableJointMotion.Free;
                joint.angularXMotion = ConfigurableJointMotion.Free;
                joint.angularYMotion = ConfigurableJointMotion.Free;
                joint.angularZMotion = ConfigurableJointMotion.Free;

                var position = weaponLodgeSurface.GetOverlappingPosition(contactPoint.point, contactPoint.otherCollider, contactPoint.thisCollider);
                weaponLodgeSurface.PlayContactEffect(physicalWeaponPartInGameMaterial, position, 1);//TODO: Send in sensible values.

                phase = Phase.PostDislodgeIgnore;
            }
        }

        private void MoveOnToDislodgableStateIfPastTimeLimit()
        {
            phaseTimer += Time.deltaTime;

            if (phaseTimer > PostLodgeFreezeTime)
            {
                phaseTimer = 0;
                phase = Phase.Dislodgable;
            }
        }

        private void DestroyIfPastTimeLimit()
        {
            phaseTimer += Time.deltaTime;

            if (phaseTimer > PostDislodgeIgnoreTime)
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            weaponLodgeSurface.ReportLodgedWeaponPointRemoved(this);
            setPhysicsIgnoreCommand.Reuse(invert: true);
            setPhysicsIgnoreCommand.TryExecute();

            if (joint != null)
            {
                Destroy(joint);
            }
        }

        public void Activate(PhysicalWeapon physicalWeapon, WeaponLodgeSurface weaponLodgeSurface, ContactPoint contactPoint)
        {
            this.physicalWeapon = physicalWeapon;
            this.weaponLodgeSurface = weaponLodgeSurface;
            this.contactPoint = contactPoint;
            weaponCollider = contactPoint.otherCollider;
            physicalWeaponPartInGameMaterial = physicalWeapon.GetColliderInGameMaterial(weaponCollider);

            strikeMovementOnEntry = physicalWeapon.strikeMovement;

            setPhysicsIgnoreCommand.Set(new Collider[] { weaponCollider }, weaponLodgeSurface.colliders, ignore: true);
            setPhysicsIgnoreCommand.TryExecute();

            if (!physicalWeapon.isWielded)
            {
                physicalWeapon.rigidbody.velocity = physicalWeapon.rigidbody.velocity.magnitude * contactPoint.normal;
                physicalWeapon.rigidbody.angularVelocity = Vector3.zero;
            }

            phase = Phase.Lodging;
        }

        private enum Phase
        {
            NotInUse,
            Lodging,
            PostLodgeFreeze,
            Dislodgable,
            PostDislodgeIgnore
        }

        private enum LodgeAxis
        {
            NotSet,
            X,
            Y
        }
    }
}