using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public class UIAudioSourcePlayer : AudioSourcePoolPlayer
    {
        [SerializeField]
        AudioClip hoverAudioClip;

        [SerializeField]
        AudioClip selectedAudioClip;   

        public void PlayHoverClip()
        {
            GetFirstAvailableAudioSource().PlayAudioClip(hoverAudioClip, new GeneralAudioClipSettings(loop: false, volume: 1.0f, pitch: 1.0f));
        }

        public void PlaySelectedAudioClip()
        {
            GetFirstAvailableAudioSource().PlayAudioClip(selectedAudioClip, new GeneralAudioClipSettings(loop: false, volume: 1.0f, pitch: 1.0f));
        }
    }
}