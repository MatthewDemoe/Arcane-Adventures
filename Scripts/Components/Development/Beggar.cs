using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    [RequireComponent(typeof(Animator))]
    public class Beggar : MonoBehaviour
    {
        private Animator animator;
        private Transform target;

        void Start()
        {
            animator = GetComponent<Animator>();
            target = Camera.main.transform;
        }

        void OnAnimatorIK()
        {
            var weight = 0;

            if (target != null)
            {
                animator.SetLookAtPosition(target.position);
                animator.SetIKPosition(AvatarIKGoal.RightHand, target.position);
                weight = 1;
            }

            animator.SetLookAtWeight(weight);
            animator.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
        }
    }
}