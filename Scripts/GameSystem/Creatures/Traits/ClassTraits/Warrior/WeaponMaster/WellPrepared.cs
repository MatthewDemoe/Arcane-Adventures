namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class WellPrepared : Trait
    {
        public static WellPrepared Instance { get; } = new WellPrepared();
        private WellPrepared() { }

        protected WellPrepared(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new WellPrepared(creature);
        }
        public override string name => "Well Prepared";
        public override string description => "When you take multiple Weapons into a mission you gain a 2.5% increase to your Damage for each unique Weapon brought.";
    }
}
