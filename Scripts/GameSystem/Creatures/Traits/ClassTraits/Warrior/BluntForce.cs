namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class BluntForce : Trait
    {
        public static BluntForce Instance { get; } = new BluntForce();
        private BluntForce() { }

        protected BluntForce(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new BluntForce(creature);
        }

        public override string name => "Blunt Force";
        public override string description => "Staggering Opponents with Bludgeoning Weapons causes the target to stagger for an additional 2 seconds.";
    }
}
