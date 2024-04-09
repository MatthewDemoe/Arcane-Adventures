namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class RagingSpirit : Trait
    {
        public static RagingSpirit Instance { get; } = new RagingSpirit();
        private RagingSpirit() { }

        protected RagingSpirit(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new RagingSpirit(creature);
        }

        public override string name => "Raging Spirit";
        public override string description => "You gain a +5 to resist all contested Spirit checks in addition to increasing your Spirit Pool by adding 50% of your Vitality Score per level.";
    }
}
