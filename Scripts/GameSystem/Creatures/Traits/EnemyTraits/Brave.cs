using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class Brave : Trait
    {
        CreatureEffect braveEffect;

        public override string name => "Brave";
        public override string description => " Immune to the Feared Condition, and +2 to Resist Spirit Contested Checks.";

        public static Brave Instance { get; } = new Brave();
        private Brave() { }

        protected Brave(Creature creature) : base(creature)
        {
            braveEffect = new CreatureEffect(
                name: name,
                description: description,
                source: name + " Trait",
                spiritContestedCheckBonus: 2,
                immuneStatusConditions: new List<AllStatusConditions.StatusConditionName>() { AllStatusConditions.StatusConditionName.Feared }
                );

            creature.modifiers.AddEffect(braveEffect);
        }

        public override Trait Get(Creature creature)
        {
            return new Brave(creature);
        }

        public override void Disable()
        {
            base.Disable();

            creature.modifiers.RemoveEffect(braveEffect);
        }
    }
}