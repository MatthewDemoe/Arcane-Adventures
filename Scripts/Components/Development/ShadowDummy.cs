using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class ShadowDummy : HumanoidMonster
    {
        private const float AttackInterval = 3f;

        [SerializeField] private Animator innerAnimator;
        [SerializeField] private GameObject innerOrcRightHand;
        [SerializeField] private GameObject outerOrcRightHand;

        private static Vector3 PositionalDisplacement = new Vector3(0.07f, -0.17f, -0.05f);
        private static Vector3 RotationalDisplacement = new Vector3(-90, 0, 0);

        protected override System.Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.Grell); } }

        private float timeOfLastAttack;
        private PhysicalWeapon rightHandWeapon;
        public bool isInAttackMode;

        protected override void Start()
        {
            base.Start();
            rightHandWeapon = itemEquipper.GetWeaponInHand(HandSide.Right);
            itemEquipper.GetNPCControllerLink(HandSide.Right).SetTarget(innerOrcRightHand);
        }

        private void Update()
        {
            if (animator.enabled && isInAttackMode && timeOfLastAttack + AttackInterval < Time.time && !animator.GetBool(CharacterAnimatorParameters.IsRecoilingFromHit))
            {
                innerAnimator.speed = 0.33f;
                innerAnimator.SetBool(GrellAnimatorParameters.IsUsingOverheadSwing, true);
                rightHandWeapon.SetManualStrikeType(StrikeType.Perfect);
            }

            if (!rightHandWeapon.strikeType.Equals(StrikeType.NotStrike) && !innerAnimator.GetBool(GrellAnimatorParameters.IsUsingOverheadSwing))
            {
                rightHandWeapon.SetManualStrikeType(StrikeType.NotStrike);
            }
        }

        private void OnAnimatorIK()
        {
            if (itemEquipper.GetNPCControllerLink(HandSide.Right).isHoldingItem)
                SetIKForHand(HandSide.Right);
        }

        private void SetIKForHand(HandSide handSide)
        {
            var avatarIKGoal = handSide.Equals(HandSide.Left) ? AvatarIKGoal.LeftHand : AvatarIKGoal.RightHand;

            animator.SetIKPositionWeight(avatarIKGoal, 1);
            animator.SetIKRotationWeight(avatarIKGoal, 1);
            animator.SetIKPosition(avatarIKGoal, GetTargetPosition(handSide));
            animator.SetIKRotation(avatarIKGoal, GetTargetRotation(handSide));
        }

        private Vector3 GetTargetPosition(HandSide handSide)
        {
            return itemEquipper.GetNPCControllerLink(handSide).GetConnectedItemGripPosition(PositionalDisplacement);
        }

        private Quaternion GetTargetRotation(HandSide handSide)
        {
            return itemEquipper.GetNPCControllerLink(handSide).connectedItem.gameObject.transform.rotation * Quaternion.Euler(RotationalDisplacement);
        }

        public void SetPartialRagdollMode(bool on)
        {
            animator.enabled = !on;
        }

        public void Recoil(float magnitude)
        {
            innerAnimator.SetBool(GrellAnimatorParameters.IsUsingOverheadSwing, false);

            var timeToRagdoll = magnitude * 0.05f;
            StartCoroutine(Ragdoll(timeToRagdoll));
        }

        private IEnumerator Ragdoll(float time)
        {
            animator.enabled = false;
            itemEquipper.GetNPCControllerLink(HandSide.Right).SetTarget(outerOrcRightHand);
            yield return new WaitForSeconds(time);
            animator.enabled = true;
            itemEquipper.GetNPCControllerLink(HandSide.Right).SetTarget(innerOrcRightHand);
        }
    }
}