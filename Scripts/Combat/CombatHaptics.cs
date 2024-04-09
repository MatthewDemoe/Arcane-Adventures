using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Combat
{
    public static class CombatHaptics
    {
        private static Dictionary<StrikeType, float> hapticAmplitudeByStrikeType = new Dictionary<StrikeType, float>
        {
            { StrikeType.Perfect, 1f },
            { StrikeType.Imperfect, 0.5f },
            { StrikeType.Incomplete, 0.25f },
            { StrikeType.NotStrike, 0.1f }
        };

        private static Dictionary<StrikeType, float> hapticDurationByStrikeType = new Dictionary<StrikeType, float>
        {
            { StrikeType.Perfect, 1f },
            { StrikeType.Imperfect, 0.5f },
            { StrikeType.Incomplete, 0.25f },
            { StrikeType.NotStrike, 0.1f }
        };

        public static void SendHapticImpulse(StrikeType strikeType, HandSide handSide)
        {
            var amplitude = hapticAmplitudeByStrikeType[strikeType];
            var duration = hapticDurationByStrikeType[strikeType];

            Controllers.SendHapticImpulse(handSide, amplitude, duration);
        }
    }
}