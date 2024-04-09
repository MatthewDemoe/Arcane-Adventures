
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    //TODO: Prevent instant kill effects 
    public class BossMonster : Trait
    {
        CreatureEffect bossMonsterEffect;

        public override string name => "Boss Monster";
        public override string description => "All Crowd Control Durations are reduced by 50%. Immune to Instant Kill effects.";

        public static BossMonster Instance { get; } = new BossMonster();
        private BossMonster() { }

        protected BossMonster(Creature creature) : base(creature)
        {
            bossMonsterEffect = new CreatureEffect(
                name: name,
                description: description,
                source: name + " Trait",
                statusConditionDurationModifier: 0.5f
                );

            creature.modifiers.AddEffect(bossMonsterEffect);
        }

        public override Trait Get(Creature creature)
        {
            return new BossMonster(creature);
        }

        public override void Disable()
        {
            base.Disable();
        }
    }
}