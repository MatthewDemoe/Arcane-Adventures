
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    [System.Serializable]
    public class AgileBody : Trait
    {
        public static AgileBody Instance { get; } = new AgileBody();
        private AgileBody() { }

        public AgileBody(Creature creature) : base(creature)
        {
            creature.modifiers.AddEffect(new CreatureEffect(
                name: name, 
                description: description, 
                source: "Race Trait",
                moveSpeed: MoveSpeedModifier
                ));
        }

        public override Trait Get(Creature creature) 
        {
            return new AgileBody(creature); 
        }

        public override string name => "Agile Body";
        public override string description => "Movement Speed is increased by 15%";
        private const float MoveSpeedModifier = 1.15f;
    }
}
