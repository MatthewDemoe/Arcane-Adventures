namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class Alchemist : Trait
    {
        public static Alchemist Instance { get; } = new Alchemist();
        private Alchemist() { }

        protected Alchemist(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new Alchemist(creature);
        }

        public override string name => "Alchemist";
        public override string description => "When your Elementalist spells deal additional damage because of Element Combinations you deal additional damage equal to 50% of your Spirit Score";
    }
}
