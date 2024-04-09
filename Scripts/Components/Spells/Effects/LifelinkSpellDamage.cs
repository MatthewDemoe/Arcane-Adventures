using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class LifelinkSpellDamage : MonoBehaviour
    {
        [Inject] protected ICombatSystem combatSystem;

        PersistentSpellEffectDamageDealer spellDamageDealer;
        Creature caster;

        void Start()
        {
            InjectorContainer.Injector.Inject(this);

            caster = GetComponent<SpellReference>().spell.GetCaster();

            spellDamageDealer = GetComponent<PersistentSpellEffectDamageDealer>();
            spellDamageDealer.OnDealDamage.AddListener(HealCaster);
        }

        private void HealCaster(Hit hit)
        {
            StatusConditionHit heal = new StatusConditionHit(caster, -(int)hit.healthChange);

            combatSystem.ReportHit(heal);
        }
    }
}