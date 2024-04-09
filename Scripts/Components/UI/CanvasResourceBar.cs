using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class CanvasResourceBar : ResourceBar
    {
        [SerializeField] 
        private bool lookAtCamera = true;

        [SerializeField]
        Image mainBar;

        [SerializeField]
        Image laggingBar;

        protected override void Start()
        {
            base.Start();

            if (lookAtCamera)
            {
                var lookAt = this.gameObject.AddComponent<LookAt>();
                lookAt.target = Camera.main.gameObject;
            }
        }

        public override void ProcessResourceChange()
        {
            mainBar.fillAmount = currentResourcePercent;

            StopAllCoroutines();
            StartCoroutine(ScaleResourceBarsRoutine());
        }

        IEnumerator ScaleResourceBarsRoutine()
        {
            previousScale = laggingBar.fillAmount;

            laggingTimer = 0.0f;

            yield return new WaitForSeconds(LagDelay);

            while (laggingTimer <= LaggingEmptyTime)
            {
                float normalizedTime = laggingTimer / LaggingEmptyTime;

                laggingBar.fillAmount = UtilMath.EasingFunction.EaseOutExpo(previousScale, mainBar.fillAmount, normalizedTime);

                yield return null;
                laggingTimer += Time.deltaTime;
            }
        }
    }
}