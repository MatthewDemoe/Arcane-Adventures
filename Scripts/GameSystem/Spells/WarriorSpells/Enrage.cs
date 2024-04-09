using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class Enrage : Spell
    {
        public static Enrage Instance { get; } = new Enrage();
        private Enrage() { caster = null; }
        private Enrage(ref Creature _caster) 
        {
            caster = _caster;

            spellEffects = new List<SpellEffect>()
            {
                new AddStatusConditionToCaster(ref caster, new Frenzied(caster, new FrenziedStatusSettings(durationInMilliseconds: 0), name)),
            };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Enrage(ref _caster);
        }

        public override string name => "Enrage";
        public override string effectDescription => "Your emotions intertwine with your spirit and react to your fury, making you frenzied.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 0;
        public override float upkeepCost => 5;
        public override bool hasUpkeep => true;

        public override float cooldown => 20;
    }
}
