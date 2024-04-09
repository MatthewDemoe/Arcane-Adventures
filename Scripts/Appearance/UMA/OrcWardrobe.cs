using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA
{
    public class OrcWardrobe : Wardrobe
    {
        public override Dictionary<RaceData, Dictionary<Feature, Enum>> featureListByRaceData => new Dictionary<RaceData, Dictionary<Feature, Enum>>()
        {
            {
                RaceData.OrcMaleDCS,
                new Dictionary<Feature, Enum>()
                {
                    { Feature.Gender, new Gender()},
                    { Feature.Top, new MaleTop() },
                    { Feature.Bottom, new MaleBottom() },
                }
            },

            {
                RaceData.OrcFemaleDCS,
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
                RaceData.OrcMaleDCS
            },

            {
                Gender.Female,
                RaceData.OrcFemaleDCS
            },
        };

        public enum Eyebrows
        {
            OrcMaleBrow01,
            OrcMaleBrow02,
            OrcMaleBrowHR,
        }

        public enum Beards
        {
            OrcMaleBeard1,
            OrcMaleBeard2,
            OrcMaleBeard3,
        }

        public enum MaleHairstyles
        {
            OrcMaleHair1,
            OrcMaleHair2,
            OrcMaleHair3,
        }

        public enum FemaleHairstyles
        {
            OrcFemaleHair1,
            OrcFemaleHair2,
            OrcFemaleHair3,
        }

        public enum MaleTop
        {
            OrcMaleShirt1,
            OrcMaleShirt2,
            OrcMaleShirt3,
        }

        public enum FemaleTop
        {
            OrcFemaleShirt1,
            OrcFemaleShirt2,
            OrcFemaleShirt3,
        }

        public enum MaleBottom
        {
            OrcMaleBottoms1,
            OrcMaleBottoms2,
        }

        public enum FemaleBottom
        {
            OrcFemalePants1,
            OrcFemalePants2,
        }
    }
}