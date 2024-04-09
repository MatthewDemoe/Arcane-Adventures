using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class MultiplyDamageFromTarget : MonoBehaviour
    {
        PhysicalTargetedSpell physicalSpell;

        Creature caster;
        Creature target;

        Spell spell;

        [SerializeField]
        float damageMultiplier = 1.0f;

        void Start()
        {
            physicalSpell = GetComponent<PhysicalTargetedSpell>();

            spell = physicalSpell.correspondingSpell;
            caster = spell.GetCaster();
        }

        public void SetTarget(Creature creature)
        {
            target = creature;
        }

        public void MultiplyDamageTaken()
        {
            caster.OnProcessHit += ProcessHit;
        }

        public void ResetDamageTaken()
        {
            caster.OnProcessHit -= ProcessHit;
        }

        private float ProcessHit(Hit hit)
        {
            if(hit.hitSource == target)
                return (int)hit.healthChange * damageMultiplier;

            return (int)hit.healthChange;
        }

        private void OnDestroy()
        {
            ResetDamageTaken();
        }
    }
}