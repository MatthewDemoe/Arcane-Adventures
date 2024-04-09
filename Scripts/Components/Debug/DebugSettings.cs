using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class DebugSettings : DebugMenuPage
    {
        [SerializeField] private Toggle showDebugBarToggle;
        [SerializeField] private Toggle hideResourceBarsToggle;
        [SerializeField] private Toggle neverShowFpsWarningToggle;
        [SerializeField] private Toggle eagleModeToggle;

        public override void ResetValues()
        {
            showDebugBarToggle.isOn = false;
            hideResourceBarsToggle.isOn = false;
            neverShowFpsWarningToggle.isOn = false;
            eagleModeToggle.isOn = EagleModeController.IsActive;
        }

        protected override bool TryInitialize()
        {
            ResetValues();
            showDebugBarToggle.onValueChanged.AddListener(DebugBar.SetVisibility);
            hideResourceBarsToggle.onValueChanged.AddListener(delegate (bool value) { SetResourceBarsVisibility(isOn: !value); });
            neverShowFpsWarningToggle.onValueChanged.AddListener(delegate (bool value) { DebugBar.ShowFpsWarningDisplay(show: !value); });
            eagleModeToggle.onValueChanged.AddListener(SetEagleMode);
            return true;
        }

        private void SetResourceBarsVisibility(bool isOn)
        {
            var resourceBars = PlayerCharacterReference.Instance.GetComponentsInChildren<ResourceBar>(includeInactive: true);

            foreach (var resourceBar in resourceBars)
            {
                resourceBar.gameObject.SetActive(isOn);
            }
        }

        private void SetEagleMode(bool isOn)
        {
            if (isOn)
                EagleModeController.Activate();
            else
                EagleModeController.Deactivate();
        }
    }
}