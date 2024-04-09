namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class AgileWeaponry : Trait
    {
        public static AgileWeaponry Instance { get; } = new AgileWeaponry();
        private AgileWeaponry() { }

        protected AgileWeaponry(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new AgileWeaponry(creature);
        }

        public override string name => "Agile Weaponry";
        public override string description => "If you are wielding a Weapon that has a Strength Requirement 20 less than your Total Strength Score. You gain +1 to your Penetration Class and a 5% increase in damage.";
    }
}
