using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class DestroyAfterDuration : MonoBehaviour
    {
        [SerializeField]
        float duration = 0.0f;

        private void Start()
        {
            StartCoroutine(WaitThenDestroy());
        }

        IEnumerator WaitThenDestroy()
        {
            yield return new WaitForSeconds(duration);

            Destroy(gameObject);
        }
    }
}
