using com.AlteredRealityLabs.ArcaneAdventures.Movement;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.XR;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class MovementAdjustmentsPage : DebugMenuPage
    {
        private const int DecimalPlaces = 3;

        [SerializeField] private Slider facedownMovementRotationMultiplierSlider;
        [SerializeField] private Slider facedownOverShoulderRotationMultiplierSlider;
        [SerializeField] private Slider angleDifferenceForFacedownAutoRotateSlider;
        [SerializeField] private Slider facedownStartSlider;
        [SerializeField] private Slider faceupAutoRotateSpeedSlider;
        [SerializeField] private Slider cameraForwardBackwardOffsetSlider;

        [SerializeField] private TextMeshProUGUI movementRotationMultiplierValueDisplay;
        [SerializeField] private TextMeshProUGUI overShoulderRotationMultiplierValueDisplay;
        [SerializeField] private TextMeshProUGUI angleDifferenceForFacedownAutoRotateValueDisplay;
        [SerializeField] private TextMeshProUGUI facedownStartValueDisplay;
        [SerializeField] private TextMeshProUGUI faceupAutoRotateSpeedValueDisplay;
        [SerializeField] private TextMeshProUGUI cameraForwardBackwardOffsetValueDisplay;

        private void SetInitialValues()
        {
            facedownMovementRotationMultiplierSlider.value = MovementSettings.FacedownMovementRotationMultiplier;
            facedownOverShoulderRotationMultiplierSlider.value = MovementSettings.FacedownOverShoulderRotationMultiplier;
            angleDifferenceForFacedownAutoRotateSlider.value = MovementSettings.AngleDifferenceForFacedownAutoRotate;
            facedownStartSlider.value = MovementSettings.FacedownStart;
            faceupAutoRotateSpeedSlider.value = MovementSettings.FaceupAutoRotateSpeed;
            cameraForwardBackwardOffsetSlider.value = XRSettings.CameraOffsetHeightMultiplier;
        }

        public override void ResetValues()
        {
            facedownMovementRotationMultiplierSlider.value = MovementSettings.DefaultFacedownMovementRotationMultiplier;
            facedownOverShoulderRotationMultiplierSlider.value = MovementSettings.DefaultFacedownOverShoulderRotationMultiplier;
            angleDifferenceForFacedownAutoRotateSlider.value = MovementSettings.DefaultAngleDifferenceForFacedownAutoRotate;
            facedownStartSlider.value = MovementSettings.DefaultFacedownStart;
            faceupAutoRotateSpeedSlider.value = MovementSettings.DefaultFaceupAutoRotateSpeed;
            cameraForwardBackwardOffsetSlider.value = XRSettings.DefaultCameraOffsetHeightMultiplier;
        }

        private void UpdateValues()
        {
            movementRotationMultiplierValueDisplay.text = MovementSettings.FacedownMovementRotationMultiplier.ToString();
            overShoulderRotationMultiplierValueDisplay.text = MovementSettings.FacedownOverShoulderRotationMultiplier.ToString();
            angleDifferenceForFacedownAutoRotateValueDisplay.text = MovementSettings.AngleDifferenceForFacedownAutoRotate.ToString();
            facedownStartValueDisplay.text = MovementSettings.FacedownStart.ToString();
            faceupAutoRotateSpeedValueDisplay.text = MovementSettings.FaceupAutoRotateSpeed.ToString();
            cameraForwardBackwardOffsetValueDisplay.text = XRSettings.CameraOffsetHeightMultiplier.ToString();
        }

        protected override bool TryInitialize()
        {
            SetInitialValues();

            facedownMovementRotationMultiplierSlider.onValueChanged.AddListener(delegate (float value) { MovementSettings.FacedownMovementRotationMultiplier = value.Round(DecimalPlaces); UpdateValues(); });
            facedownOverShoulderRotationMultiplierSlider.onValueChanged.AddListener(delegate (float value) { MovementSettings.FacedownOverShoulderRotationMultiplier = value.Round(DecimalPlaces); UpdateValues(); });
            angleDifferenceForFacedownAutoRotateSlider.onValueChanged.AddListener(delegate (float value) { MovementSettings.AngleDifferenceForFacedownAutoRotate = value.Round(DecimalPlaces); UpdateValues(); });
            facedownStartSlider.onValueChanged.AddListener(delegate (float value) { MovementSettings.FacedownStart = value.Round(DecimalPlaces); UpdateValues(); });
            faceupAutoRotateSpeedSlider.onValueChanged.AddListener(delegate (float value) { MovementSettings.FaceupAutoRotateSpeed = value.Round(DecimalPlaces); UpdateValues(); });
            cameraForwardBackwardOffsetSlider.onValueChanged.AddListener(delegate (float value) { XRSettings.CameraOffsetHeightMultiplier = value.Round(DecimalPlaces); UpdateValues(); });

            UpdateValues();

            return true;
        }
    }
}