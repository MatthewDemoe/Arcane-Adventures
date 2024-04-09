using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class Attack : ActionNode
    {
        bool attackPerformed = false;
        const float RotateSpeed = 5.0f;

        protected override void OnStart()
        {
            if (!context.creatureBehaviour.isAttacking)
            {
                attackPerformed = context.creatureBehaviour.Attack();
                context.agent.enabled = false;
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            context.agent.enabled = true;
        }

        protected override State OnUpdate()
        {
            if (!attackPerformed)
                return State.Success;

            if (context.creatureBehaviour.isAttacking)
                return State.Running;
            
            context.creatureBehaviour.FinishAttack();

            return State.Success;
        }
    }
}