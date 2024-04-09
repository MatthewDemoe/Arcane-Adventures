using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class TranslateOverTime : MonoBehaviour
    {
        [SerializeField]
        float duration = 3.0f;

        [SerializeField]
        Vector3 startPos = new Vector3();

        [SerializeField]
        Vector3 endPos = new Vector3();

        // Start is called before the first frame update
        void Start()
        {
            StartTranslate();
        }

        void StartTranslate()
        {
            StartCoroutine(nameof(TranslateRoutine));
        }

        IEnumerator TranslateRoutine()
        {
            float dt = 0.0f;

            while (dt < duration)
            {
                transform.position = Vector3.Lerp(startPos, endPos, UtilMath.Lmap(dt, 0.0f, duration, 0.0f, 1.0f));

                yield return null;
                dt += Time.deltaTime;
            }
        }
    }
}
