namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class PiercingStrikes : Trait
    {
        public static PiercingStrikes Instance { get; } = new PiercingStrikes();
        private PiercingStrikes() { }

        protected PiercingStrikes(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new PiercingStrikes(creature);
        }

        public override string name => "Piercing Strikes";
        public override string description => "Increase Penetration Class on Melee Weapons by 1";
    }
}
