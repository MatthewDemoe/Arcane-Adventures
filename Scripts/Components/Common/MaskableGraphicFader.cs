using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class MaskableGraphicFader : MonoBehaviour
    {
        [SerializeField] private MaskableGraphic maskableGraphic;
        [SerializeField] private FadeAction action;
        [SerializeField] private float fadeInDurationInSeconds;
        [SerializeField] private float timeBetweenActionsInSeconds;
        [SerializeField] private float fadeOutDurationInSeconds;
        [SerializeField] private bool destroyOnFinish;

        private float targetAlpha;
        private bool finished = false;
        private float timeLeftBetweenActionsInSeconds;

        private void Start()
        {
            timeLeftBetweenActionsInSeconds = timeBetweenActionsInSeconds;
            var initialAlpha = (action.Equals(FadeAction.FadeIn) || action.Equals(FadeAction.FadeInThenOut)) ? 0 : 1;
            maskableGraphic.canvasRenderer.SetAlpha(initialAlpha);
            var fadeIn = action.Equals(FadeAction.FadeIn) || action.Equals(FadeAction.FadeInThenOut);
            StartFadeAction(fadeIn);
        }

        private void Update()
        {
            if (finished || !maskableGraphic || !maskableGraphic.canvasRenderer || maskableGraphic.canvasRenderer.GetAlpha() != targetAlpha)
            {
                return;
            }

            if (isFinishedWhenTargetIsMet)
            {
                Finish();
            }
            else
            {
                TryStartNextFadeAction();
            }
        }

        private bool isFinishedWhenTargetIsMet => 
                    (action.Equals(FadeAction.FadeIn) && targetAlpha == 1) ||
                    (action.Equals(FadeAction.FadeOut) && targetAlpha == 0) ||
                    (action.Equals(FadeAction.FadeOutThenIn) && targetAlpha == 1) ||
                    (action.Equals(FadeAction.FadeInThenOut) && targetAlpha == 0);

        private void Finish()
        {
            if (destroyOnFinish)
            {
                Destroy(this.gameObject);
            }

            finished = true;
        }

        private void TryStartNextFadeAction()
        {
            timeLeftBetweenActionsInSeconds -= Time.deltaTime;

            if (timeLeftBetweenActionsInSeconds <= 0)
            {
                StartFadeAction(fadeIn: action.Equals(FadeAction.FadeOutThenIn));
            }
        }

        private void StartFadeAction(bool fadeIn)
        {
            float duration;

            if (fadeIn)
            {
                targetAlpha = 1;
                duration = fadeInDurationInSeconds;
            }
            else
            {
                targetAlpha = 0;
                duration = fadeOutDurationInSeconds;
            }

            maskableGraphic.CrossFadeAlpha(targetAlpha, duration, false);
        }

        public enum FadeAction
        {
            FadeIn,
            FadeOut,
            FadeInThenOut,
            FadeOutThenIn
        }
    }
}