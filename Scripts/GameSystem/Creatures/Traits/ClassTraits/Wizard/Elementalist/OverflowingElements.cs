namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class OverflowingElements : Trait
    {
        public static OverflowingElements Instance { get; } = new OverflowingElements();
        private OverflowingElements() { }

        protected OverflowingElements(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new OverflowingElements(creature);
        }

        public override string name => "Overflowing Elements";
        public override string description => "When you cast an Elemental Spell, the correlating damage type creates a small surge of activity within 5 feet of you causing creatures within this space to take Damage equal to 25% of your Spirit Score.";
    }
}
