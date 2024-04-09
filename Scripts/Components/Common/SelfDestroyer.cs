using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class SelfDestroyer : MonoBehaviour
    {
        [SerializeField]
        float destructionDelay = 0.0f;

        [SerializeField]
        bool triggerOnStart = false;

        private void Start()
        {
            if (triggerOnStart)
                DestroySelfAfterDelay();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        public void DestroySelfAfterDelay()
        {
            StartCoroutine(DestroyAfterDelayRoutine());
        }

        IEnumerator DestroyAfterDelayRoutine()
        {
            yield return new WaitForSeconds(destructionDelay);

            DestroySelf();
        }
    }
}