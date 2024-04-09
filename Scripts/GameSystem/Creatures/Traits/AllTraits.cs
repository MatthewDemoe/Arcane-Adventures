using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public static class AllTraits
    {
        public static Trait GetTraitType(Identifiers.Trait traitType)
        {
            switch (traitType)
            {
                //Race
                case Identifiers.Trait.AgileBody:
                    return AgileBody.Instance;

                case Identifiers.Trait.VariedPassions:
                    return VariedPassions.Instance;

                //Berserker
                case Identifiers.Trait.TunnelVision:
                    return TunnelVision.Instance;

                case Identifiers.Trait.FrothingRage:
                    return FrothingRage.Instance;

                case Identifiers.Trait.PleasureInDeath:
                    return PleasureInDeath.Instance;

                //Essence Mage
                case Identifiers.Trait.ReserveSpirit:
                    return ReserveSpirit.Instance;

                case Identifiers.Trait.UnstableEssence:
                    return UnstableEssence.Instance;

                case Identifiers.Trait.SiphonedLife:
                    return SiphonedLife.Instance;

                default:
                    throw new NotImplementedException($"Unhandled Trait {traitType} passed to {nameof(GetTraitType)}");
            }
        }
    }
}
