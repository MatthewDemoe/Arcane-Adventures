using System.Globalization;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class CombatDemoSettings : MonoBehaviour
    {
        private const int DecimalPlaces = 2;

        private const float DefaultKnockbackForce = 1;
        
        [SerializeField] private TextMeshProUGUI knockbackForceValueDisplay;
        [SerializeField] private Slider knockbackForceSlider;
        [SerializeField] private TMP_Dropdown ragdollOnBodyStrikeDropdown;
        [SerializeField] private Toggle dropWeaponOnRagdollToggle;
        [SerializeField] private Toggle dropHatOnRagdollToggle;

        public float knockbackForce { get; private set; }
        public RagdollOnBodyStrikeBehavior ragdollOnBodyStrikeBehavior { get; private set; }
        public bool dropWeaponOnRagdoll { get; private set; }
        public bool dropHatOnRagdoll { get; private set; }
        
        private void Start()
        {
            var options = EnumExtensions.GetValues<RagdollOnBodyStrikeBehavior>()
                .Select(option => option.ToString().AddSpacesToPascalCase())
                .ToList();
            ragdollOnBodyStrikeDropdown.AddOptions(options);
            
            Reset();
            
            knockbackForceSlider.onValueChanged.AddListener(OnKnockbackForceSliderValueChanged);
            ragdollOnBodyStrikeDropdown.onValueChanged.AddListener(OnRagdollOnBodyStrikeDropdownValueChanged);
            dropWeaponOnRagdollToggle.onValueChanged.AddListener(OnDropWeaponOnRagdollToggleValueChanged);
            dropHatOnRagdollToggle.onValueChanged.AddListener(OnDropHatOnRagdollToggleValueChanged);
        }

        private void Reset()
        {
            knockbackForceSlider.value = DefaultKnockbackForce;
            OnKnockbackForceSliderValueChanged(DefaultKnockbackForce);

            ragdollOnBodyStrikeDropdown.value = (int)RagdollOnBodyStrikeBehavior.TotalRagdoll;
            OnRagdollOnBodyStrikeDropdownValueChanged(ragdollOnBodyStrikeDropdown.value);

            dropWeaponOnRagdollToggle.isOn = true;
            OnDropWeaponOnRagdollToggleValueChanged(dropWeaponOnRagdollToggle.isOn);

            dropHatOnRagdollToggle.isOn = true;
            OnDropHatOnRagdollToggleValueChanged(dropHatOnRagdollToggle.isOn);
        }

        private void OnKnockbackForceSliderValueChanged(float value)
        {
            knockbackForceValueDisplay.text = value.Round(DecimalPlaces).ToString(CultureInfo.InvariantCulture);
            knockbackForce = value;
        }

        private void OnRagdollOnBodyStrikeDropdownValueChanged(int optionsIndex)
        {
            ragdollOnBodyStrikeBehavior = (RagdollOnBodyStrikeBehavior) optionsIndex;
        }

        private void OnDropWeaponOnRagdollToggleValueChanged(bool isOn)
        {
            dropWeaponOnRagdoll = isOn;
        }
        
        private void OnDropHatOnRagdollToggleValueChanged(bool isOn)
        {
            dropHatOnRagdoll = isOn;
        }

        public enum RagdollOnBodyStrikeBehavior
        {
            TotalRagdoll, 
            LimitedRagdoll, 
            NoRagdoll
        }
    }
}