using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class Brutalize : Spell
    {
        public static Brutalize Instance { get; } = new Brutalize();
        private Brutalize() { caster = null; }
        private Brutalize(ref Creature _caster) : base(ref _caster) 
        {
            spellEffects = new List<SpellEffect>()
            {
                new BuffNextWeaponAttack(ref _caster, damage: 3.0f, criticalHitChance: 1.25f),
            };
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Brutalize(ref _caster);
        }

        public override string name => "Brutalize";
        public override string effectDescription => "Your rage fuels your next strike, increasing its damage and crit chance. Killing an enemy with this strike sends fear into the hearts of other foes.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 50;

        public override float cooldown => 30;
    }
}
