namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class EnduranceTraining : Trait
    {
        public static EnduranceTraining Instance { get; } = new EnduranceTraining();
        private EnduranceTraining() { }

        protected EnduranceTraining(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new EnduranceTraining(creature);
        }

        public override string name => "Endurance Training";
        public override string description => "Reduce movement penalty from armour by 50% and gain 2 Attribute Points to spend on Vitality or Strength.";
    }
}
