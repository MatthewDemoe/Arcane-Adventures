using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioSourcePlayer : MonoBehaviour
    {
        [SerializeField]
        protected AudioSource audioSource;        

        protected const float PitchShiftRange = 0.05f;

        public AudioClipSettings audioClipSettings;

        public virtual bool isPlaying => IsPlayingOnce(audioSource);

        protected void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.pitch = Random.Range(1.0f - PitchShiftRange, 1.0f + PitchShiftRange);
        }

        public virtual void PlayAudioClip()
        {
            audioSource.volume = Mathf.Clamp(audioClipSettings.settings.volume, 0.0f, 1.0f);

            audioSource.clip = audioClipSettings.audioClip;

            audioSource.Play();
        }

        protected bool IsPlayingOnce(AudioSource audioSource)
        {
            return audioSource != null && audioSource.isPlaying && !audioSource.loop;
        }

        protected IEnumerator DestroyWhenAudioClipFinishes()
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);

            StopAllCoroutines();
            Destroy(gameObject);
        }
    }
}