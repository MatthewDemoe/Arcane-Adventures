using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class AvatarAdjustmentsPage : DebugMenuPage
    {
        private const int DecimalPlaces = 3;

        [SerializeField] private Slider heightSlider;
        [SerializeField] private TextMeshProUGUI heightValueDisplay;
        [SerializeField] Button applyHeightButton;

        [SerializeField] private Slider handSizeSlider;
        [SerializeField] private TextMeshProUGUI handSizeValueDisplay;

        [SerializeField] Button outOfBodyButton;

        private float initialHeight;

        private void SetInitialValues()
        {
            initialHeight = PlayerCharacterReference.Instance.playerAvatar.GetNormalizedHeight();
            ResetValues();
        }

        public override void ResetValues()
        {
            heightSlider.value = initialHeight;
            handSizeSlider.value = 0.5f;
        }

        private void UpdateValues()
        {
            heightValueDisplay.text = heightSlider.value.Round(DecimalPlaces).ToString();
            handSizeValueDisplay.text = handSizeSlider.value.Round(DecimalPlaces).ToString();
        }

        protected override bool TryInitialize()
        {
            if (PlayerCharacterReference.Instance is null ||
                PlayerCharacterReference.Instance.playerAvatar == null)
            {
                return false;
            }

            SetInitialValues();

            heightSlider.onValueChanged.AddListener(delegate (float value)
                {
                    UpdateValues();
                }
            );

            handSizeSlider.onValueChanged.AddListener(delegate (float value)
                {
                    //PlayerCharacterReference.Instance.playerAvatar.SetHandSize(handSizeSlider.value);
                    UpdateValues();
                }
            );

            applyHeightButton.onClick.AddListener(delegate ()
                {
                    //PlayerCharacterReference.Instance.playerAvatar.SetNormalizedHeight(heightSlider.value);
                }
            );

            outOfBodyButton.onClick.AddListener(SwitchToOrFromOutOfBody);

            UpdateValues();

            return true;
        }

        private void SwitchToOrFromOutOfBody()
        {
            var offsetUpdater = XRReferences.Instance.GetComponentInChildren<CameraOffsetUpdater>();
            var switchToOutOfBody = offsetUpdater.enabled;

            if (!switchToOutOfBody)
            {
                Camera.main.transform.parent = offsetUpdater.transform;
            }

            offsetUpdater.enabled = !switchToOutOfBody;

            var playerCharacterTrackedPoseDriver = Camera.main.GetComponent<PlayerCharacterTrackedPoseDriver>();
            playerCharacterTrackedPoseDriver.enabled = !switchToOutOfBody;
            PlayerCharacterReference.Instance.playerAvatar.autoRotateToCamera = !switchToOutOfBody;
            PlayerCharacterReference.Instance.playerAvatar.showHead = switchToOutOfBody;

            if (switchToOutOfBody)
            {
                Camera.main.transform.parent = null;
            }
        }
    }
}