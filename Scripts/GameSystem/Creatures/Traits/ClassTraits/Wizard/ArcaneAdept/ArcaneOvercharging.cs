namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ArcaneOvercharging : Trait
    {
        public static ArcaneOvercharging Instance { get; } = new ArcaneOvercharging();
        private ArcaneOvercharging() { }

        protected ArcaneOvercharging(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ArcaneOvercharging(creature);
        }

        public override string name => "Arcane Overcharging";
        public override string description => "From your experiments with Enhancement magic you now have a greater understanding of the potential from your spells, allowing spells to critically hit and gaining a 5% critical hit chance.";
    }
}
