using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public static class AllStatusConditions
    {
        public enum StatusConditionName 
        { 
            BadlyBleeding, BadlyPoisoned, Bleeding, Blessed, Blinded, BodilyRestaint, Chilled, Crippled, Dazed, Demotivated, Disarmed, Diseased, Feared, Frenzied, Hasted, Immobilized, Intangible, Invisible, 
            KnockedBack, Maddened, Motivated, Muted, Poisoned, Prone, Restrained, Slowed, SpiritualRestraint, Staggered, Stunned, Strengthened, Taunted, Toughened, Vulnerable, Weakened 
        }

        public static StatusCondition ConvertEnumToStatusCondition(StatusConditionName statusCondition, ref Creature target, StatusConditionSettings statusConditionSettings, string source, bool startDuration = true)
        {            
            return StatusCondition.CreateStatusCondition(GetStatusConditionType(statusCondition), target, statusConditionSettings, source, startDuration);
        }

        private static StatusCondition GetStatusConditionType(StatusConditionName statusCondition)
        {
            switch (statusCondition)
            {
                case (StatusConditionName.Bleeding):
                    return new Bleeding();

                case (StatusConditionName.Dazed):
                    return new Dazed();

                case (StatusConditionName.Demotivated):
                    return new Demotivated();

                case (StatusConditionName.Disarmed):
                    return new Disarmed();

                case (StatusConditionName.Feared):
                    return new Feared();

                case (StatusConditionName.Frenzied):
                    return new Frenzied();

                case (StatusConditionName.Hasted):
                    return new Hasted();

                case (StatusConditionName.Immobilized):
                    return new Immobilized();

                case (StatusConditionName.KnockedBack):
                    return new KnockedBack();

                case (StatusConditionName.Maddened):
                    return new Maddened();

                case (StatusConditionName.Motivated):
                    return new Motivated();

                case (StatusConditionName.Muted):
                    return new Muted();

                case (StatusConditionName.Poisoned):
                    return new Poisoned();

                case (StatusConditionName.Restrained):
                    return new Restrained();

                case (StatusConditionName.Slowed):
                    return new Slowed();

                case (StatusConditionName.Staggered):
                    return new Staggered();

                case (StatusConditionName.Stunned):
                    return new Stunned();

                case (StatusConditionName.Taunted):
                    return new Taunted();

                case (StatusConditionName.Vulnerable):
                    return new Vulnerable();

                default:
                    throw new NotImplementedException($"Unhandled StatusConditionName {statusCondition} passed to {nameof(GetStatusConditionType)}");
            }
        }
    }
}