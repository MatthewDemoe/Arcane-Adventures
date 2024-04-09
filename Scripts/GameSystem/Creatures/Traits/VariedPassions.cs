

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    [System.Serializable]
    public class VariedPassions : Trait
    {
        CreatureEffect variedPassionsEffect;

        public static VariedPassions Instance { get; } = new VariedPassions();

        protected VariedPassions() : base() { }

        protected VariedPassions(Creature creature) : base(creature)
        {
            UpdateCreatureEffect();

            creature.OnLevelUp += UpdateCreatureEffect;
        }

        ~VariedPassions()
        {
            creature.OnLevelUp -= UpdateCreatureEffect;
        }

        public override Trait Get(Creature creature)
        {
            return new VariedPassions(creature);
        }

        public override string name => "Varied Passions";
        public override string description => $"You gain an additional {initialStatBoost} Attribute Points to allocate after choosing the human. Gain an additional Attribute Point every {statBoostAfterNumLevels} levels. Gain an additional General Trait from your Base Class.";

        private const int initialStatBoost = 3;
        private const int statBoostAfterNumLevels = 2;

        private void UpdateCreatureEffect()
        {
            variedPassionsEffect = new CreatureEffect(
                name: name,
                description: $"You gain an additional {initialStatBoost} Attribute Points, and an additional Attribute Point every {statBoostAfterNumLevels} levels. Gain an additional General Trait from your Base Class.",
                source: name + " Trait",
                traitStatPoints: initialStatBoost + (creature.stats.level > 1 ? ((creature.stats.level - 1) / statBoostAfterNumLevels) : 0)
                );

            creature.stats.SetTraitStatPoints(variedPassionsEffect);
        }
    }
}
