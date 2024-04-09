using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class MoveAwayFromTarget : ActionNode
    {
        const float RotateSpeed = 5.0f;

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            UpdateTargetPosition();

            if (context.agent.pathPending)
            {
                return State.Running;
            }

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
            {
                return State.Failure;
            }

            return State.Running;
        }

        private void UpdateTargetPosition()
        {
            if (context.creatureBehaviour.target == null)
                return;

            blackboard.fromTargetToAgent = context.agent.transform.position - context.creatureBehaviour.target.position;

            blackboard.fromTargetToAgent.y = 0.0f;

            blackboard.moveToPosition = context.agent.transform.position + (blackboard.fromTargetToAgent.normalized * context.agent.stoppingDistance * 2);
            context.agent.destination = blackboard.moveToPosition;

            Vector3 lookDirection = Vector3.RotateTowards(context.transform.forward, -blackboard.fromTargetToAgent, RotateSpeed * Time.deltaTime, 1.0f);
            context.transform.forward = lookDirection;
        }
    }
}