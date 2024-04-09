using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class Shockwave : Spell
    {
        public static Shockwave Instance { get; } = new Shockwave();
        private Shockwave() : base() { }
        private Shockwave(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Shockwave(ref _caster);
        }

        public override string name => "Shockwave";
        public override string effectDescription => "Spirit erupts from you, creating a shockwave in all directions that deals damage and knocks back enemies.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 30.0f;

        public override float channelCost => 0.0f;
        protected override float baseDuration => 0.0f;

        protected override float _radius => 3;
        public override bool hasDuration => false;

        public override float cooldown => 15.0f;

        public override float knockbackDistance { get; } = 3.0f;
        public override bool isSelfTargeted => true;

        protected override Damage _damage => new Damage(2.0f * caster.stats.subtotalSpirit, DamageType.Magical);
    }
}
