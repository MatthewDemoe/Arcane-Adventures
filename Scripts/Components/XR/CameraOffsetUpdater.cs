using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.XR;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR
{
    public class CameraOffsetUpdater : MonoBehaviour
    {
        private float forwardDisplacementMultiplier;

        private Camera mainCamera;

        private void Start()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (PlayerCharacterReference.Instance == null || 
                PlayerCharacterReference.Instance.creature is PlayerGhost ||
                PlayerCharacterReference.Instance.playerAvatar == null || 
                !PlayerCharacterReference.Instance.playerAvatar.isAvatarReady)
            {
                return;
            }

            forwardDisplacementMultiplier = PlayerCharacterReference.Instance.playerAvatar.GetHeight() * XRSettings.CameraOffsetHeightMultiplier;
            var amountLookedDown = Vector3.Dot(mainCamera.transform.forward, Vector3.down);
            var forwardDisplacement = PlayerCharacterReference.Instance.playerAvatar.transform.forward * amountLookedDown * forwardDisplacementMultiplier;
            var newPosition = PlayerCharacterReference.Instance.playerAvatar.transform.position + forwardDisplacement;

            transform.position = newPosition;
        }

        public void ReportAutoRotation(float eulerDelta)
        {
            var newRotation = this.transform.rotation.eulerAngles;
            newRotation.y += eulerDelta;
            transform.rotation = Quaternion.Euler(newRotation);
        }
    }
}