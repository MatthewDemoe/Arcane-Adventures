namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class HeroicPresence : Trait
    {
        public static HeroicPresence Instance { get; } = new HeroicPresence();
        private HeroicPresence() { }

        protected HeroicPresence(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new HeroicPresence(creature);
        }

        public override string name => "Heroic Presence";
        public override string description => "When you are within 5ft of an Ally they gain the Motivated Condition, you are also immune to the Demotivated Condition.";
    }
}
