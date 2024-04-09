using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Player.Hands
{
    public class PlayerHandAnimationController
    {
        private readonly Animator animator;
        private float stopGripAtNormalizedTime;
        private GripState gripState = GripState.GripReleased;
        private readonly int animationLayer;

        public PlayerHandAnimationController(Animator animator, HandSide handSide)
        {
            this.animator = animator;
            this.animationLayer = handSide.Equals(HandSide.Left) ? AnimationLayers.LeftHand : AnimationLayers.RightHand;
        }

        public void Update()
        {
            if (gripState.Equals(GripState.OnSpecificFrame)) { return; }

            var animatorStateInfo = animator.GetCurrentAnimatorStateInfo(0);

            if (gripState.Equals(GripState.Gripping) &&
                animatorStateInfo.IsName(Animations.Grip) &&
                animatorStateInfo.normalizedTime > stopGripAtNormalizedTime)
            {
                gripState = GripState.Gripped;
            }
            else if (gripState.Equals(GripState.ReleasingGrip) &&
                animatorStateInfo.IsName(Animations.ReleaseGrip) &&
                animatorStateInfo.normalizedTime >= 1)
            {
                gripState = GripState.GripReleased;
            }
        }

        public void SetStaticGrip(float normalizedTime)
        {
            animator.Play(Animations.Grip, animationLayer, normalizedTime);
            gripState = GripState.OnSpecificFrame;
        }

        public void Grip(float stopAtNormalizedTime = 1)
        {
            SetStaticGrip(stopAtNormalizedTime);//TODO: Play animation.
        }

        public void ReleaseGrip()
        {
            SetStaticGrip(0);//TODO: Play animation.
        }

        private enum GripState
        {
            GripReleased,
            ReleasingGrip,
            Gripped,
            Gripping,
            OnSpecificFrame
        }

        private static class Animations
        {
            public const string Grip = "Grip";
            public const string ReleaseGrip = "Release Grip";
        }

        private static class AnimationLayers
        {
            public const int BaseLayer = 0;
            public const int RightHand = 1;
            public const int LeftHand = 2;
        }
    }
}