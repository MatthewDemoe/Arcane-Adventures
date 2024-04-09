using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class BuffNextWeaponAttack : SpellEffect
    {
        private float damageMultiplier = 0.0f;
        private float knockbackMultiplier = 0.0f;
        private float criticalHitChance = 0.0f;
        private StrikeType _strikeType = StrikeType.NotStrike;

        CreatureEffect creatureEffect;

        public BuffNextWeaponAttack(ref Creature caster, float damage = 0.0f, float knockback = 0.0f, float criticalHitChance = 0.0f, StrikeType strikeType = StrikeType.NotStrike) : base(ref caster)
        {
            damageMultiplier = damage;
            knockbackMultiplier = knockback;
            this.criticalHitChance = criticalHitChance;
            _strikeType = strikeType;
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            if (_strikeType == StrikeType.NotStrike)
            {
                creatureEffect = new CreatureEffect(
                    name: $"{spell.name} Buff",
                    description: $"Increase weapon damage by {damageMultiplier}, knockback by {knockbackMultiplier}, and critical chance by {criticalHitChance}.",
                    source: $"{spell.name}",
                    weaponDamageDealt: damageMultiplier,
                    knockBackDealt: knockbackMultiplier, 
                    criticalHitChance: criticalHitChance
                    );
            }

            else
            {
                creatureEffect = new CreatureEffect(
                    name: $"{spell.name} Buff",
                    description: $"Increase weapon damage by {damageMultiplier}, knockback by {knockbackMultiplier}, for {_strikeType}s.",
                    source: $"{spell.name}",
                    DamageMultiplierByStrikeType: new Dictionary<StrikeType, float>() { { _strikeType, damageMultiplier} },                     
                    KnockbackMultiplierByStrikeType: new Dictionary<StrikeType, float>() { { _strikeType, knockbackMultiplier } }
                    );
            }

            caster.modifiers.AddEffect(creatureEffect);
            caster.OnWeaponHitReported += RemoveBuff;
        }

        private void RemoveBuff(Creature target, StrikeType strikeType)
        {
            caster.modifiers.RemoveEffect(creatureEffect);
            caster.OnWeaponHitReported -= RemoveBuff;
        }
    }
}