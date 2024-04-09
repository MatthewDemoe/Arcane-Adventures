using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using System;
using UnityEngine;
using UnityEngine.VFX;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class PossessedDummyReference : Monster
    {
        private const float NoForceVelocity = 2f;
        private const float HeadYForce = 67.2f;
        private const float ForearmsYForce = 6.4f;
        private const float ShouldAddForceXYRotationLimit = 25f;
        private const float ShouldAddForceYPositionLimit = 0.1f;
        private const float JointDestructionDelay = 0.5f;

        [SerializeField] private Rigidbody headRigidbody;
        [SerializeField] private Rigidbody[] forearmRigidbodies;
        [SerializeField] private Rigidbody leftLegRigidbody;
        [SerializeField] private Rigidbody rightLegRigidbody;
        [SerializeField] private Rigidbody pelvis;

        private VisualEffect[] visualEffects;
        private Joint[] joints;

        private bool disconnected;
        private Vector3 headForce = Vector3.zero;
        private Vector3 forearmsForce = Vector3.zero;

        protected override Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.PossessedDummy); } }

        protected override void Awake()
        {
            base.Awake();
            visualEffects = GetComponentsInChildren<VisualEffect>();
            joints = GetComponentsInChildren<Joint>();
            headForce.y = HeadYForce;
            forearmsForce.y = ForearmsYForce;
        }

        private void FixedUpdate()
        {
            var shouldAddForce = ShouldAddForce();

            if (shouldAddForce)
            {
                headRigidbody.AddForce(headForce, ForceMode.Impulse);

                foreach (var forearmRigidBody in forearmRigidbodies)
                {
                    forearmRigidBody.AddForce(forearmsForce, ForceMode.Impulse);
                }
            }
        }

        private bool ShouldAddForce()
        {
            if (creature.isDead || headRigidbody.velocity.magnitude > NoForceVelocity)
            {
                return false;
            }

            var eulerRotation = pelvis.transform.localRotation.eulerAngles;

            return eulerRotation.x > ShouldAddForceXYRotationLimit || 
                eulerRotation.x < -ShouldAddForceXYRotationLimit || 
                eulerRotation.y > ShouldAddForceXYRotationLimit || 
                eulerRotation.y < -ShouldAddForceXYRotationLimit ||
                leftLegRigidbody.transform.position.y < ShouldAddForceYPositionLimit || 
                rightLegRigidbody.transform.position.y < ShouldAddForceYPositionLimit;
        }

        public override void ProcessDamage()
        {
            base.ProcessDamage();

            if (creature.isDead && !disconnected)
            {
                foreach (var visualEffect in visualEffects)
                {
                    visualEffect.enabled = false;
                }

                Invoke(nameof(DestroyAllJoints), JointDestructionDelay);

                disconnected = true;
            }
        }

        private void DestroyAllJoints()
        {
            foreach (var joint in joints)
            {
                Destroy(joint);
            }
        }
    }
}