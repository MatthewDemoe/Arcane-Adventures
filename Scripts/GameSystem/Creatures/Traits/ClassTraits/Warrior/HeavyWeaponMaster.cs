namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class HeavyWeaponMaster : Trait
    {
        public static HeavyWeaponMaster Instance { get; } = new HeavyWeaponMaster();
        private HeavyWeaponMaster() { }

        protected HeavyWeaponMaster(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new HeavyWeaponMaster(creature);
        }

        public override string name => "Heavy Weapon Master";
        public override string description => "Maneuver Weapons easier by counting them as if they were 1 Category Lower than they are. In addition you now deal 1.5x Weight Damage.";
    }
}