namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class EmpoweringAura : Trait
    {
        public static EmpoweringAura Instance { get; } = new EmpoweringAura();
        private EmpoweringAura() { }

        protected EmpoweringAura(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new EmpoweringAura(creature);
        }

        public override string name => "Empowering Aura";
        public override string description => "Allies who are within 10ft of you gain increased Spirit Regeneration equal to 25% of your Vitality Score.";
    }
}
