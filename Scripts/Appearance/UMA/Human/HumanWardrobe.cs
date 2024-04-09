using com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA;
using System.Collections.Generic;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA
{
    public class HumanWardrobe : Wardrobe
    {
        public override Dictionary<RaceData, Dictionary<Feature, Enum>> featureListByRaceData => new Dictionary<RaceData, Dictionary<Feature, Enum>>()
        {
            {
                RaceData.HumanMaleDCS,
                new Dictionary<Feature, Enum>()
                {
                    { Feature.Gender, new Gender()},
                    { Feature.Top, new MaleTop() },
                    { Feature.Bottom, new MaleBottom() },
                }
            },
        
            {
                RaceData.HumanFemaleDCS,
                new Dictionary<Feature, Enum>()
                {
                    { Feature.Gender, new Gender()},
                    { Feature.Top, new FemaleTop() },
                    { Feature.Bottom, new FemaleBottom() },
                }
            },
        };

        public override Dictionary<Gender, RaceData> raceDataByGender => new Dictionary<Gender, RaceData>()
        {                
            {
                Gender.Male,
                RaceData.HumanMaleDCS
            },
        
            {
                Gender.Female,
                RaceData.HumanFemaleDCS
            },                          
        };

        public enum Eyebrows
        {
            HumanMaleBrow01,
            HumanMaleBrow02,
            HumanMaleBrowHR,
        }
        
        public enum Beards
        {
            HumanMaleBeard1,
            HumanMaleBeard2,
            HumanMaleBeard3,
        }
        
        public enum MaleHairstyles
        {
            HumanMaleHair1, 
            HumanMaleHair2,
            HumanMaleHair3, 
        }
        
        public enum FemaleHairstyles
        {
            HumanFemaleHair1,
            HumanFemaleHair2,
            HumanFemaleHair3,
        }

        public enum MaleTop
        {
            HumanMaleShirt1,
            HumanMaleShirt2,
            HumanMaleShirt3,
        }

        public enum FemaleTop
        {
            HumanFemaleShirt1,
            HumanFemaleShirt2,
            HumanFemaleShirt3,
        }

        public enum MaleBottom
        {
            HumanMaleBottoms1,
            HumanMaleBottoms2,
        }

        public enum FemaleBottom
        {
            HumanFemalePants1,
            HumanFemalePants2,
        }
    }
}
