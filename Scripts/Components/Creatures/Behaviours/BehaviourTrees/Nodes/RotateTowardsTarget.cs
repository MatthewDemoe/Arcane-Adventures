using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class RotateTowardsTarget : ActionNode
    {
        const float RotateSpeed = 5.0f;
        const float MinimumAngle = 10.0f;

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            if (context.transform == context.creatureBehaviour.target)
                return State.Success;

            PerformRotation();

            if (Vector3.Angle(context.transform.forward, -blackboard.fromTargetToAgent) > MinimumAngle)
                return State.Running;

            return State.Success;
        }

        private void PerformRotation()
        {
            blackboard.fromTargetToAgent = context.agent.transform.position - context.creatureBehaviour.target.position;
            blackboard.fromTargetToAgent.y = 0.0f;

            Vector3 lookDirection = Vector3.RotateTowards(context.transform.forward, -blackboard.fromTargetToAgent, RotateSpeed * Time.deltaTime, 1.0f);
            context.transform.forward = lookDirection;
        }
    }
}