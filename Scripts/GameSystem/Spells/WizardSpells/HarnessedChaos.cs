using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class HarnessedChaos : ProjectileSpell
    {
        public static HarnessedChaos Instance { get; } = new HarnessedChaos();
        private HarnessedChaos() : base() { }
        private HarnessedChaos(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new HarnessedChaos(ref _caster);
        }

        public override string name => "Harnessed Chaos";
        public override string effectDescription => "Create and wield a ball of pure arcana. Fire it at an enemy, or use it as a melee weapon.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;
        protected override float _radius => 0f;
        public override bool hasDuration => false;
        public override float channelDuration => 0.25f;
        public override float channelInterval => 0.1f;

        public override float cooldown => 15;

        protected override float _range => 40.0f;
        protected override float _force => CombatSettings.Spells.ProjectileForce;
        public override float knockbackDistance { get; } = 1.0f;
        protected override Damage _damage => new Damage(3.0f * caster.stats.subtotalSpirit, Combat.DamageType.Magical);
    }
}
