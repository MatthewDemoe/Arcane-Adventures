namespace com.AlteredRealityLabs.ArcaneAdventures.Movement
{
    public static class MovementSettings
    {
        public const float DefaultFacedownMovementRotationMultiplier = 1f;
        public static float FacedownMovementRotationMultiplier = DefaultFacedownMovementRotationMultiplier;

        public const float DefaultFacedownOverShoulderRotationMultiplier = 0.01f;
        public static float FacedownOverShoulderRotationMultiplier = DefaultFacedownOverShoulderRotationMultiplier;

        public const float DefaultFaceupAutoRotateSpeed = 0.05f;
        public static float FaceupAutoRotateSpeed = DefaultFaceupAutoRotateSpeed;

        public const float DefaultAngleDifferenceForFacedownAutoRotate = 75f;
        public static float AngleDifferenceForFacedownAutoRotate = DefaultAngleDifferenceForFacedownAutoRotate;

        public const float DefaultFacedownStart = 0.75f;
        public static float FacedownStart = DefaultFacedownStart;
    }
}