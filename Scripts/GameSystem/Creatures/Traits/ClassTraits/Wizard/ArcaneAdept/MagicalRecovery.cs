namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class MagicalRecovery : Trait
    {
        public static MagicalRecovery Instance { get; } = new MagicalRecovery();
        private MagicalRecovery() { }

        protected MagicalRecovery(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new MagicalRecovery(creature);
        }

        public override string name => "Magical Recovery";
        public override string description => "You have uncovered a method of spiritual recovery, as such now when you use a spell that deals damage to a creature you gain 10% of the damage dealt back as available Spirit.";
    }
}
