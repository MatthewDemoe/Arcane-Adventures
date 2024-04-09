using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races
{
    [System.Serializable]
    public class Race
    {
        private static Dictionary<Identifiers.Race, Race> RacesByIdentifier;
        public static Race Human => Get(Identifiers.Race.Human);
        public static Race Elf => Get(Identifiers.Race.Elf);

        public Trait trait { get; protected set; }

        public string name { get; }
        public Identifiers.Race identifier { get; }
        public int strengthModifier { get; }
        public int vitalityModifier { get; }
        public int spiritModifier { get; }
        public Identifiers.Trait traitIdentifier { get; }

        public string lore { get; }
        public float maleMinimumHeight { get; }
        public float maleMaximumHeight { get; }
        public float femaleMinimumHeight { get; }
        public float femaleMaximumHeight { get; }

        public Race(string name, Identifiers.Race identifier, int strengthModifier, int vitalityModifier, int spiritModifier, Identifiers.Trait trait, string lore, float maleMinimumHeight, float maleMaximumHeight, float femaleMinimumHeight, float femaleMaximumHeight)
        {
            this.name = name;
            this.identifier = identifier;
            this.strengthModifier = strengthModifier;
            this.vitalityModifier = vitalityModifier;
            this.spiritModifier = spiritModifier;
            traitIdentifier = trait;
            this.lore = lore;
            this.maleMinimumHeight = maleMinimumHeight;
            this.maleMaximumHeight = maleMaximumHeight;
            this.femaleMinimumHeight = femaleMinimumHeight;
            this.femaleMaximumHeight = femaleMaximumHeight;
        }

        public void InitializeStats(Stats stats) 
        {
            stats.SetRaceStats(this);
            stats.traitStatPoints = 0;
        }

        public void InitializeTrait(Creature creature)
        {
            trait = Trait.GetTrait(AllTraits.GetTraitType(traitIdentifier), creature);
        }

        public static Race Get(Identifiers.Race identifier)
        {
            if (RacesByIdentifier == null)
            {
                RacesByIdentifier = RaceAsset.LoadRaces().ToDictionary(race => race.identifier, race => race);
            }

            return RacesByIdentifier.TryGetValue(identifier, out var race) ? race : null;
        }
    }
}