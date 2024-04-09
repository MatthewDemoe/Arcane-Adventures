using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses
{
    public class ArcaneAdept : CharacterClassArchetype
    {
        public override string name => "Arcane Adept";
        public override string classDescription => "Focus on the science behind the Arcane; enhance and alter spells to your whim and destroy your foes with spells no other caster can reproduce.";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.ArcaneAdept;

        public static ArcaneAdept Instance { get; } = new ArcaneAdept();
        protected ArcaneAdept() { }

        public override List<Trait> traits => new List<Trait>() 
        {
            AlterationMastery.Instance,
            ArcaneOvercharging.Instance,
            ComplexCaster.Instance,
            MagicalRecovery.Instance,
            SpiritComposer.Instance,
        };

        public override List<Spell> spells => new List<Spell>()
        {
            ArcaneBlast.Instance,
            EnhancedRange.Instance,
            ArcaneGrasp.Instance,
            ElementalCasting.Instance,
            Shockwave.Instance,
        };
    }
}
