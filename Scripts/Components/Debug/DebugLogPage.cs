using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class DebugLogPage : DebugMenuPage
    {
        [SerializeField] bool includeStackTrace;
        [SerializeField] TextMeshProUGUI debugOutput;
        [SerializeField] private int debugMessagesDisplayed;
        [SerializeField] private Toggle activateOutputToggle;

        private readonly List<string> messages = new List<string>();
        private bool isOn = false;

        public override void ResetValues()
        {
            activateOutputToggle.isOn = false;
            messages.Clear();
        }

        protected override bool TryInitialize()
        {
            debugOutput.text = string.Empty;
            ResetValues();
            Application.logMessageReceived += OnLogMessageReceived;
            activateOutputToggle.onValueChanged.AddListener(SetLoggingState);

            return true;
        }

        private void SetLoggingState(bool setToOn)
        {
            isOn = setToOn;
            debugOutput.text = isOn ? string.Join("\n\n", messages) : string.Empty;
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            var message = $"[{DateTime.Now:HH:mm:ss}] [{type}] {condition}";

            if (includeStackTrace)
            {
                message += $"\n{stackTrace}";
            }

            messages.Add(message);

            if (messages.Count > debugMessagesDisplayed)
            {
                messages.RemoveAt(0);
            }

            if (isOn)
            {
                debugOutput.text = string.Join("\n\n", messages);
            }
        }
    }
}