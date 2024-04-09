using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class BullRush : Spell
    {
        public static BullRush Instance { get; } = new BullRush();
        private BullRush() { caster = null; }
        private BullRush(ref Creature _caster) : base(ref _caster) 
        {
            spellEffects = new List<SpellEffect>()
            {
                new AddCreatureEffectToCaster(ref _caster, new CreatureEffect(
                    name: $"{name} Buff",
                    description: $"Increased weapon damage by {WeaponDamageBuff} while dashing",
                    source: $"{name} Spell",
                    weaponDamageDealt: WeaponDamageBuff
                    )),
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BullRush(ref _caster);
        }

        public override string name => "Bull Rush";
        public override string effectDescription => "Dash forward, knocking back enemies and dealing damage to them. Weapon attacks while dashing also deal increased damage.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;

        public override bool hasDuration => false;

        public override bool isSelfTargeted => false;
        protected override float _radius => 2.0f;

        protected override float _range => 25f;

        public override float cooldown => 15;

        public override float knockbackDistance => 1.5f;
        private const float WeaponDamageBuff = 1.5f;

        protected override Damage _damage => new Damage(caster.stats.subtotalStrength, Combat.DamageType.Bludgeoning);
    }
}
