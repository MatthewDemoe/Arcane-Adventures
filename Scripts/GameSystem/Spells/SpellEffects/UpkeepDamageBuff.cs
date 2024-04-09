using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class UpkeepDamageBuff : SpellEffect
    {
        CreatureEffect creatureEffect;

        private float damageMultiplier = 0.0f;
        private float knockbackMultiplier = 0.0f;

        public UpkeepDamageBuff(ref Creature caster, float damage = 0.0f, float knockback = 0.0f) : base(ref caster)
        {
            damageMultiplier = damage;
            knockbackMultiplier = knockback;
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            creatureEffect = new CreatureEffect
            (
                name: "Damage Buff",
                description: $"Damage increased by {damageMultiplier}. \nKnockback increased by {knockbackMultiplier}.",
                source: $"{spell.name}", 
                weaponDamageDealt: damageMultiplier, 
                knockBackDealt: knockbackMultiplier
            );

            caster.modifiers.AddEffect(creatureEffect);
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.modifiers.RemoveEffect(creatureEffect);
        }
    }
}