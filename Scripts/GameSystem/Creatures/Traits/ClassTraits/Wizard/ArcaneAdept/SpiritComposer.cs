namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class SpiritComposer : Trait
    {
        public static SpiritComposer Instance { get; } = new SpiritComposer();
        private SpiritComposer() { }

        protected SpiritComposer(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new SpiritComposer(creature);
        }

        public override string name => "Spirit Composer";
        public override string description => "When you cast Alteration Magic you may choose to target an ally after casting this spell. This Alteration Magic then affects the next spell they cast.";
    }
}
