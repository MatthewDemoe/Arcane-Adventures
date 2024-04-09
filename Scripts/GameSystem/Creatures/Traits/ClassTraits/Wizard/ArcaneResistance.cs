namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ArcaneResistance : Trait
    {
        public static ArcaneResistance Instance { get; } = new ArcaneResistance();
        private ArcaneResistance() { }

        protected ArcaneResistance(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ArcaneResistance(creature);
        }

        public override string name => "Arcane Resistance";
        public override string description => "You gain a +5 to resist all Contested Checks against Spell effects.";
    }
}