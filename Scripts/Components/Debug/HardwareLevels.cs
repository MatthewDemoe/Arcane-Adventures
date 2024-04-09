using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class HardwareLevels : DebugMenuPage
    {
        [SerializeField] private Slider cpuSlider;
        [SerializeField] private TextMeshProUGUI cpuValueDisplay;

        [SerializeField] private Slider gpuSlider;
        [SerializeField] private TextMeshProUGUI gpuValueDisplay;

        [SerializeField] private Slider colorGamutSlider;
        [SerializeField] private TextMeshProUGUI colorGamutDisplay;

        [SerializeField] private Button applyButton;

        private int currentCpuLevel;
        private int currentGpuLevel;
        private int currentColorGamut;

        public override void ResetValues()
        {
            cpuSlider.value = currentCpuLevel;
            gpuSlider.value = currentGpuLevel;
            colorGamutSlider.value = currentColorGamut;
        }

        protected override bool TryInitialize()
        {
            currentCpuLevel = Unity.XR.Oculus.Stats.AdaptivePerformance.CPULevel;
            currentGpuLevel = Unity.XR.Oculus.Stats.AdaptivePerformance.GPULevel;
            currentColorGamut = 0;//TODO: "(int)OVRPlugin.GetHmdColorDesc();" didn't work (it should?), commenting out for now to avoid exception clutter.

            cpuSlider.onValueChanged.AddListener(delegate (float value) { cpuValueDisplay.text = value.ToString(); });
            gpuSlider.onValueChanged.AddListener(delegate (float value) { gpuValueDisplay.text = value.ToString(); });
            colorGamutSlider.onValueChanged.AddListener(delegate (float value) { colorGamutDisplay.text = value.ToString(); });

            applyButton.onClick.AddListener(Apply);
            ResetValues();

            return true;
        }

        private void Apply()
        {
            var newCpuLevel = (int)cpuSlider.value;
            var newGpuLevel = (int)gpuSlider.value;
            var newColorGamut = (int)colorGamutSlider.value;

            if (newCpuLevel != currentCpuLevel)
            {
                if (Unity.XR.Oculus.Performance.TrySetCPULevel(newCpuLevel))
                {
                    currentCpuLevel = newCpuLevel;
                }
                else throw new System.Exception("Error setting CPU level");
            }

            if (newGpuLevel != currentGpuLevel)
            {
                if (Unity.XR.Oculus.Performance.TrySetGPULevel(newGpuLevel))
                {
                    currentGpuLevel = newGpuLevel;
                }
                else throw new System.Exception("Error setting GPU level");
            }

            if (newColorGamut != currentColorGamut)
            {
                OVRPlugin.SetClientColorDesc((OVRPlugin.ColorSpace) newColorGamut);
            }
        }
    }
}