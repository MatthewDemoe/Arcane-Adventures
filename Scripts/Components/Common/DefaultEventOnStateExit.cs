using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class DefaultEventOnStateExit : StateMachineBehaviour
    {
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            base.OnStateExit(animator, stateInfo, layerIndex);

            if (animator.TryGetComponent(out AnimationEventInvoker animationEventInvoker))
                animationEventInvoker.CancelAnimation();
        }
    }
}