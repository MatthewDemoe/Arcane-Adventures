using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class BloodBoil : Spell
    {
        public static BloodBoil Instance { get; } = new BloodBoil();
        private BloodBoil() { caster = null; }
        private BloodBoil(ref Creature _caster) { caster = _caster; }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BloodBoil(ref _caster);
        }

        public override string name => "Blood Boil";
        public override string effectDescription => "Using your Spirit you affect your body, increasing your heart rate, and drastically increasing your body temperature.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 0.0f;

        public override float channelCost => 5.0f;
        protected override float baseDuration => 0.0f;
        public override bool hasDuration => false;

        public override float cooldown => 10.0f;

    }
}
