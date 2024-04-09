using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class HealTarget : MonoBehaviour
    {
        [Inject] protected ICombatSystem combatSystem;

        Creature caster;
        PhysicalSpell physicalSpell;

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);

            physicalSpell = GetComponentInParent<PhysicalSpell>();
            caster = physicalSpell.correspondingSpell.GetCaster();
        }

        public void DoHeal(Creature target)
        {
            MagicHit heal = new MagicHit(caster, target, physicalSpell.correspondingSpell.modifiedDamageSources);
            combatSystem.ReportHit(heal);
        }
    }
}