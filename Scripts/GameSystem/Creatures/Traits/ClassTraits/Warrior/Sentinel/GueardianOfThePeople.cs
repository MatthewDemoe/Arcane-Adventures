namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class GuardianOfThePeople : Trait
    {
        public static GuardianOfThePeople Instance { get; } = new GuardianOfThePeople();
        private GuardianOfThePeople() { }

        protected GuardianOfThePeople(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new GuardianOfThePeople(creature);
        }

        public override string name => "Guardian Of The People";
        public override string description => "Enemies are more likely to target you, in addition enemies make Contested checks with a -5 penalty to resist being taunted by you. Taunted enemies also remain taunted by you for an additional 5 seconds.";
    }
}
