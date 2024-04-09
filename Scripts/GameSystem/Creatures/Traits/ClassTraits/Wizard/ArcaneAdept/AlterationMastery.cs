namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class AlterationMastery : Trait
    {
        public static AlterationMastery Instance { get; } = new AlterationMastery();
        private AlterationMastery() { }

        protected AlterationMastery(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new AlterationMastery(creature);
        }

        public override string name => "Alteration Mastery";
        public override string description => "Your mastery to use and control the bending nature of magic allows you to now add up to 3 Alteration Spells into one Base Spell.";
    }
}
