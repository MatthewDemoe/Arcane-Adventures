using com.AlteredRealityLabs.ArcaneAdventures.Movement;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class PlayerCharacterRotator
    {
        private readonly Rigidbody rigidbody;
        private readonly Camera camera;
        private readonly Transform cameraOffset;
        private float manualRotationSpeed = 0;
        private float manualRotationAcceleration = 0.1f;//TODO: Get from settings.
        private float maximumManualRotationSpeed = 2;//TODO: Get from settings.
        
        public PlayerCharacterRotator(Rigidbody rigidbody, Camera camera)
        {
            this.rigidbody = rigidbody;
            this.camera = camera;
            cameraOffset = camera.transform.parent;
        }
        
        public void AutoRotateToCamera()
        {
            var mainCameraY = camera.transform.rotation.eulerAngles.y;
            var targetRotation = Quaternion.Euler(0, mainCameraY, 0);
            var amountLookedDown = Vector3.Dot(camera.transform.forward, Vector3.down);
            var rotationSpeed = amountLookedDown > MovementSettings.FacedownStart ?
                GetRotationSpeedBasedOnMovementAndAngleDifference(mainCameraY) :
                MovementSettings.FaceupAutoRotateSpeed;

            if (rotationSpeed == 0)
                return;

            var newRotation = Quaternion.Lerp(rigidbody.rotation, targetRotation, rotationSpeed);
            rigidbody.MoveRotation(newRotation);
        }
        
        private float GetRotationSpeedBasedOnMovementAndAngleDifference(float mainCameraY)
        {
            var yAngleDifference = Mathf.Abs(Mathf.DeltaAngle(rigidbody.rotation.eulerAngles.y, mainCameraY));
            var playerVelocity = rigidbody.velocity;
            playerVelocity.y = 0;

            var rotationSpeed = playerVelocity.magnitude > 0.1f ? 
                playerVelocity.magnitude * MovementSettings.FacedownMovementRotationMultiplier * Time.deltaTime : 
                0f;

            if (yAngleDifference < MovementSettings.AngleDifferenceForFacedownAutoRotate)
                return rotationSpeed;

            var angleModifier = (yAngleDifference - MovementSettings.AngleDifferenceForFacedownAutoRotate) * 2;
            var overShoulderRotationSpeed = angleModifier * MovementSettings.FacedownOverShoulderRotationMultiplier * Time.deltaTime;
            return Mathf.Max(overShoulderRotationSpeed, rotationSpeed);
        }
        
        public void Rotate(float input)
        {
            UpdateManualRotationSpeed(input);
            AddManualRotationToRigidbody();
            AddRotationToCameraOffset(manualRotationSpeed);
        }

        private void UpdateManualRotationSpeed(float input)
        {
            var targetRotationSpeed = maximumManualRotationSpeed * input;
            
            manualRotationSpeed = manualRotationSpeed < targetRotationSpeed ?
                Mathf.Min(manualRotationSpeed + manualRotationAcceleration, targetRotationSpeed) :
                Mathf.Max(manualRotationSpeed - manualRotationAcceleration, targetRotationSpeed);
        }

        private void AddManualRotationToRigidbody()//TODO: Move to CreatureControlInterpreter.
        {
            var newRotation = rigidbody.rotation.eulerAngles;
            newRotation.y += manualRotationSpeed;
            rigidbody.MoveRotation(Quaternion.Euler(newRotation));
        }
        
        private void AddRotationToCameraOffset(float rotation)
        {
            var newRotation = cameraOffset.rotation.eulerAngles;
            newRotation.y += rotation;
            cameraOffset.rotation = Quaternion.Euler(newRotation);
        }

    }
}