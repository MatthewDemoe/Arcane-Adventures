namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class DestructiveTerrain : Trait
    {
        public static DestructiveTerrain Instance { get; } = new DestructiveTerrain();
        private DestructiveTerrain() { }

        protected DestructiveTerrain(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new DestructiveTerrain(creature);
        }

        public override string name => "Destructive Terrain";
        public override string description => "Spells that deal Area of Effect Damage deal 15% more damage.";
    }
}
