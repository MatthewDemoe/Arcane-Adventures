using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class VignetteEnabler : MonoBehaviour
    {
        [SerializeField]
        Image image;
        public static VignetteEnabler Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void TurnOn()
        {
            Color color = Instance.image.color;
            color.a = 1;

            StartCoroutine(TransitionColor(color, 0.25f));
        }

        public void TurnOff()
        {
            Color color = Instance.image.color;
            color.a = 0;

            StartCoroutine(TransitionColor(color));
        }

        IEnumerator TransitionColor(Color finalColor, float duration = 1.0f)
        {
            float timer = 0;
            Color initialColor = Instance.image.color;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                float normalizedTime = UtilMath.Lmap(timer, 0.0f, duration, 0.0f, 1.0f);

                Instance.image.color = Color.Lerp(initialColor, finalColor, normalizedTime);

                yield return null;
            }
        }
    }
}