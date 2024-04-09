namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class PhysicalArcaneManipulation : Trait
    {
        public static PhysicalArcaneManipulation Instance { get; } = new PhysicalArcaneManipulation();
        private PhysicalArcaneManipulation() { }

        protected PhysicalArcaneManipulation(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new PhysicalArcaneManipulation(creature);
        }

        public override string name => "Physical Arcane Manipulation";
        public override string description => "20% of your Vitality Score is added to your Spirit Score.";
    }
}
