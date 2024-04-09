using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class FavoredWind : Spell
    {
        public static FavoredWind Instance { get; } = new FavoredWind();
        private FavoredWind() : base() { }
        private FavoredWind(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new FavoredWind(ref _caster);
        }

        public override string name => "Favored Wind";
        public override string effectDescription => "Conjure heavy winds and use them to your advantage. Hastes allies and slow enemies with the direction of the wind.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;

        public override float initialCost => 25;
        public override bool hasUpkeep => true;
        public override float upkeepCost => 5.0f;

        public override float cooldown => 10.0f;

        protected override float _range => 4.5f;
        public override bool isRangeCentered => true;
        public override bool isSelfTargeted => true;
        protected override float _radius => 4.5f;
    }
}
