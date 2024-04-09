using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class CameraFader : MonoBehaviour
    {
        public const float DefaultDuration = 1.0f;

        private static CameraFader Instance;

        [SerializeField] private Image image;

        private readonly UnityEvent onCompleted = new UnityEvent();

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            FadeIn();
        }

        public static void SetColor(Color32 color) => Instance.image.color = color;
        public static void SetAlpha(float alpha) => Instance.image.canvasRenderer.SetAlpha(alpha);
        public static void FadeIn() => FadeIn(DefaultDuration);
        public static void FadeOut() => FadeOut(DefaultDuration);
        public static void FadeIn(UnityAction onCompletedAction) => FadeIn(DefaultDuration, onCompletedAction);
        public static void FadeOut(UnityAction onCompletedAction) => FadeOut(DefaultDuration, onCompletedAction);
        public static void FadeIn(float duration, UnityAction onCompletedAction = null) => Instance.Fade(duration, onCompletedAction, 0);

        public static void FadeOut(float duration, UnityAction onCompletedAction = null)
        {
            Instance.onCompleted.AddListener(Instance.ResetColor);
            Instance.Fade(duration, onCompletedAction, 1);
        }

        private void Fade(float duration, UnityAction onCompletedAction, float targetAlpha)
        {
            if (onCompletedAction != null)
            {
                onCompleted.AddListener(onCompletedAction);
            }

            StartCoroutine(DoFade(duration));
            image.CrossFadeAlpha(targetAlpha, duration, false);
        }

        private void ResetColor()
        {
            image.color = new Color32(0, 0, 0, 255);
        }

        private IEnumerator DoFade(float duration = DefaultDuration)
        {
            yield return new WaitForSecondsRealtime(duration);

            onCompleted.Invoke();
            onCompleted.RemoveAllListeners();
        }
    }
}
