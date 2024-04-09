
namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    [System.Serializable]
    public abstract class Trait {
        public abstract string name { get; }
        public abstract string description { get; }

        protected Creature creature;

        public Trait() { }

        public Trait(Creature creature)
        {
            this.creature = creature;
        }

        public abstract Trait Get(Creature creature);

        public static Trait GetTrait<T>(T traitType, Creature creature) where T : Trait
        {
            return traitType.Get(creature);
        }

        public virtual void Disable() { }
    }
}
