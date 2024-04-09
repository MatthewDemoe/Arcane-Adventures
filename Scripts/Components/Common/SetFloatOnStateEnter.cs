using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class SetFloatOnStateEnter : StateMachineBehaviour
    {
        [SerializeField] private string floatName;
        [SerializeField] private float value;

        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetFloat(floatName, value);
            base.OnStateEnter(animator, stateInfo, layerIndex);
        }
    }
}