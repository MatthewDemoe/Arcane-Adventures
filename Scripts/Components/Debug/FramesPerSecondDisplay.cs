using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    [RequireComponent(typeof(FramesPerSecondCalculator))]
    public class FramesPerSecondDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI averageDisplay;
        [SerializeField] private TextMeshProUGUI bestWorstDisplay;

        private void Start()
        {
            var framesPerSecondCalculator = GetComponent<FramesPerSecondCalculator>();
            framesPerSecondCalculator.AddOnFpsUpdatedListener(OnFpsUpdated);
        }

        private void OnFpsUpdated(float averageFps, float bestFps, float worstFps)
        {
            averageDisplay.text = $"{averageFps}";
            bestWorstDisplay.text = $"{bestFps}\n{worstFps}";
        }
    }
}