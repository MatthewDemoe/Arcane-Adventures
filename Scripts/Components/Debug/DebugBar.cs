using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class DebugBar : MonoBehaviour
    {
        [SerializeField] private Canvas debugBarCanvas;

        private static DebugBar Instance;
        
        private FpsWarningDisplay fpsWarningDisplay;

        public bool isOn { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple debug bar instances.");
            }

            Instance = this;

            fpsWarningDisplay = GetComponent<FpsWarningDisplay>();
            GetComponentInChildren<LogDisplay>(includeInactive: true).Initialize();
        }

        public static void SetVisibility(bool isOn)
        {
            Instance.isOn = isOn;
            Instance.debugBarCanvas.gameObject.SetActive(isOn);
        }

        public static void ShowFpsWarningDisplay(bool show) => Instance.fpsWarningDisplay.enabled = show;
    }
}