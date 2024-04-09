using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses
{
    public class EssenceMage : CharacterClassArchetype
    {
        public override string name => "Essence Mage";
        public override string classDescription => "Channel the natural Spirit of the world. Harness its chaotic nature and manipulate its intense power to dominate your opponents.";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.EssenceMage;

        public static EssenceMage Instance { get; } = new EssenceMage();
        protected EssenceMage() { }

        public override List<Trait> traits => new List<Trait>() 
        {
            //TODO: Enable when completed
            //DestructiveTerrain.Instance,
            //PhysicalArcaneManipulation.Instance,
            ReserveSpirit.Instance,
            SiphonedLife.Instance,
            UnstableEssence.Instance,
        };

        public override List<Spell> spells => new List<Spell>() 
        {
            ShadowBind.Instance,
            CursedBlood.Instance,
            ToxicMiasma.Instance,
            HarnessedChaos.Instance,
            InvokeFear.Instance, 
        };
    }
}
