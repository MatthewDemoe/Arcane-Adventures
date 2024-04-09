using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class WaterGeyser : Spell
    {
        public static WaterGeyser Instance { get; } = new WaterGeyser();
        private WaterGeyser() : base() { }
        private WaterGeyser(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new WaterGeyser(ref _caster);
        }

        public override string name => "Water Geyser";
        public override string effectDescription => "Create an intense jet of water that damages and pushes back enemies.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        protected override Damage _damage => new Damage(1.5f * caster.stats.subtotalSpirit, Combat.DamageType.Bludgeoning);
        public override List<Element> elements => new List<Element>() { Element.Ocean };
        protected override bool scaleDamageOverDuration => true;

        public override float initialCost => 25;

        public override float channelCost => 0;
        protected override float baseDuration => 1.0f;
        public override bool hasDuration => true;

        public override float cooldown => 10.0f;

        protected override float _range => 3.0f;
        protected override float _radius => 0.5f;

        public override float knockbackDistance => 1.5f;
    }
}
