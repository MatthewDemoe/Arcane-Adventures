using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Combat
{
    public static class CombatSettings
    {
        public static class Controller
        {
            public const float DefaultMaximumAngularVelocity = 20f;
            public static float MaximumAngularVelocity = DefaultMaximumAngularVelocity;

            public const float DefaultMaximumVelocity = 5f;
            public static float MaximumVelocity = DefaultMaximumVelocity;

            public const float DefaultCharacterStrengthToMassRatio = 0.1f;
            public static float CharacterStrengthToMassRatio = DefaultCharacterStrengthToMassRatio;

            public static bool showGhostHandsWhenStunned = true;
        }

        public static class Strikes
        {
            public const float DefaultVelocityTwoPointThreshold = 3.0f;
            public static float VelocityTwoPointThreshold = DefaultVelocityTwoPointThreshold;

            public const float DefaultVelocityThreePointThreshold = 4.5f;
            public static float VelocityThreePointThreshold = DefaultVelocityThreePointThreshold;

            public const float DefaultDistanceTwoPointThreshold = 0.6f;
            public static float DistanceTwoPointThreshold = DefaultDistanceTwoPointThreshold;

            public const float DefaultDistanceThreePointThreshold = 0.75f;
            public static float DistanceThreePointThreshold = DefaultDistanceThreePointThreshold;

            public static Vector3 DefaultAxisScale => new Vector3(1, 0.5f, 1);
            public static Vector3 AxisScale = DefaultAxisScale;

            public const float DefaultMoveThresholdDenominator = 5.0f;
            public static float MoveThresholdDenominator = DefaultMoveThresholdDenominator;

            public const float DefaultThrustThreshold = 0.04f;
            public static float ThrustThreshold = DefaultThrustThreshold;

            public const float DefaultThrustExpirationTime = 0.15f;
            public static float ThrustExpirationTime = DefaultThrustExpirationTime;

            public const float DefaultFlickThreshold = 20f;
            public static float FlickThreshold = DefaultFlickThreshold;

            public const float DefaultFlickExpirationTime = 0.15f;
            public static float FlickExpirationTime = DefaultFlickExpirationTime;

            public const float DefaultKnockbackForceMultiplier = 10000;
            public static float KnockbackForceMultiplier = DefaultKnockbackForceMultiplier;
        }

        public static class Spells
        {
            public static float AimAngleAdjust = 45.0f;

            public const float DefaultKnockbackForceMultiplier = 3.0f;
            public static float KnockbackForceMultiplier = DefaultKnockbackForceMultiplier;

            public const float DefaultDashForceMultiplier = 190.0f;
            public static float DashForceMultiplier = DefaultDashForceMultiplier;

            public const float DefaultProjectileForce = 2000.0f;
            public static float ProjectileForceMultiplier = 1.0f;
            public static float ProjectileForce => DefaultProjectileForce * ProjectileForceMultiplier;

            public const float DefaultAimAssistRange = 30.0f;
            public static float AimAssistRangeMultiplier = 1.0f;
            public static float AimAssistRange => DefaultAimAssistRange * AimAssistRangeMultiplier;

            public const float DefaultAimAssistWidthFraction = 5.0f;
            public static float AimAssistWidthMultiplier = 1.0f;
            public static float AimAssistWidthFraction => DefaultAimAssistWidthFraction * AimAssistWidthMultiplier;

            public const float DefaultMinAimAssistAmount = 0.025f;
            public static float MinAimAssistMultiplier = 1.0f;
            public static float MinAimAssistAmount => DefaultMinAimAssistAmount * MinAimAssistMultiplier * AimAssistRange;

            public const float DefaultMaxAimAssistAmount = 0.004f;
            public static float MaxAimAssistMultiplier = 1.0f;
            public static float MaxAimAssistAmount => DefaultMaxAimAssistAmount * MaxAimAssistMultiplier * AimAssistRange;

            public static bool UseSingleTargeter = false;
            public static bool UseAutomaticTargeter = true;
            public static bool ShowTargetingCollider = false;
        }
    }
}