using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class ClearTarget : ActionNode
    {
        protected override void OnStart()
        {
            blackboard.moveToPosition = context.agent.transform.position;
        }

        protected override void OnStop()
        {
            context.agent.enabled = true;
        }

        protected override State OnUpdate()
        {
            if (blackboard.moveToPosition == null)
                return State.Failure;

            return State.Success;
        }
    }
}