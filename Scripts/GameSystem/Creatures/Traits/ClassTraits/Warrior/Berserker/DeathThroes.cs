namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class DeathThroes : Trait
    {
        public static DeathThroes Instance { get; } = new DeathThroes();
        private DeathThroes() { }

        public DeathThroes(Creature creature) : base(creature) { }

        public override Trait Get(Creature creature)
        {
            return new DeathThroes(creature);
        }

        public override string name => "Death Throes";
        public override string description => "Whenever you are dropped to 0 hit points, you instead drop to 1 hit point and may fight on for an additional 15 seconds before dying. In this state all health you would gain is reduced by 50% and if you end during the State with more than 25% of your Maximum health you do not die after this trait's duration. This feature may only be used once every 5 minutes.";

    }
}
