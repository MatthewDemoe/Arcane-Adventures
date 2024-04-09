using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class ElementalCasting : Spell
    {
        public static ElementalCasting Instance { get; } = new ElementalCasting();
        private ElementalCasting() : base() { }
        private ElementalCasting(ref Creature _caster) : base(ref _caster)
        {
            spellEffects = new List<SpellEffect>()
            {
                new BuffNextSpell(ref caster, new Damage(character.stats.subtotalSpirit * 1.5f, DamageType.Fire)),
            };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ElementalCasting(ref _caster);
        }

        public override string name => "Elemental Casting";
        public override string effectDescription => "Twist the nature of your spells to deal extra damage and overcome resistances.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 10.0f;

        public override float channelCost => 0.0f;
        protected override float baseDuration => 0.0f;
        public override bool hasDuration => false;

        public override float cooldown => 10;
    }
}
