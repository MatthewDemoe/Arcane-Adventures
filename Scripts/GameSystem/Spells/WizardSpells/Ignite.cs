using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class Ignite : Spell
    {
        public static Ignite Instance { get; } = new Ignite();
        private Ignite() : base() { }
        private Ignite(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Ignite(ref _caster);
        }

        public override string name => "Ignite";
        public override string effectDescription => "Send forth your spirit and engulf your enemies in flames.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        protected override Damage _damage => new Damage(3.0f * caster.stats.subtotalSpirit, Combat.DamageType.Fire);
        public override List<Element> elements => new List<Element>() { Element.Inferno };
        protected override bool scaleDamageOverDuration => true;
        public override float channelInterval => 1.0f;

        public override float initialCost => 25;

        public override float channelCost => 0;
        protected override float baseDuration => 10;
        public override bool hasDuration => true;

        public override float cooldown => 5;

        protected override float _range => 10.0f;
    }
}
