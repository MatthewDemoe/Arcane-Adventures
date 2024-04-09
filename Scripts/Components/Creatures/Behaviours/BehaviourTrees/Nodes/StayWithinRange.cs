using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class StayWithinRange : ActionNode
    {
        const float MinDistance = 7.0f;
        const float MaxDistance = 9.0f;
        float currentDistance = 0.0f;

        protected override void OnStart()
        {
            if (context.agent.enabled == false)
                context.agent.enabled = true;

            context.OnCreatureDistancingTimeout += ForceCreatureToBePrimaryAttacker;
        }

        protected override State OnUpdate()
        {
            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            currentDistance = Vector3.Distance(context.transform.position, context.creatureBehaviour.target.position);

            if (currentDistance > MinDistance && currentDistance < MaxDistance)
            {
                context.isRunningFromTarget = false;
                context.OnCreatureDistancingTimeout -= ForceCreatureToBePrimaryAttacker;

                return State.Success;
            }

            if(currentDistance < MinDistance)
                context.isRunningFromTarget = true;

            return State.Failure;
        }

        protected void ForceCreatureToBePrimaryAttacker(Context context)
        {
            context.mobCoordinator.ForceRoleAndReshuffle(context.creatureBehaviour, CreatureBehaviour.Role.PrimaryAttacker);

            context.OnCreatureDistancingTimeout -= ForceCreatureToBePrimaryAttacker;
        }
    }
}