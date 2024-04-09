using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using UnityEngine;
using UnityEngine.Events;
using Injection;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PhysicalSpellDamageDealer : MonoBehaviour, ISpellReferencer
    {
        [Inject] protected ICombatSystem combatSystem;

        public PhysicalSpell physicalSpell { get; set; }

        public UnityEvent<Hit> OnDealDamage = new UnityEvent<Hit>();

        void Start()
        {
            InjectorContainer.Injector.Inject(this);
        }

        public void DealDamage(CreatureReference creatureReference) => DealDamage(creatureReference.creature);

        public void DealDamage(Creature creature)
        {
            if (creature.isDead)
                return;

            MagicHit spellHit = new MagicHit(physicalSpell.correspondingSpell.GetCaster(), creature, physicalSpell.correspondingSpell.modifiedDamageSources);

            combatSystem.ReportHit(spellHit);
            OnDealDamage.Invoke(spellHit);
        }
    }
}