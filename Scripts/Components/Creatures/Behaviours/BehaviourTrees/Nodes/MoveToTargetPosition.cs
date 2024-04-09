using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class MoveToTargetPosition : ActionNode
    {
        float startTime = 0.0f;

        float CloseRangeDistance => context.creatureBehaviour.closeRangeDistance;

        const float DistancingTimeout = 5.0f;

        //TODO: Reenable when we have a proper walk backwards animation. 
        //float MinDistance => context.creatureBehaviour.maximumCloseRangeDistance;

        protected override void OnStart()
        {
            if (context.agent.enabled == false)
                context.agent.enabled = true;

            context.agent.stoppingDistance = context.creatureBehaviour.closeRangeDistance;
            startTime = Time.time;
        }

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            UpdateTargetPosition();

            if (context.agent.pathPending)
            {
                if (Time.time - startTime < DistancingTimeout)
                    return State.Running;

                context.creatureBehaviour.CloseGap();
                startTime = Time.time;
            }

            if (blackboard.distanceFromTarget < CloseRangeDistance)
                return State.Success;

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                return State.Failure;

            return State.Running;
        }

        private void UpdateTargetPosition()
        {
            if (context.creatureBehaviour.target == null)
                return;

            blackboard.distanceFromTarget = Vector3.Distance(context.creatureBehaviour.target.position, context.agent.transform.position);

            blackboard.fromTargetToAgent = context.agent.transform.position - context.creatureBehaviour.target.position;
            blackboard.fromTargetToAgent.y = 0.0f;

            Vector3 targetToSelf = context.creatureBehaviour.target.position - context.transform.position;

            blackboard.moveToPosition = context.transform.position + (targetToSelf.normalized * context.agent.stoppingDistance * 1.5f);
            context.agent.destination = blackboard.moveToPosition;
        }
    }
}