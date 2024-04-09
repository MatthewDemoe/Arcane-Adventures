using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using System;
using UnityEngine;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponFunctionality
{
    [RequireComponent(typeof(PhysicalHandHeldItem))]
    public class HealWielderComponent : MonoBehaviour
    {
        [Inject] protected ICombatSystem combatSystem;

        [SerializeField]
        float healPercent;

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
        }

        public void HealWielder()
        {
            if (healPercent <= 0.0f)
                throw new Exception($"{nameof(healPercent)} is less than 0");

            Creature wielder = GetComponent<PhysicalHandHeldItem>().wielder.creature;

            StatusConditionHit healthGain = new StatusConditionHit(wielder, (int)(-wielder.stats.maxHp * healPercent));
            combatSystem.ReportHit(healthGain);
        }
    }
}