namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ExpansiveKnowledge : Trait
    {
        public static ExpansiveKnowledge Instance { get; } = new ExpansiveKnowledge();
        private ExpansiveKnowledge() { }

        protected ExpansiveKnowledge(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ExpansiveKnowledge(creature);
        }

        public override string name => "Expansive Knowledge";
        public override string description => "You learn 2 additional spells and you can now memorize one additional spell per Spell Category.";
    }
}