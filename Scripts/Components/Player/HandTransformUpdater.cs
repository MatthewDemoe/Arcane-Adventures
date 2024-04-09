using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using UMA.CharacterSystem;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    [RequireComponent(typeof(Animator))]
    public class HandTransformUpdater : MonoBehaviour
    {
        [SerializeField] private GameObject rightMiddleFingerBone;
        [SerializeField] private GameObject leftMiddleFingerBone;

        private Animator animator;

        public GameObject GetMiddleFingerBone(HandSide handSide) => handSide.Equals(HandSide.Left) ? leftMiddleFingerBone : rightMiddleFingerBone;

        private bool _isPlayerAvatar = true;
        private ControllerLocationMimicker controllerLocationMimicker = null;
        private DynamicCharacterAvatar DynamicCharacterAvatar;

        CreatureReference creatureReference;

        private void Start()
        {
            animator = GetComponent<Animator>();
            creatureReference = GetComponentInParent<CreatureReference>();

            _isPlayerAvatar = GetComponentInParent<ControllerLocationMimicker>() == null;

            if (!_isPlayerAvatar)
            {
                controllerLocationMimicker = GetComponentInParent<ControllerLocationMimicker>();
                DynamicCharacterAvatar = GetComponent<DynamicCharacterAvatar>();
                DynamicCharacterAvatar.CharacterCreated.AddListener((umaData) =>
                {
                    controllerLocationMimicker.SetHeadLocation(umaData.GetBoneGameObject(UmaBones.Head).transform);
                });
            }
        }

        private void OnAnimatorIK()
        {
            if (_isPlayerAvatar && !creatureReference.creature.isInputEnabled)
                return;

            SetIKForHand(HandSide.Left);
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
            var controllerLink = ControllerLink.Get(handSide);
            var handPosition = _isPlayerAvatar ? controllerLink.GetHandPosition() : controllerLocationMimicker.HandTransform(handSide).position;

            var middleFingerBone = GetMiddleFingerBone(handSide);
            var displacement = middleFingerBone.transform.position - middleFingerBone.transform.parent.position;
            handPosition -= displacement;

            return handPosition;
        }

        private Quaternion GetTargetRotation(HandSide handSide)
        {            
            var controllerLink = ControllerLink.Get(handSide);

            if (controllerLink.isHoldingItem)
            {
                var yModifier = controllerLink.isUsingStandardGrip ? 0 : 180;
                var zModifier = handSide.Equals(HandSide.Left) ? -180 : 0;

                return controllerLink.connectedItem.transform.rotation * Quaternion.Euler(90, yModifier, zModifier);
            }
            else
            {
                var controller = XRReferences.GetController(handSide);
                var rotationDisplacement = Quaternion.Euler(0, 0, handSide.Equals(HandSide.Left) ? 90 : -90);

                return (_isPlayerAvatar ? controller.transform.rotation : controller.transform.localRotation) * rotationDisplacement;
            }
        }

    }
}