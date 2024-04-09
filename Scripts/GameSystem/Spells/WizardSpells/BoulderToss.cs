using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class BoulderToss : Spell
    {
        public static BoulderToss Instance { get; } = new BoulderToss();
        private BoulderToss() : base() { }
        private BoulderToss(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BoulderToss(ref _caster);
        }

        public override string name => "Boulder Toss";
        public override string effectDescription => "Conjure a large boulder and hurl it towards enemies, dealing damage and knocking back.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override bool isSelfTargeted => false;
        public override float channelDuration => 3.0f;
        public override float channelInterval => 0.1f;

        public override float initialCost => 30;

        public override float cooldown => 15.0f;

        protected override float _range => 10.0f;
        protected override float _radius => 1.0f;
        protected override float _force => 1000.0f;
        public override float knockbackDistance => 2.0f;

        protected override Damage _damage => new Damage(caster.stats.subtotalSpirit * 3, Combat.DamageType.Bludgeoning);
        public override List<Element> elements => new List<Element>() { Element.Earth };
    }
}
