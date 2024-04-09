namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class ElementalAttunement : Trait
    {
        public static ElementalAttunement Instance { get; } = new ElementalAttunement();
        private ElementalAttunement() { }

        protected ElementalAttunement(Creature creature) : base(creature)
        {

        }

        public override Trait Get(Creature creature)
        {
            return new ElementalAttunement(creature);
        }

        public override string name => "Elemental Attunement";
        public override string description => "You may choose an Element of your choice that causes spells of that type to deal an additional 5% damage, in addition you gain resistance to the most common damage type of that Element. (You may choose this Trait more than once by selecting a separate element type.";
    }
}
