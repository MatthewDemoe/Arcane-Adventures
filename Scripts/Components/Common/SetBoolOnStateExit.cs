using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class SetBoolOnStateExit : StateMachineBehaviour
    {
        [SerializeField] private string boolName;
        [SerializeField] private bool value;

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(boolName, value);
            base.OnStateExit(animator, stateInfo, layerIndex);
        }
    }
}