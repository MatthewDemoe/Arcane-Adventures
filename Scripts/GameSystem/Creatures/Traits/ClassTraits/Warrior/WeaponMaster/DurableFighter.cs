namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class DurableFighter : Trait
    {
        public static DurableFighter Instance { get; } = new DurableFighter();
        private DurableFighter() { }

        protected DurableFighter(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new DurableFighter(creature);
        }

        public override string name => "Durable Fighter";
        public override string description => "Your Crowd Control Reduction increases by 20%.";
    }
}
