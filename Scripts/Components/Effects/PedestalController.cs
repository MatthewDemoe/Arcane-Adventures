using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class PedestalController : MonoBehaviour
    {
        [SerializeField]
        Transform endTransform;
        Vector3 endPosition;
        Vector3 beginPosition;

        [SerializeField]
        float duration = 2.0f;

        [SerializeField]
        float shakeInterval = 0.05f;

        [SerializeField]
        float shakeIntensity = 0.1f;

        [SerializeField]
        bool onStart = true;

        private void Start()
        {
            beginPosition = transform.position;
            endPosition = endTransform.position;

            if (onStart)
                StartCoroutine(nameof(TranslateRoutine));
        }

        public void Forward()
        {
            StartCoroutine(nameof(TranslateRoutine));
        }

        public void Reverse()
        {
            Vector3 tmp = endPosition;

            endPosition = beginPosition;
            beginPosition = tmp;

            StartCoroutine(nameof(TranslateRoutine));
        }

        private float DetermineYTranslation(float dt)
        {
            float n = UtilMath.Lmap(dt, 0.0f, duration, 0.0f, 1.0f);

            float newY = UtilMath.EasingFunction.EaseInOutExpo(beginPosition.y, endPosition.y, n);

            return newY;
        }

        IEnumerator TranslateRoutine()
        {
            float dt = 0.0f;

            float xDir = 1.0f;
            float setInterval = 0.0f;
            float i = shakeIntensity;

            while (dt < duration)
            {
                i = UtilMath.Lmap(dt, 0.0f, duration, shakeIntensity, 0.0f);

                transform.position = new Vector3(transform.position.x + (xDir * Time.deltaTime * i), DetermineYTranslation(dt), transform.position.z);

                yield return null;
                dt += Time.deltaTime;
                setInterval += Time.deltaTime;

                if (setInterval >= shakeInterval)
                {
                    setInterval = 0.0f;
                    xDir *= -1.0f;
                }
            }
        }
    }
}
