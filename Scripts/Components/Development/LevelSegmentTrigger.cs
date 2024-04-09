using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    [RequireComponent(typeof(Collider))]
    public class LevelSegmentTrigger : MonoBehaviour
    {
        [SerializeField] List<LevelSegment> levelSegmentsToEnable;
        [SerializeField] List<LevelSegment> levelSegmentsToDisable;

        private void OnTriggerEnter(Collider other)
        {
            foreach (var levelSegment in levelSegmentsToEnable)
            {
                levelSegment.gameObject.SetActive(true);
            }

            foreach (var levelSegment in levelSegmentsToDisable)
            {
                levelSegment.gameObject.SetActive(false);
            }
        }
    }
}