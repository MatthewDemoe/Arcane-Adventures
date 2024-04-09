using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class MovePerpendicularToTarget : ActionNode
    {
        public float tolerance = 4.0f;
        public float angleTolerance = 120;

        protected override void OnStart()
        {
            context.agent.destination = blackboard.moveToPosition;
            context.agent.stoppingDistance = tolerance;
        }

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            UpdateTargetPosition();

            if (context.agent.pathPending)
            {
                return State.Running;
            }

            if (blackboard.angleAroundTarget >= angleTolerance)
            {
                return State.Success;
            }

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid
                || blackboard.distanceFromTarget >= tolerance)
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

            blackboard.distanceFromTarget = Vector3.Distance(context.creatureBehaviour.target.position, context.agent.transform.position);
            blackboard.angleAroundTarget = Vector3.Angle(context.creatureBehaviour.target.forward, blackboard.fromTargetToAgent.normalized);            

            blackboard.moveToPosition = context.agent.transform.position + (Vector3.Cross(blackboard.fromTargetToAgent.normalized, Vector3.up).normalized * 10.0f);
            context.agent.destination = blackboard.moveToPosition;
        }
    }
}