using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    public class AnimatorLookAtPositionUpdater : MonoBehaviour
    {
        private Animator animator;
        public Transform target;

        void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        void OnAnimatorIK()
        {
            if (target == null)
            {
                animator.SetLookAtWeight(0);
            }
            else
            {
                animator.SetLookAtWeight(weight: 0.75f, bodyWeight: 0, headWeight: 1, eyesWeight: 1, clampWeight: 0.75f);
                animator.SetLookAtPosition(target.position);
            }
        }
    }
}