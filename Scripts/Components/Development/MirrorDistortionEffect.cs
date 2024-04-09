using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class MirrorDistortionEffect : MonoBehaviour
    {
        private const float DefaultLensDistortionValue = 0;
        private const float DefaultChannelMixerGreenValue = 100;
        private const float EffectStartDistance = 7;
        private const float EffectMaxedDistance = 2;

        [SerializeField] private GameObject mirror;

        private LensDistortion lensDistortion;
        private ChannelMixer channelMixer;

        private void Start()
        {
            var volume = GetComponent<Volume>();

            if (!volume.profile.TryGet(out lensDistortion) || !volume.profile.TryGet(out channelMixer))
            {
                throw new System.Exception("Required overrides not present on volume");
            }

            lensDistortion.intensity.value = DefaultLensDistortionValue;
            channelMixer.greenOutGreenIn.value = DefaultChannelMixerGreenValue;
        }

        private void Update()
        {
            var distance = Vector3.Distance(mirror.transform.position, PlayerCharacterReference.Instance.transform.position);
            var lensDistortionValue = DefaultLensDistortionValue;
            var greenValue = DefaultChannelMixerGreenValue;

            if (distance < EffectStartDistance)
            {
                var effectValue = Mathf.InverseLerp(EffectMaxedDistance, EffectStartDistance, distance);
                lensDistortionValue = effectValue - 1;
                greenValue = effectValue * 100;
            }

            lensDistortion.intensity.value = lensDistortionValue;
            channelMixer.greenOutGreenIn.value = greenValue;
        }
    }
}