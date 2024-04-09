using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class ShieldFromDamage : MonoBehaviour
    {
        [SerializeField]
        Stats.Stat shieldHealthStat;

        [SerializeField]
        float shieldHealthStatMultiplier;

        Creature caster;

        [SerializeField]
        [ReadOnly]
        float barrierHealth = 0;

        public UnityEvent OnBarrierDestroyed = new UnityEvent();

        public void InitializeBarrier()
        {
            caster = GetComponentInParent<PhysicalSpell>().correspondingSpell.GetCaster();

            barrierHealth = caster.stats.totalStatsByName[shieldHealthStat] * shieldHealthStatMultiplier;

            caster.preventDamage = true;
            caster.OnProcessHit += RedirectHitDamage;
        }

        private float RedirectHitDamage(Hit hit)
        {
            hit.CalculateHealthChange();

            barrierHealth -= hit.healthChange.Value;

            if (barrierHealth <= 0.0f)
                DestroyBarrier();

            return 0.0f;
        }

        private void DestroyBarrier()
        {
            caster.preventDamage = false;
            caster.stats.AdjustHealth(-(int)barrierHealth);

            caster.OnProcessHit -= RedirectHitDamage;
            OnBarrierDestroyed.Invoke();
        }

        private void OnDestroy()
        {
            caster.preventDamage = false;
        }
    }
}