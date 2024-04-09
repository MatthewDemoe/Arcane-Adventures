using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(EnemySpellCaster))]
    public abstract class SpellCastingEnemyBehaviour : EnemyBehaviour
    {
        protected EnemySpellCaster enemySpellCaster;

        protected override void Initialize(CreatureReference creatureReference)
        {
            base.Initialize(creatureReference);

            enemySpellCaster = GetComponent<EnemySpellCaster>();
            animationEventInvoker.OnAnimationCancelled.AddListener(ResetTarget);
        }

        protected bool TryCastSpell(Spell spell)
        {
            if (enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(spell))
                return false;

            enemySpellCaster.TryEquipSpell(spell, HandSide.Right);

            animationEventInvoker.OnAnimationEnd.AddListener(StartCast);
            animationEventInvoker.OnAnimationEnd.AddListener(FinishCast);

            return true;
        }

        protected void FinishCast()
        {
            enemySpellCaster.FinishCast(HandSide.Right);
            animationEventInvoker.OnAnimationEnd.RemoveListener(FinishCast);
        }

        protected void StartCast()
        {
            enemySpellCaster.StartCast(HandSide.Right);
            animationEventInvoker.OnAnimationEnd.RemoveListener(StartCast);
        }
    }
}
