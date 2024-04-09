using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class CharacterAdjustmentsPage : DebugMenuPage
    {

        [SerializeField] private TextMeshProUGUI values;
        [SerializeField] private Slider strengthSlider;
        [SerializeField] private Slider vitalitySlider;
        [SerializeField] private Slider spiritSlider;

        private Creature playerCharacterCreature => PlayerCharacterReference.Instance.creature;

        private void SetInitialValues()
        {
            strengthSlider.value = playerCharacterCreature.stats.subtotalStrength;
            vitalitySlider.value = playerCharacterCreature.stats.subtotalVitality;
            spiritSlider.value = playerCharacterCreature.stats.subtotalSpirit;
        }

        private void UpdateValues()
        {
            values.text =
                "\n" +
                "\n" +
                $"{playerCharacterCreature.stats.subtotalStrength}\n" +
                $"\n" +
                $"{playerCharacterCreature.stats.subtotalVitality}\n" +
                "\n" +
                $"{playerCharacterCreature.stats.subtotalSpirit}";
        }

        protected override bool TryInitialize()
        {
            if (PlayerCharacterReference.Instance is null) { return false; }

            strengthSlider.onValueChanged.RemoveListener(AdjustStrength);
            vitalitySlider.onValueChanged.RemoveListener(AdjustVitality);
            spiritSlider.onValueChanged.RemoveListener(AdjustSpirit);
            InitializeValuesAndSliders();

            return true;
        }

        private void InitializeValuesAndSliders()
        {
            SetInitialValues();

            strengthSlider.onValueChanged.AddListener(AdjustStrength);
            vitalitySlider.onValueChanged.AddListener(AdjustVitality);
            spiritSlider.onValueChanged.AddListener(AdjustSpirit);

            UpdateValues();
        }

        private void AdjustStrength(float value)
        {
            var newStrength = (int)value;
            var amount = newStrength - playerCharacterCreature.stats.subtotalStrength;
            playerCharacterCreature.stats.AdjustStatByName(Stats.Stat.Strength, amount);
            UpdateValues();
        }

        private void AdjustVitality(float value)
        {
            var newVitality = (int)value;
            var amount = newVitality - playerCharacterCreature.stats.subtotalVitality;
            playerCharacterCreature.stats.AdjustStatByName(Stats.Stat.Vitality, amount);
            UpdateValues();
        }

        private void AdjustSpirit(float value)
        {
            var newSpirit = (int)value;
            var amount = newSpirit - playerCharacterCreature.stats.subtotalSpirit;
            playerCharacterCreature.stats.AdjustStatByName(Stats.Stat.Spirit, amount);
            UpdateValues();
        }
    }
}