using System.Collections.Generic;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA
{
    public abstract class Wardrobe 
    {
        static HumanWardrobe HumanWardrobe = new HumanWardrobe();
        static OrcWardrobe OrcWardrobe = new OrcWardrobe();

        //TODO: remove gender form features, and just use the character gender
        public enum Feature
        {
            Gender,
            Top,
            Bottom,
        }

        public enum RaceData
        {
            HumanMaleDCS,
            HumanFemaleDCS,

            OrcMaleDCS,
            OrcFemaleDCS,
        }

        public enum Gender
        {
            Male,
            Female,
        }

        public static Dictionary<Feature, UMAComponents.Slots> FeatureSlotsByFeature = new Dictionary<Feature, UMAComponents.Slots>()
        {
            { Feature.Top, UMAComponents.Slots.Chest },
            { Feature.Bottom, UMAComponents.Slots.Legs },
        };

        public static Dictionary<Identifiers.Gender, Gender> GendersByIdentifier => new Dictionary<Identifiers.Gender, Gender>()
        {
            {
                Identifiers.Gender.Male,
                Gender.Male
            },

            {
                Identifiers.Gender.Female,
                Gender.Female
            },
        };

        public static Dictionary<Gender, Identifiers.Gender> IdentifierByGender => new Dictionary<Gender, Identifiers.Gender>()
        {
            {
                Gender.Male,
                Identifiers.Gender.Male                
            },

            {
                Gender.Female,
                Identifiers.Gender.Female               
            },
        };

        public static Dictionary<Identifiers.Race, Wardrobe> WardrobesByRace = new Dictionary<Identifiers.Race, Wardrobe>()
        {
            {
                Identifiers.Race.Human,
                HumanWardrobe
            },
        
            {
                Identifiers.Race.Elf,
                OrcWardrobe
            },
        };

        public abstract Dictionary<RaceData, Dictionary<Feature, Enum>> featureListByRaceData { get; }

        public abstract Dictionary<Gender, RaceData> raceDataByGender { get; }
    }
}