using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    [RequireComponent(typeof(DebugBar), typeof(FramesPerSecondCalculator))]
    public class FpsWarningDisplay : MonoBehaviour
    {
        [SerializeField] private Canvas canvas;
        [SerializeField] private float fpsWarningThreshold;

        private DebugBar debugBar;
        private TextMeshProUGUI textMeshProUGUI;
        private FramesPerSecondCalculator framesPerSecondCalculator;

        private void Awake()
        {
            debugBar = GetComponent<DebugBar>();
            textMeshProUGUI = canvas.GetComponentInChildren<TextMeshProUGUI>();
            framesPerSecondCalculator = GetComponent<FramesPerSecondCalculator>();
            
        }

        private void OnEnable()
        {
            framesPerSecondCalculator.AddOnFpsUpdatedListener(OnFpsUpdated);
        }

        private void OnFpsUpdated(float averageFps, float bestFps, float worstFps)
        {
            var activate = !debugBar.isOn && averageFps < fpsWarningThreshold;

            if (activate)
            {
                textMeshProUGUI.text = $"FPS:\n{averageFps}";
            }

            canvas.gameObject.SetActive(activate);
        }

        private void OnDisable()
        {
            framesPerSecondCalculator.RemoveOnFpsUpdatedListener(OnFpsUpdated);
            canvas.gameObject.SetActive(false);
        }
    }
}