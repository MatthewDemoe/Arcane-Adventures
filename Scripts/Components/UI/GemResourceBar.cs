using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class GemResourceBar : ResourceBar
    {
        [SerializeField]
        List<MeshRenderer> shardRenderers;

        Color shardBaseColor = Color.white;

        float fadingResourcePercent = 1.0f;

        public override void ProcessResourceChange()
        {
            StopAllCoroutines();
            StartCoroutine(FadeGemsOverTimeRoutine());
        }

        IEnumerator FadeGemsOverTimeRoutine()
        {
            laggingTimer = 0.0f;
            float normalizedTime = 0.0f;

            while (laggingTimer < LaggingEmptyTime)
            { 
                yield return null;

                laggingTimer += Time.deltaTime;
                normalizedTime = UtilMath.Lmap(laggingTimer, 0.0f, LaggingEmptyTime, 0.0f, 1.0f);

                fadingResourcePercent = UtilMath.Lmap(normalizedTime, 0.0f, 1.0f, fadingResourcePercent, currentResourcePercent);

                int gemToSelect = (int)(fadingResourcePercent * 5);

                if (gemToSelect < 5)
                {
                    float gemFadeAmount = (fadingResourcePercent * 5.0f) % 1;

                    shardRenderers[gemToSelect].material.color = shardBaseColor * gemFadeAmount;
                }
            }
        }
    }
}