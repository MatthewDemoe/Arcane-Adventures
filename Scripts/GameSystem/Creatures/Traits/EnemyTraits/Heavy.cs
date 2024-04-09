
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class Heavy : Trait
    {
        CreatureEffect heavyEffect;

        public override string name => "Heavy";
        public override string description => "This creature is unaffected by spells that force a creature's movement.";

        public static Heavy Instance { get; } = new Heavy();
        private Heavy() { }

        protected Heavy(Creature creature) : base(creature)
        {
            heavyEffect = new CreatureEffect(
                name: name, 
                description: description,
                source: name + " Trait",
                knockBackTaken: 0.0f
                );

            creature.modifiers.AddEffect(heavyEffect);
        }

        public override Trait Get(Creature creature)
        {
            return new Heavy(creature);
        }

        public override void Disable()
        {
            base.Disable();

            creature.modifiers.RemoveEffect(heavyEffect);
        }
    }
}