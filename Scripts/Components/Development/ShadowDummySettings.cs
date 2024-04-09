using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Stats;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class ShadowDummySettings : MonoBehaviour
    {
        [SerializeField] private Slider assignedStrengthSlider;
        [SerializeField] private TextMeshProUGUI assignedStrengthValueDisplay;
        [SerializeField] private TextMeshProUGUI totalStrengthValueDisplay;
        [SerializeField] private Button attackModeButton;
        [SerializeField] private Button partialRagdollModeButton;

        private ShadowDummy shadowDummy;
        private bool attackMode = false;
        private bool partialRagdollMode = false;

        private int currentAssignedStrength => shadowDummy.creature.stats.GetAssignedStatByName(Stat.Strength);

        private void Awake()
        {
            shadowDummy = GetComponentInParent<ShadowDummy>();
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(0.1f);

            assignedStrengthSlider.onValueChanged.AddListener(SetAssignedStrength);
            assignedStrengthSlider.value = currentAssignedStrength;
            UpdateTexts();

            attackModeButton.onClick.AddListener(FlipAttackMode);
            partialRagdollModeButton.onClick.AddListener(FlipPartialRagdollMode);
        }

        private void SetAssignedStrength(float newValue)
        {
            var currentValue = shadowDummy.creature.stats.GetAssignedStatByName(Stat.Strength);
            var difference = (int)newValue - currentValue;
            shadowDummy.creature.stats.AdjustStatByName(Stat.Strength, difference);
            UpdateTexts();
        }

        private void FlipAttackMode()
        {
            attackMode = !attackMode;
            shadowDummy.isInAttackMode = attackMode;
            attackModeButton.GetComponentInChildren<TextMeshProUGUI>().text = (attackMode ? "Dea" : "A") + "ctivate Attack Mode";
        }

        private void FlipPartialRagdollMode()
        {
            partialRagdollMode = !partialRagdollMode;
            shadowDummy.SetPartialRagdollMode(partialRagdollMode);
            partialRagdollModeButton.GetComponentInChildren<TextMeshProUGUI>().text = (partialRagdollMode ? "Dea" : "A") + "ctivate Partial Ragdoll Mode";
        }

        private void UpdateTexts()
        {
            assignedStrengthValueDisplay.text = currentAssignedStrength.ToString();
            totalStrengthValueDisplay.text = shadowDummy.creature.stats.subtotalStrength.ToString();
        }
    }
}