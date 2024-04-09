using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR
{
    [Serializable]
    [AddComponentMenu("XR/Player Character Tracked Pose Driver (New Input System)")]
    public class PlayerCharacterTrackedPoseDriver : TrackedPoseDriver
    {
        [SerializeField] private GameObject controllers;

        private bool isStartingPositionSet = false;
        private Vector3 startingPosition;
        private float startingHeight = 0.0f;

        public void ResetStartingPosition()
        {
            isStartingPositionSet = false;
        }

        protected override void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
        {
            if (!isStartingPositionSet && newPosition == Vector3.zero)
            {
                transform.localPosition = newPosition;
                transform.localRotation = newRotation;
                return;
            }

            if (!isStartingPositionSet)
            {
                startingPosition = newPosition;
                startingHeight = PlayerCharacterReference.Instance.playerAvatar.isAvatarReady ? PlayerCharacterReference.Instance.playerAvatar.GetHeadToFootDistance() : GetCharacterHeight();
                isStartingPositionSet = true;
            }

            UpdateRotation(newRotation);
            UpdatePosition(newPosition);
        }

        private void UpdatePosition(Vector3 newPosition)
        {
            var finalPosition = Vector3.zero;

            float headHeight = PlayerCharacterReference.Instance.playerAvatar.isAvatarReady ? PlayerCharacterReference.Instance.playerAvatar.GetHeadToFootDistance() + GetCharacterHeightOffset() : GetCharacterHeight();

            var input = Mathf.InverseLerp(0, startingPosition.y, newPosition.y);
            var newY = Mathf.Lerp(0, headHeight, input);
            var yChange = newY - newPosition.y;
            finalPosition.y = newY;

            transform.localPosition = finalPosition;

            //TODO: Find a way to fix XZ coordinates to neck without breaking camera offset 

            XRReferences.Instance.SetHeadsetPosition(newPosition);

            UpdateControllersPosition(yChange);
        }

        private void UpdateControllersPosition(float yChange)
        {
            var controllersPosition = controllers.transform.localPosition;
            controllersPosition.x = -XRReferences.Instance.headsetPosition.x;
            controllersPosition.y = yChange;
            controllersPosition.z = -XRReferences.Instance.headsetPosition.z;
            controllers.transform.localPosition = controllersPosition;
        }

        private void UpdateRotation(Quaternion newRotation)
        {
            XRReferences.Instance.SetHeadsetRotation(newRotation);
            transform.localRotation = newRotation;
        }

        private float GetCharacterHeight()
        {
            if (PlayerCharacterReference.Instance == null || 
                PlayerCharacterReference.Instance.playerAvatar == null || 
                !PlayerCharacterReference.Instance.playerAvatar.isAvatarReady)
            {
                return startingPosition.y;
            }

            return PlayerCharacterReference.Instance.playerAvatar.GetHeight();
        }

        private float GetCharacterHeightOffset()
        {
            if (PlayerCharacterReference.Instance == null ||
                PlayerCharacterReference.Instance.playerAvatar == null ||
                !PlayerCharacterReference.Instance.playerAvatar.isAvatarReady)
            {
                return startingPosition.y;
            }

            return PlayerCharacterReference.Instance.playerAvatar.GetHeight() - startingHeight;
        }
    }
}