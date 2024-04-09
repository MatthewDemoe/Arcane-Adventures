namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ComplexCaster : Trait
    {
        public static ComplexCaster Instance { get; } = new ComplexCaster();
        private ComplexCaster() { }

        protected ComplexCaster(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ComplexCaster(creature);
        }

        public override string name => "Complex Caster";
        public override string description => "Reduce the cost of all Enhancement spells by 25%.";
    }
}
