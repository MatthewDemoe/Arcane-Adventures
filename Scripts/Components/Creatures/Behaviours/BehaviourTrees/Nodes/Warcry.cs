using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class Warcry : ActionNode
    {
        AnimationEventInvoker animationEventInvoker;

        bool animationStarted = false;
        bool animationRunning = true;

        private const float MaxWaitTime = 0.5f;
        float waitTime = 0.0f;
        float startTime = 0.0f;

        protected override void OnStart()
        {
            waitTime = Random.Range(0.0f, MaxWaitTime);
            startTime = Time.time;            
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if (!animationStarted && (Time.time - startTime > waitTime))
                StartAnimation();

            if (!animationRunning)
                return State.Success;

            return State.Running;
        }

        private void StartAnimation()
        {
            animationStarted = true;
            context.animator.SetTrigger(CharacterAnimatorParameters.Warcry);

            if(!context.animator.gameObject.TryGetComponent(out animationEventInvoker))
                animationEventInvoker = context.animator.gameObject.AddComponent<AnimationEventInvoker>();

            animationEventInvoker.OnAnimationEnd.AddListener(StopAnimation);
            animationEventInvoker.OnAnimationCancelled.AddListener(StopAnimation);
        }

        private void StopAnimation()
        {
            animationRunning = false;
            animationEventInvoker.OnAnimationEnd.RemoveListener(StopAnimation);
            animationEventInvoker.OnAnimationCancelled.RemoveListener(StopAnimation);
        }
    }
}