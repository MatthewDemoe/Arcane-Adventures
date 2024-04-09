using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public static class AudioSourceExtensions
    {
        public static void PlayAudioClip(this AudioSource audioSource, AudioClip audioClip, GeneralAudioClipSettings audioClipSettings)
        {
            audioSource.gameObject.SetActive(true);

            audioSource.clip = audioClip;
            audioSource.loop = audioClipSettings.loop;
            audioSource.volume = audioClipSettings.volume;
            audioSource.pitch = audioClipSettings.pitch;

            audioSource.Play();

            audioSource.GetComponentInParent<AudioSourcePoolPlayer>().DisableAudioClipWhenFinished(audioSource);
        }

        public static void PlayAudioClip(this AudioSource audioSource, AudioClip audioClip, GeneralAudioClipSettings audioClipSettings, Transform transform)
        {
            PlayAudioClip(audioSource, audioClip, audioClipSettings);

            if(transform != null)
                audioSource.gameObject.transform.parent = transform;
        }

        public static void StopAudioClip(this AudioSource audioSource, Transform transform)
        {
            audioSource.Stop();
            audioSource.gameObject.SetActive(false);

            if (transform != null)
                audioSource.gameObject.transform.parent = transform;
        }
    }
}