using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.PostProcessing
{
    [RequireComponent(typeof(Volume))]
    public class MovementVignetteEffect : MonoBehaviour
    {
        [SerializeField] private float playerVelocityToVignetteIntensity;
        [SerializeField] private float playerAngularVelocityToVignetteIntensity;

        private Volume volume;
        private Vignette vignette;
        private Rigidbody playerRigidbody;
        private PlayerCharacterMovementController playerCharacterMovementController;

        private void Start()
        {
            volume = gameObject.GetComponent<Volume>();

            if (!volume.profile.TryGet(out vignette))
            {
                throw new System.Exception("Vignette not present on volume");
            }

            playerRigidbody = PlayerCharacterReference.Instance.GetComponent<Rigidbody>();
            playerCharacterMovementController = PlayerCharacterReference.Instance.GetComponent<PlayerCharacterMovementController>();
        }

        private void Update()
        {
            if (vignette == null)
            {
                return;
            }

            var intensityFromVelocity = playerRigidbody.velocity.magnitude * playerVelocityToVignetteIntensity;
            var intensityFromAngularVelocity = Mathf.Abs(playerCharacterMovementController.rotationSpeed) * playerAngularVelocityToVignetteIntensity;
            var vignetteIntensity = Mathf.Max(intensityFromVelocity, intensityFromAngularVelocity);

            vignette.intensity.value = vignetteIntensity;
        }
    }
}