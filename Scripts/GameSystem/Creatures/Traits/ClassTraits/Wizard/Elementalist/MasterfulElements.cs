namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class MasterfulElements : Trait
    {
        public static MasterfulElements Instance { get; } = new MasterfulElements();
        private MasterfulElements() { }

        protected MasterfulElements(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new MasterfulElements(creature);
        }

        public override string name => "MasterfulElements";
        public override string description => "Your Elementalist Spells deal an additional 10% damage to creatures who are neither Reistant nor Immune to your Damage Type.";
    }
}
