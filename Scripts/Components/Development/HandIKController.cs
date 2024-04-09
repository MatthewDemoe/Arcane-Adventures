using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using UnityEngine;
using static com.AlteredRealityLabs.ArcaneAdventures.Components.Development.SetupPhaseInputHandler;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    [RequireComponent(typeof(Animator))]
    public class HandIKController : MonoBehaviour
    {
        [SerializeField] private GameObject rightMiddleFingerBone;
        [SerializeField] private GameObject leftMiddleFingerBone;
        
        private Animator animator;
        private ProjectSpecificTrackedPoseDriver projectSpecificTrackedPoseDriver;
        
        [Inject] protected SetupPhaseSettings setupPhaseSettings;

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            animator = GetComponent<Animator>();
            projectSpecificTrackedPoseDriver = Camera.main.GetComponent<ProjectSpecificTrackedPoseDriver>();
        }

        void OnAnimatorIK()
        {
            SetIKPositionAndRotation(AvatarIKGoal.LeftHand, HandSide.Left);
            SetIKPositionAndRotation(AvatarIKGoal.RightHand, HandSide.Right);
            SetLookAt();
        }

        private void SetIKPositionAndRotation(AvatarIKGoal avatarIKGoal, HandSide handSide)
        {
            if (projectSpecificTrackedPoseDriver == null)
                projectSpecificTrackedPoseDriver = Camera.main.GetComponent<ProjectSpecificTrackedPoseDriver>();

            animator.SetIKPosition(avatarIKGoal, GetTargetPosition(handSide));
            animator.SetIKRotation(avatarIKGoal, GetTargetRotation(handSide));
            animator.SetIKPositionWeight(avatarIKGoal, 1);
            animator.SetIKRotationWeight(avatarIKGoal, 1);
        }

        private Vector3 GetTargetPosition(HandSide handSide)
        {
            var controllerLink = ControllerLink.Get(handSide);
            var handPosition = controllerLink.GetHandPosition();
            var middleFingerBone = handSide.Equals(HandSide.Left) ? leftMiddleFingerBone : rightMiddleFingerBone;
            var displacement = middleFingerBone.transform.position - middleFingerBone.transform.parent.position;
            handPosition -= displacement;

            return handPosition;
        }
        
        private Quaternion GetTargetRotation(HandSide handSide)
        {
            var controllerLink = ControllerLink.Get(handSide);
            
            if (!controllerLink.isHoldingItem)
                return controllerLink.transform.rotation;

            var isRightStandardOrLeftReverse = handSide.Equals(HandSide.Right) == controllerLink.isUsingStandardGrip;

            var z = isRightStandardOrLeftReverse ? 0 : 180;
            
            if (controllerLink.isSecondHandInTwoHandedWielding)
                isRightStandardOrLeftReverse = !isRightStandardOrLeftReverse;
            
            var x = 90 * (isRightStandardOrLeftReverse ? -1 : 1);
            
            return controllerLink.rotationDisplacement * Quaternion.Euler(x, 0, z);
        }
        
        private void SetLookAt()
        {
            var lookAtPosition = Camera.main.transform.position + Camera.main.transform.forward * 1;
            animator.SetLookAtPosition(lookAtPosition);
            animator.SetLookAtWeight(1);
        }
    }
}