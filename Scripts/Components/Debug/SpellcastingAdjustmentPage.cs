using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class SpellcastingAdjustmentPage : DebugMenuPage
    {
        private const int DecimalPlaces = 3;

        [SerializeField] private Slider angleSlider;
        [SerializeField] private TextMeshProUGUI angleValueDisplay;

        [SerializeField] private Slider projectileForceSlider;
        [SerializeField] private TextMeshProUGUI projectileForceDisplay;

        [SerializeField] private Slider aimAssistRangeSlider;
        [SerializeField] private TextMeshProUGUI aimAssistRangeDisplay;

        [SerializeField] private Slider aimAssistWidthSlider;
        [SerializeField] private TextMeshProUGUI aimAssistWidthDisplay;

        [SerializeField] private Slider minAssistAmountSlider;
        [SerializeField] private TextMeshProUGUI minAssistAmountDisplay;

        [SerializeField] private Slider maxAssistAmountSlider;
        [SerializeField] private TextMeshProUGUI maxAssistAmountDisplay;

        [SerializeField] private Toggle targetedAimAssistButton;
        [SerializeField] private Toggle automaticAimAssistButton;
        [SerializeField] private Toggle showColliderButton;

        PlayerSpellCaster playerSpellCaster;

        protected override bool TryInitialize()
        {
            if (PlayerCharacterReference.Instance == null)
            {
                return false;
            }

            playerSpellCaster = PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>();

            SetInitialValues();

            angleSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            projectileForceSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            aimAssistRangeSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            aimAssistWidthSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            minAssistAmountSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            maxAssistAmountSlider.onValueChanged.AddListener(delegate (float value)
            {
                UpdateValues();
            }
            );

            automaticAimAssistButton.onValueChanged.AddListener(delegate (bool value)
            {
                UpdateValues();
            }
            );

            targetedAimAssistButton.onValueChanged.AddListener(delegate (bool value)
            {
                UpdateValues();
            }
            );

            showColliderButton.onValueChanged.AddListener(delegate (bool value)
            {
                UpdateValues();
            }
            );

            UpdateValues();

            return true;
        }

        private void SetInitialValues()
        {
            angleSlider.value = CombatSettings.Spells.AimAngleAdjust;

            projectileForceSlider.value = CombatSettings.Spells.ProjectileForceMultiplier;
            aimAssistRangeSlider.value = CombatSettings.Spells.AimAssistRangeMultiplier;
            aimAssistWidthSlider.value = CombatSettings.Spells.AimAssistWidthMultiplier;
            minAssistAmountSlider.value = CombatSettings.Spells.MinAimAssistMultiplier;
            maxAssistAmountSlider.value = CombatSettings.Spells.MaxAimAssistMultiplier;

            targetedAimAssistButton.isOn = CombatSettings.Spells.UseSingleTargeter;
            automaticAimAssistButton.isOn = CombatSettings.Spells.UseAutomaticTargeter;
            showColliderButton.isOn = CombatSettings.Spells.ShowTargetingCollider;
        }

        private void UpdateValues()
        {
            angleValueDisplay.text = angleSlider.value.Round(DecimalPlaces).ToString();
            CombatSettings.Spells.AimAngleAdjust = angleSlider.value;

            CombatSettings.Spells.ProjectileForceMultiplier = projectileForceSlider.value;
            projectileForceDisplay.text = projectileForceSlider.value.Round(DecimalPlaces).ToString();

            CombatSettings.Spells.AimAssistRangeMultiplier = aimAssistRangeSlider.value;
            aimAssistRangeDisplay.text = aimAssistRangeSlider.value.Round(DecimalPlaces).ToString();

            CombatSettings.Spells.AimAssistWidthMultiplier = aimAssistWidthSlider.value;
            aimAssistWidthDisplay.text = aimAssistWidthSlider.value.Round(DecimalPlaces).ToString();

            CombatSettings.Spells.MinAimAssistMultiplier = minAssistAmountSlider.value;
            minAssistAmountDisplay.text = minAssistAmountSlider.value.Round(DecimalPlaces).ToString();

            CombatSettings.Spells.MaxAimAssistMultiplier = maxAssistAmountSlider.value;
            maxAssistAmountDisplay.text = maxAssistAmountSlider.value.Round(DecimalPlaces).ToString();

            CombatSettings.Spells.UseSingleTargeter = targetedAimAssistButton.isOn;
            CombatSettings.Spells.UseAutomaticTargeter = automaticAimAssistButton.isOn;
            CombatSettings.Spells.ShowTargetingCollider = showColliderButton.isOn;
        }
    }
}