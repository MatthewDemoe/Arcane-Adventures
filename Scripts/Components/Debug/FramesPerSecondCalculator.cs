using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class FramesPerSecondCalculator : MonoBehaviour
    {
        private int framesRendered;
        private float timePassed;
        private float bestFrameRenderTime;
        private float worstFrameRenderTime;

        public delegate void OnFpsUpdated(float averageFps, float bestFps, float worstFps);
        private event OnFpsUpdated onFpsUpdated;

        private void Start()
        {
            ResetValues();
        }

        private void Update()
        {
            CalculateRenderTime(Time.unscaledDeltaTime);

            if (timePassed >= 1)
            {
                UpdateFpsValues();
                ResetValues();
            }
        }

        private void CalculateRenderTime(float frameRenderTime)
        {
            framesRendered++;
            timePassed += frameRenderTime;

            if (frameRenderTime < bestFrameRenderTime)
            {
                bestFrameRenderTime = frameRenderTime;
            }

            if (frameRenderTime > worstFrameRenderTime)
            {
                worstFrameRenderTime = frameRenderTime;
            }
        }

        private void UpdateFpsValues()
        {
            var averageFps = (float) Math.Round(framesRendered / timePassed, 2);
            var bestFps = (float)Math.Round(1f / bestFrameRenderTime, 2);
            var worstFps = (float) Math.Round(1f / worstFrameRenderTime, 2);

            if (onFpsUpdated != null)
            {
                onFpsUpdated.Invoke(averageFps, bestFps, worstFps);
            }
        }

        private void ResetValues()
        {
            framesRendered = 0;
            timePassed = 0f;
            bestFrameRenderTime = float.MaxValue;
            worstFrameRenderTime = 0f;
        }

        public void AddOnFpsUpdatedListener(OnFpsUpdated listener) => onFpsUpdated += listener;
        public void RemoveOnFpsUpdatedListener(OnFpsUpdated listener) => onFpsUpdated -= listener;
    }
}