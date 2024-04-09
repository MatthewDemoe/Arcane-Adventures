namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class GreaterSpiritualConnection : Trait
    {
        public static GreaterSpiritualConnection Instance { get; } = new GreaterSpiritualConnection();
        private GreaterSpiritualConnection() { }

        protected GreaterSpiritualConnection(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new GreaterSpiritualConnection(creature);
        }

        public override string name => "Greater Spiritual Connection";
        public override string description => "Increases Spirit Regeneration by 20% of your Spirit Score.";
    }
}