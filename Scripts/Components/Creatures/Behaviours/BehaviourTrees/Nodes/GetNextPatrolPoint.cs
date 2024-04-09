using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class GetNextPatrolPoint : ActionNode
    {
        int currentPatrolPoint = 0;

        protected override void OnStart()
        {
            UpdatePatrolPoint();
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }

        private void UpdatePatrolPoint()
        {
            Vector3 nextPatrolPoint = context.mobCoordinator.patrolPoints[currentPatrolPoint];
            currentPatrolPoint = (currentPatrolPoint + 1) % (context.mobCoordinator.patrolPoints.Count);

            blackboard.moveToPosition = nextPatrolPoint;
            context.agent.SetDestination(blackboard.moveToPosition);
        }
    }
}