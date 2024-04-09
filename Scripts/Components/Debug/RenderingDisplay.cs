using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Profiling;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class RenderingDisplay : MonoBehaviour
    {
        private static readonly List<string> StatNamesToUse = new List<string>
        {
            StatNames.Render.SetPassCallsCount,
            StatNames.Render.DrawCallsCount,
            StatNames.Render.TotalBatchesCount,
            StatNames.Render.TrianglesCount,
            StatNames.Render.VerticesCount
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
            profileRecorderByStatName = StatNamesToUse.ToDictionary(statName => statName, statName => ProfilerRecorder.StartNew(ProfilerCategory.Render, statName));
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
            var values = StatNamesToUse.Select(statName => $"{profileRecorderByStatName[statName].LastValue}");
            valuesDisplay.text = string.Join("\n", values);
        }
    }
}