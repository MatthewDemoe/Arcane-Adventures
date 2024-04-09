using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class DisarmingManeuver : Spell
    {
        public static DisarmingManeuver Instance { get; } = new DisarmingManeuver();
        private DisarmingManeuver() { caster = null; }
        private DisarmingManeuver(ref Creature _caster) { caster = _caster; }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new DisarmingManeuver(ref _caster);
        }

        public override string name => "Disarming Maneuver";
        public override string effectDescription => "Imbue your weapon with spirit to try and disarm your opponent.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 25;
        public override bool hasDuration => true;
        protected override float baseDuration => 5;

        public override float cooldown => 15;
    }
}
