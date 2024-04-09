using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using UnityEngine;
using UnityEngine.Events;
using Injection;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PersistentSpellEffectDamageDealer : MonoBehaviour, ISpellReferencer
    {
        [Inject] protected ICombatSystem combatSystem;

        CreatureReference hitCreatureReference;
        public PhysicalSpell physicalSpell { get; set; }

        public UnityEvent<Hit> OnDealDamage = new UnityEvent<Hit>();

        void Start()
        {
            InjectorContainer.Injector.Inject(this);
            hitCreatureReference = GetComponentInParent<CreatureReference>();
        }

        public void DealDamage()
        {
            if (hitCreatureReference.creature.isDead)
                return;

            MagicHit spellHit = new MagicHit(physicalSpell.correspondingSpell.GetCaster(), hitCreatureReference.creature, physicalSpell.correspondingSpell.modifiedDamageSources);
            
            combatSystem.ReportHit(spellHit);
            OnDealDamage.Invoke(spellHit);

            if (!hitCreatureReference.creature.isDead)
                return;

            GetComponent<SpellReference>().physicalSpell.OnEndCast.Invoke();
            GetComponent<SpellReference>().physicalSpell.OnDurationEnd.Invoke();
        }
    }
}