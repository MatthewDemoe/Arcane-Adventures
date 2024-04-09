using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.PostProcessing
{
    [RequireComponent(typeof(Volume))]
    public class TookDamageEffect : MonoBehaviour
    {
        private const float IntensityChangePerSecond = 2;
        public static TookDamageEffect Instance { get; private set; }

        private Volume volume;
        private Vignette vignette;
        private bool isActive;
        private bool isIncreasingIntensity;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            volume = gameObject.GetComponent<Volume>();

            if (!volume.profile.TryGet(out vignette))
            {
                throw new System.Exception("Vignette not present on volume");
            }
        }

        private void Update()
        {
            if (vignette == null || !isActive)
            {
                return;
            }

            if (isIncreasingIntensity)
            {
                vignette.intensity.value += IntensityChangePerSecond * Time.deltaTime;

                if (vignette.intensity.max == vignette.intensity.value)
                {
                    isIncreasingIntensity = false;
                }
            }
            else
            {
                vignette.intensity.value -= IntensityChangePerSecond * Time.deltaTime;

                if (vignette.intensity.min == vignette.intensity.value)
                {
                    isActive = false;
                }
            }
        }

        public void Activate()
        {
            isIncreasingIntensity = true;
            isActive = true;
        }
    }
}