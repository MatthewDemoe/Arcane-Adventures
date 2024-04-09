using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class RendingWind : Spell
    {
        public static RendingWind Instance { get; } = new RendingWind();
        private RendingWind() { caster = null; }
        private RendingWind(ref Creature _caster) { caster = _caster; }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new RendingWind(ref _caster);
        }


        public override string name => "Rending Wind";
        public override string effectDescription => "Manifest your Spirit as it slices through the air cutting into your foes.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25;
        public override float channelCost => 0;
        protected override float baseDuration => 5;
        public override bool hasDuration => true;

        public override float cooldown => 5;
    }
}