namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ElementalRedirection : Trait
    {
        public static ElementalRedirection Instance { get; } = new ElementalRedirection();
        private ElementalRedirection() { }

        protected ElementalRedirection(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ElementalRedirection(creature);
        }

        public override string name => "Elemental Redirection";
        public override string description => "Every 15 seconds if you would be subjected to Elemental Damage you instead take 25% less damage and your next Elemental Spell deals additional damage equal to 2x your Spirit Score.";
    }
}
