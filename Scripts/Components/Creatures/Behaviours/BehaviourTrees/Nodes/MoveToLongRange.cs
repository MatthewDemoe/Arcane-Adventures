using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class MoveToLongRange : ActionNode
    {
        float tolerance = 0.1f;
        float startTime = 0.0f;

        const float MinDistance = 7.0f;
        const float MaxDistance = 9.0f;
        const float DistancingTimeout = 10.0f;

        protected override void OnStart()
        {
            if (context.agent.enabled == false)
                context.agent.enabled = true;

            context.agent.stoppingDistance = tolerance;
            startTime = Time.time;
        }

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled || (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid))
                return State.Failure;

            UpdateTargetPosition();

            if (context.agent.pathPending)
            {
                if (Time.time - startTime > DistancingTimeout && context.isRunningFromTarget)
                    context.OnCreatureDistancingTimeout.Invoke(context);

                return State.Running;
            }

            if (blackboard.distanceFromTarget < MaxDistance && blackboard.distanceFromTarget > MinDistance)
            {
                return State.Success;
            }

            return State.Running;
        }

        private void UpdateTargetPosition()
        {
            if (context.creatureBehaviour.target == null)
                return;

            Vector3 targetToSelf = context.transform.position - context.creatureBehaviour.target.position;

            if (targetToSelf.magnitude > MaxDistance)
                targetToSelf = -targetToSelf;

            blackboard.moveToPosition = context.transform.position + targetToSelf.normalized;
            blackboard.distanceFromTarget = Vector3.Distance(context.creatureBehaviour.target.position, context.agent.transform.position);
            context.agent.destination = blackboard.moveToPosition;
        }
    }
}