using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class LogDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI lastMessageDisplay;
        [SerializeField] private TextMeshProUGUI errorCountDisplay;
        [SerializeField] private TextMeshProUGUI assertCountDisplay;
        [SerializeField] private TextMeshProUGUI warningCountDisplay;
        [SerializeField] private TextMeshProUGUI logCountDisplay;
        [SerializeField] private TextMeshProUGUI exceptionCountDisplay;

        private readonly Dictionary<LogType, int> logCountByLogType = EnumExtensions.GetValues<LogType>()
            .ToDictionary(logType => logType, logType => 0);
        private bool initialized = false;

        public void Initialize()
        {
            if (!initialized)
            {
                Application.logMessageReceived += OnLogMessageReceived;
                initialized = true;
            }
        }

        private void OnLogMessageReceived(string condition, string stackTrace, LogType type)
        {
            lastMessageDisplay.text = $"[{DateTime.Now:HH:mm:ss}] [{type}] {condition}";
            logCountByLogType[type]++;
            errorCountDisplay.text = GetLogCountDisplay(LogType.Error);
            assertCountDisplay.text = GetLogCountDisplay(LogType.Assert);
            warningCountDisplay.text = GetLogCountDisplay(LogType.Warning);
            logCountDisplay.text = GetLogCountDisplay(LogType.Log);
            exceptionCountDisplay.text = GetLogCountDisplay(LogType.Exception);
        }

        private string GetLogCountDisplay(LogType type)
        {
            if (logCountByLogType[type] > 999)
            {
                return "+999";
            }
            else return logCountByLogType[type].ToString();
        }
    }
}