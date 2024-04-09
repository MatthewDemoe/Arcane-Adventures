namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class OverwhelmingSpirit : Trait
    {
        public static OverwhelmingSpirit Instance { get; } = new OverwhelmingSpirit();
        private OverwhelmingSpirit() { }

        protected OverwhelmingSpirit(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new OverwhelmingSpirit(creature);
        }

        public override string name => "Overwhelming Spirit";
        public override string description => "Casting a spell that forces a creature to make a Contested Check with a -2 Penalty. A spell that affects multiple creatures causes the creatures to make the contested check with a -1 Penalty.";
    }
}
