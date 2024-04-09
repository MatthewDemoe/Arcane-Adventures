namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ShieldMaster : Trait
    {
        public static ShieldMaster Instance { get; } = new ShieldMaster();
        private ShieldMaster() { }

        protected ShieldMaster(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ShieldMaster(creature);
        }

        public override string name => "Shield Master";
        public override string description => "Damage dealt from attacks hitting your shield is blocked for an additional 15% damage.";
    }
}
