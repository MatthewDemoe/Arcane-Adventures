using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells;

using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses.WizardClasses
{
    public class Elementalist : CharacterClassArchetype
    {
        public override string name => "Elementalist";
        public override string classDescription => "Conjure and bend the very elements of this world to wreak havoc among your enemies by combining elements in advantageous ways.";
        public override Identifiers.CharacterClassArchetype identifier => Identifiers.CharacterClassArchetype.Elementalist; 

        public static Elementalist Instance { get; } = new Elementalist();
        protected Elementalist() { }

        public override List<Trait> traits => new List<Trait>() 
        {
            Alchemist.Instance,
            ElementalAttunement.Instance,
            ElementalRedirection.Instance,
            MasterfulElements.Instance,
            OverflowingElements.Instance,
        };

        public override List<Spell> spells => new List<Spell>()
        {
            BoulderToss.Instance,
            FavoredWind.Instance,
            Ignite.Instance,
            WaterGeyser.Instance,
        };
    }
}