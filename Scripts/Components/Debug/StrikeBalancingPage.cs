using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class StrikeBalancingPage : DebugMenuPage
    {
        private const int DecimalPlaces = 3;

        [SerializeField] private Slider strikeVelocityTwoPointThresholdSlider;
        [SerializeField] private Slider strikeVelocityThreePointThresholdSlider;
        [SerializeField] private Slider thrustVelocityThresholdSlider;
        [SerializeField] private Slider flickVelocityThresholdSlider;
        [SerializeField] private Slider strikeDistanceTwoPointThresholdSlider;
        [SerializeField] private Slider strikeDistanceThreePointThresholdSlider;
        [SerializeField] private Slider thrustExpirationTimeSlider;
        [SerializeField] private Slider flickExpirationTimeSlider;
        [SerializeField] private Slider axisScaleXSlider;
        [SerializeField] private Slider axisScaleYSlider;
        [SerializeField] private Slider axisScaleZSlider;
        [SerializeField] private Slider knockbackForceSlider;

        [SerializeField] private TextMeshProUGUI strikeVelocityTwoPointThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI strikeVelocityThreePointThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI thrustVelocityThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI flickVelocityThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI strikeDistanceTwoPointThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI strikeDistanceThreePointThresholdValueDisplay;
        [SerializeField] private TextMeshProUGUI thrustExpirationTimeValueDisplay;
        [SerializeField] private TextMeshProUGUI flickExpirationTimeValueDisplay;
        [SerializeField] private TextMeshProUGUI axisScaleXValueDisplay;
        [SerializeField] private TextMeshProUGUI axisScaleYValueDisplay;
        [SerializeField] private TextMeshProUGUI axisScaleZValueDisplay;
        [SerializeField] private TextMeshProUGUI knockbackForceValueDisplay;

        private void SetInitialValues()
        {
            strikeVelocityTwoPointThresholdSlider.value = CombatSettings.Strikes.VelocityTwoPointThreshold;
            strikeVelocityThreePointThresholdSlider.value = CombatSettings.Strikes.VelocityThreePointThreshold;
            thrustVelocityThresholdSlider.value = CombatSettings.Strikes.ThrustThreshold;
            flickVelocityThresholdSlider.value = CombatSettings.Strikes.FlickThreshold;
            strikeDistanceTwoPointThresholdSlider.value = CombatSettings.Strikes.DistanceTwoPointThreshold;
            strikeDistanceThreePointThresholdSlider.value = CombatSettings.Strikes.DistanceThreePointThreshold;
            thrustExpirationTimeSlider.value = CombatSettings.Strikes.ThrustExpirationTime;
            flickExpirationTimeSlider.value = CombatSettings.Strikes.FlickExpirationTime;
            axisScaleXSlider.value = CombatSettings.Strikes.AxisScale.x;
            axisScaleYSlider.value = CombatSettings.Strikes.AxisScale.y;
            axisScaleZSlider.value = CombatSettings.Strikes.AxisScale.z;
            knockbackForceSlider.value = CombatSettings.Strikes.KnockbackForceMultiplier;
        }

        public override void ResetValues()
        {
            strikeVelocityTwoPointThresholdSlider.value = CombatSettings.Strikes.DefaultVelocityTwoPointThreshold;
            strikeVelocityThreePointThresholdSlider.value = CombatSettings.Strikes.DefaultVelocityThreePointThreshold;
            thrustVelocityThresholdSlider.value = CombatSettings.Strikes.DefaultThrustThreshold;
            flickVelocityThresholdSlider.value = CombatSettings.Strikes.DefaultFlickThreshold;
            strikeDistanceTwoPointThresholdSlider.value = CombatSettings.Strikes.DefaultDistanceTwoPointThreshold;
            strikeDistanceThreePointThresholdSlider.value = CombatSettings.Strikes.DefaultDistanceThreePointThreshold;
            thrustExpirationTimeSlider.value = CombatSettings.Strikes.DefaultThrustExpirationTime;
            flickExpirationTimeSlider.value = CombatSettings.Strikes.DefaultFlickExpirationTime;
            axisScaleXSlider.value = CombatSettings.Strikes.DefaultAxisScale.x;
            axisScaleYSlider.value = CombatSettings.Strikes.DefaultAxisScale.y;
            axisScaleZSlider.value = CombatSettings.Strikes.DefaultAxisScale.z;
            knockbackForceSlider.value = CombatSettings.Strikes.DefaultKnockbackForceMultiplier;
        }

        private void UpdateValues()
        {
            strikeVelocityTwoPointThresholdValueDisplay.text = CombatSettings.Strikes.VelocityTwoPointThreshold.ToString();
            strikeVelocityThreePointThresholdValueDisplay.text = CombatSettings.Strikes.VelocityThreePointThreshold.ToString();
            thrustVelocityThresholdValueDisplay.text = CombatSettings.Strikes.ThrustThreshold.ToString();
            flickVelocityThresholdValueDisplay.text = CombatSettings.Strikes.FlickThreshold.ToString();
            strikeDistanceTwoPointThresholdValueDisplay.text = CombatSettings.Strikes.DistanceTwoPointThreshold.ToString();
            strikeDistanceThreePointThresholdValueDisplay.text = CombatSettings.Strikes.DistanceThreePointThreshold.ToString();
            thrustExpirationTimeValueDisplay.text = CombatSettings.Strikes.ThrustExpirationTime.ToString();
            flickExpirationTimeValueDisplay.text = CombatSettings.Strikes.FlickExpirationTime.ToString();
            axisScaleXValueDisplay.text = CombatSettings.Strikes.AxisScale.x.ToString();
            axisScaleYValueDisplay.text = CombatSettings.Strikes.AxisScale.y.ToString();
            axisScaleZValueDisplay.text = CombatSettings.Strikes.AxisScale.z.ToString();
            knockbackForceValueDisplay.text = CombatSettings.Strikes.KnockbackForceMultiplier.ToString();
        }

        protected override bool TryInitialize()
        {
            SetInitialValues();

            strikeVelocityTwoPointThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.VelocityTwoPointThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            strikeVelocityThreePointThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.VelocityThreePointThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            thrustVelocityThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.ThrustThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            flickVelocityThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.FlickThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            strikeDistanceTwoPointThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.DistanceTwoPointThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            strikeDistanceThreePointThresholdSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.DistanceThreePointThreshold = value.Round(DecimalPlaces); UpdateValues(); });
            thrustExpirationTimeSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.ThrustExpirationTime = value.Round(DecimalPlaces); UpdateValues(); });
            flickExpirationTimeSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.FlickExpirationTime = value.Round(DecimalPlaces); UpdateValues(); });
            axisScaleXSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.AxisScale.x = value.Round(DecimalPlaces); UpdateValues(); });
            axisScaleYSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.AxisScale.y = value.Round(DecimalPlaces); UpdateValues(); });
            axisScaleZSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.AxisScale.z = value.Round(DecimalPlaces); UpdateValues(); });
            knockbackForceSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Strikes.KnockbackForceMultiplier = value.Round(DecimalPlaces); UpdateValues(); });

            UpdateValues();

            return true;
        }
    }
}