using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class StayOutsideAngle : ActionNode
    {
        const float MinAngle = 30.0f;

        protected override void OnStart()
        {
            if (context.agent.enabled == false)
                context.agent.enabled = true;
        }

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            UpdateAngle();

            if (blackboard.angleAroundTarget > MinAngle)
                return State.Success;

            return State.Failure;
        }

        private void UpdateAngle()
        {
            blackboard.fromTargetToAgent = context.agent.transform.position - context.creatureBehaviour.target.position;
            blackboard.fromTargetToAgent.y = 0.0f;
            blackboard.angleAroundTarget = Vector3.Angle(context.creatureBehaviour.target.forward, blackboard.fromTargetToAgent.normalized);
        }
    }
}