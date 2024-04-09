namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class Revenge : Trait
    {
        public static Revenge Instance { get; } = new Revenge();
        private Revenge() { }

        protected Revenge(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new Revenge(creature);
        }

        public override string name => "Revenge";
        public override string description => "When you are struck by an Attack within 5ft of yourself, you deal 25% of your Vitality Score back as Damage to the attacker.";
    }
}
