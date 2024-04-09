namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class DominantSpells : Trait
    {
        public static DominantSpells Instance { get; } = new DominantSpells();
        private DominantSpells() { }

        protected DominantSpells(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new DominantSpells(creature);
        }

        public override string name => "Dominant Spells";
        public override string description => "Your Spells that force Contested Checks are made with a -2 Penalty.";
    }
}
