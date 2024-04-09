using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    public abstract class EnemyBehaviour : CreatureBehaviour
    {
        public override Team team => Team.Enemy;

        protected override void Initialize(CreatureReference creatureReference)
        {
            base.Initialize(creatureReference);

            animationEventInvoker = GetComponentInChildren<AnimationEventInvoker>();
            creatureReference.creature.OnMovementToggled += () => { navMeshAgent.enabled = creatureReference.creature.isMovementEnabled; };
        }

        protected bool GetIsAttacking(List<string> animatorParameters)
        {
            return animatorParameters.Any(attack => animator.GetBool(attack));
        }
    }
}
