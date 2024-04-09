namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class AdaptiveFighting : Trait
    {
        public static AdaptiveFighting Instance { get; } = new AdaptiveFighting();
        private AdaptiveFighting() { }

        protected AdaptiveFighting(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new AdaptiveFighting(creature);
        }

        public override string name => "Adaptive Fighting";
        public override string description => "Each time a Creature damages you with a Weapon Attack you reduce their future Weapon Damage by 1.5%. This ability stacks 5x and loses a stack every 5 seconds the creature does not attack you.";
    }
}
