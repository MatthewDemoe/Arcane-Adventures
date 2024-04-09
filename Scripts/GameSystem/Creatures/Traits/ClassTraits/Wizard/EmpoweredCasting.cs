namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class EmpoweredCasting : Trait
    {
        public static EmpoweredCasting Instance { get; } = new EmpoweredCasting();
        private EmpoweredCasting() { }

        protected EmpoweredCasting(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new EmpoweredCasting(creature);
        }

        public override string name => "Empowered Casting";
        public override string description => "Increase the damage your spells deal by 5%.";

    }
}
