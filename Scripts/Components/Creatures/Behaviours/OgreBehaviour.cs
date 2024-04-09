using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponFunctionality;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using UnityEngine;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(Ogre))]
    public class OgreBehaviour : SpellCastingEnemyBehaviour
    {
        Ogre ogreReference => creatureReference as Ogre;
        CharacterAnimationAudioSourcePoolPlayer characterAudioSourceCreator;

        public override List<Weapon> useableWeapons => new List<Weapon>() 
        {
            ItemCache.GetItem(ItemCache.ItemNames.Tree) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Fist) as Weapon,
        };

        public override float closeRangeDistance => 5.0f;
        const float ThrowVelocityMultiplier = 25.0f;

        private bool isEquippedWithTree => (ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right)).name.Contains(ItemCache.ItemNames.Tree);

        private List<AttackableSurface> _rightArmSurfaces = null;
        private List<AttackableSurface> rightArmSurfaces
        {
            get
            {
                if (!(_rightArmSurfaces is null))
                    return _rightArmSurfaces;

                _rightArmSurfaces = GetRightArmSurfaces();
                return _rightArmSurfaces;
            }
        }

        private GameObject heldCharacter;

        public override bool isAttacking => GetIsAttacking(OgreAnimatorParameters.attacks);
        protected override bool shouldUpdateMovementParameters => false;

        public override List<Role> alternativeRoles => new List<Role>()
        {
            Role.SupportAttacker,
        };

        public override Role preferredRole => Role.PrimaryAttacker;

        private List<AttackableSurface> GetRightArmSurfaces()
        {
            List<Collider> rightArmColliders = GetComponent<AttackableSurfaceCoordinator>().colliders.ToList().
                FindAll(collider =>
                {
                    AttackableSurface attackableSurface = collider.GetComponent<AttackableSurface>();
                    return attackableSurface.surfaceName.Equals($"{BodyPartNames.Right} {BodyPartNames.Hand}") || attackableSurface.surfaceName.Equals($"{BodyPartNames.Right} {BodyPartNames.Forearm}");
                });

            return rightArmColliders.Select(collider => collider.GetComponent<AttackableSurface>()).ToList();
        }

        protected override void Start()
        {
            base.Start();

            navMeshAgent.updatePosition = false;
            characterAudioSourceCreator = GetComponentInChildren<CharacterAnimationAudioSourcePoolPlayer>();
        }

        public override void FinishAttack()
        {
            base.FinishAttack();

            PhysicalWeapon rightHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right);
            PhysicalWeapon leftHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Left);

            if (rightHandWeapon != null)
                rightHandWeapon.SetManualStrikeType(StrikeType.NotStrike);

            if (leftHandWeapon != null)
                leftHandWeapon.SetManualStrikeType(StrikeType.NotStrike);
        }

        public override void PickUpItem(PhysicalWeapon physicalWeapon)
        {
            base.PickUpItem(physicalWeapon);
            animator.SetBool(CharacterAnimatorParameters.IsPickingUpItem, true);
            characterAudioSourceCreator.PlayAudioClipByName(CharacterAudioClipNames.OgreAudioClipNames.OgreGrabLow);
        }

        public override void CloseGap()
        {
            if (enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(Charge.Instance) || !TryCastSpell(Charge.Instance))
                return;

            animator.SetBool(OgreAnimatorParameters.IsUsingCharge, true);
            animationEventInvoker.OnAnimationInitialize.AddListener(StartCast);
        }

        public override bool Attack()
        {
            if ((Time.time - timeOfLastAttack) < timeBetweenAttacks)
                return false;

            //TODO: Enable when there's an animation
            //if (distanceToTarget < maximumCloseRangeDistance)
            //{
            //    if (TryCastSpell(Stomp.Instance))
            //    {
            //        animator.SetBool(OgreAnimatorParameters.IsUsingStomp, true);
            //        return true;
            //    }
            //}

            if (isEquippedWithTree)
                DetermineArmedAttack();

            else
                DetermineUnarmedAttack();

            return true;
        }

        public void DetermineUnarmedAttack()
        {
            PhysicalWeapon rightHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right);
            PhysicalWeapon leftHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Left);

            float action = Random.value;

            if (action > 0.5f)
            {
                animator.SetBool(OgreAnimatorParameters.IsGrabbingPlayer, true);
                characterAudioSourceCreator.PlayAudioClipByName(CharacterAudioClipNames.OgreAudioClipNames.OgreGrabLow);
                animationEventInvoker.OnAnimationEnd.AddListener(GrabCharacter);
            }
            
            else
            {
                animator.SetBool(OgreAnimatorParameters.IsUsingHandSlam, true);
                leftHandWeapon.SetManualStrikeType(StrikeType.Imperfect);
            }
        }

        public void DetermineArmedAttack()
        {
            PhysicalWeapon rightHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right);
            PhysicalWeapon leftHandWeapon = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Left);

            float action = Random.value;

            if (ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right).GetComponent<TreeSlamDurabilityTracker>().maximumDurabilityReached)
            {
                animationEventInvoker.OnAnimationEnd.AddListener(ThrowTree);
                characterAudioSourceCreator.PlayAudioClipByName(CharacterAudioClipNames.OgreAudioClipNames.OgreThrow);

                animator.SetBool(OgreAnimatorParameters.IsUsingThrow, true);
                return;
            }

            if (action < 5f && !enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(TreeSlam.Instance))
            {
                if (!TryCastSpell(TreeSlam.Instance))
                    return;

                characterAudioSourceCreator.PlayAudioClipByName(CharacterAudioClipNames.OgreAudioClipNames.OgreOverheadSlam);
                animator.SetBool(OgreAnimatorParameters.IsUsingOverheadSwing, true);
                return;
            }

            else
            {
                characterAudioSourceCreator.PlayAudioClipByName(CharacterAudioClipNames.OgreAudioClipNames.OgreSwingMulti);
                animator.SetBool(OgreAnimatorParameters.IsUsingSlashCombo, true);
            }
            
            rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);
        }

        private void ThrowTree()
        {
            PhysicalWeapon stump = ogreReference.itemEquipper.GetWeaponInHand(HandSide.Right);

            ogreReference.itemEquipper.UnequipItem(HandSide.Right);
            stump.GetComponent<Rigidbody>().velocity = ((target.transform.position - stump.transform.position).normalized * ThrowVelocityMultiplier);
            stump.SetManualStrikeType(StrikeType.Imperfect);

            animationEventInvoker.OnAnimationEnd.RemoveListener(ThrowTree);
        }

        private void ThrowCharacter()
        {
            LetCharacterGo();

            heldCharacter.GetComponent<Rigidbody>().velocity = transform.forward * ThrowVelocityMultiplier;
        }

        private void DropCharacter()
        {
            LetCharacterGo();
            
            animator.SetBool(OgreAnimatorParameters.GrabInterrupted, true);
        }

        private void LetCharacterGo()
        {
            rightArmSurfaces.ForEach(surface => surface.OnSurfaceAttacked.RemoveListener(DropCharacter));

            animationEventInvoker.OnAnimationEnd.RemoveListener(ThrowCharacter);

            heldCharacter.GetComponent<TransformAttacher>().Unattach();
            target = heldCharacter.transform;

            ogreReference.itemEquipper.GetItemInHand(HandSide.Right).SetActive(true);
        }

        private void GrabCharacter()
        {
            animationEventInvoker.OnAnimationEnd.RemoveListener(GrabCharacter);
            animationEventInvoker.OnAnimationEnd.AddListener(ThrowCharacter);

            rightArmSurfaces.ForEach(surface => surface.OnSurfaceAttacked.AddListener(DropCharacter));

            heldCharacter = target.gameObject;

            TransformAttacher transformAttacher = target.gameObject.AddComponent<TransformAttacher>();
            transformAttacher.Initialize(ogreReference.itemEquipper.GetTarget(HandSide.Right).transform);

            ogreReference.itemEquipper.GetItemInHand(HandSide.Right).SetActive(false);
            target = transform;
        } 
    }
}