using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class MemoryUsageDisplay : MonoBehaviour
    {
        private static readonly List<string> StatNamesToUse = new List<string>
        {
            StatNames.Memory.TotalUsedMemory,
            StatNames.Memory.TotalReservedMemory,
            StatNames.Memory.GCUsedMemory,
            StatNames.Memory.GCReservedMemory
        };

        [SerializeField] private TextMeshProUGUI labelsDisplay;
        [SerializeField] private TextMeshProUGUI valuesDisplay;

        private Dictionary<string, ProfilerRecorder> profileRecorderByStatName;

        private void Awake()
        {
            var labels = StatNamesToUse.Select(statName => StatNames.Shorten(statName));
            labelsDisplay.text = string.Join("\n", labels);
        }

        private void OnEnable()
        {
            profileRecorderByStatName = StatNamesToUse.ToDictionary(statName => statName, statName => ProfilerRecorder.StartNew(ProfilerCategory.Memory, statName));
        }

        private void OnDisable()
        {
            foreach (var profilerRecorder in profileRecorderByStatName.Select(pair => pair.Value))
            {
                profilerRecorder.Dispose();
            }

            profileRecorderByStatName = null;
        }

        private void Update()
        {
            var values = StatNamesToUse
                .Select(statName => (int)(profileRecorderByStatName[statName].LastValue / 1024 / 1024))
                .Select(mb => $"{Math.Round(mb / 1024f, 2)}");

            valuesDisplay.text = string.Join("\n", values);
        }
    }
}