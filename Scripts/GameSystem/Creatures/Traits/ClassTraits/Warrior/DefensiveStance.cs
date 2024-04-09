namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class DefensiveStance : Trait
    {
        public static DefensiveStance Instance { get; } = new DefensiveStance();
        private DefensiveStance() { }

        protected DefensiveStance(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new DefensiveStance(creature);
        }

        public override string name => "Defensive Stance";
        public override string description => "After not moving 2ft from a position for 5 seconds you gain a 10% damage reduction buff for 5 seconds.";
    }
}