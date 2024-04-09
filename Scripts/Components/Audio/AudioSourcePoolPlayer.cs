using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public class AudioSourcePoolPlayer : MonoBehaviour
    {
        protected const float FadeOutDuration = 1.0f;

        [SerializeField]
        protected GameObject audioSourcePoolParent;

        [SerializeField]
        protected GameObject audioSourcePrefab;

        [SerializeField]
        float initialNumberOfAudioSources = 5.0f;

        protected List<AudioSource> audioSourcePool = new List<AudioSource>();

        public bool isPlaying => audioSourcePool.Any(audioSource => audioSource.gameObject.activeInHierarchy);

        protected virtual void Awake()
        {
            for (int i = 0; i < initialNumberOfAudioSources; i++)
            {
                audioSourcePool.Add(CreateNewAudioSource());
            }

            audioSourcePool.ForEach((audioSource) => audioSource.gameObject.SetActive(false));
        }

        public bool IsPlaying(AudioClipSettings audioClipSettings) => audioSourcePool.Any(audioSource => audioSource.gameObject.activeInHierarchy && audioSource.clip == audioClipSettings.audioClip);        

        public void PlayAudioClip(AudioClipSettings audioClipSettings)
        {
            if (audioClipSettings.interruptPreviousSounds)
                audioSourcePool.ForEach(audioSource =>
                {
                    if (audioSource.gameObject.activeInHierarchy)
                        StartCoroutine(FadeOutAudioClipRoutine(audioSource, waitUntilFinishedPlaying: true));
                });

            GetFirstAvailableAudioSource().PlayAudioClip(audioClipSettings.audioClip, audioClipSettings.settings);
        }

        public void PlayAudioClip(AudioClipSettings audioClipSettings, Transform transform)
        {
            if (audioClipSettings.interruptPreviousSounds)
                audioSourcePool.ForEach(audioSource =>
                {
                    if (audioSource.gameObject.activeInHierarchy)
                        StartCoroutine(FadeOutAudioClipRoutine(audioSource));
                });

            GetFirstAvailableAudioSource().PlayAudioClip(audioClipSettings.audioClip, audioClipSettings.settings, transform);
        }

        protected AudioSource GetFirstAvailableAudioSource()
        {
            AudioSource audioSource = audioSourcePool.FirstOrDefault((poolAudioSource) => !poolAudioSource.gameObject.activeInHierarchy);

            if (audioSource == null)
                audioSource = CreateNewAudioSource();

            return audioSource.GetComponent<AudioSource>(); ;
        }

        protected AudioSource CreateNewAudioSource()
        {
            AudioSource newAudioSource = Instantiate(audioSourcePrefab, audioSourcePoolParent.transform).GetComponent<AudioSource>();

            audioSourcePool.Add(newAudioSource);
            newAudioSource.gameObject.SetActive(false);

            return newAudioSource;
        }

        public void DetachAllSounds()
        {
            transform.parent = null;

            audioSourcePool.ForEach(audioSource =>
            {
                StartCoroutine(FadeOutAudioClipRoutine(audioSource));
            });

            StartCoroutine(DestroyWhenAllSoundsFinishRoutine());
        }

        public void StopAllSounds()
        {
            audioSourcePool.ForEach(audioSource =>
            {
                StartCoroutine(FadeOutAudioClipRoutine(audioSource, waitUntilFinishedPlaying: false));
            });
        }

        public void DisableAudioClipWhenFinished(AudioSource audioSource)
        {
            StartCoroutine(DisableAudioClipWhenFinishedRoutine(audioSource));
        }

        public IEnumerator DisableAudioClipWhenFinishedRoutine(AudioSource audioSource)
        {
            yield return new WaitUntil(() => !audioSource.isPlaying);

            audioSource.StopAudioClip(audioSourcePoolParent.transform);
        }

        protected IEnumerator FadeOutAudioClipRoutine(AudioSource audioSource, bool waitUntilFinishedPlaying = true)
        {
            float timer = 0.0f;
            float normalizedTime = 0.0f;

            if (!audioSource.loop && waitUntilFinishedPlaying)
            {
                yield return new WaitUntil(() => !audioSource.isPlaying);

                audioSource.StopAudioClip(audioSourcePoolParent.transform);
            }

            while (timer < FadeOutDuration)
            {
                timer += Time.deltaTime;

                normalizedTime = UtilMath.Lmap(timer, 0.0f, FadeOutDuration, 1.0f, 0.0f);

                audioSource.volume = normalizedTime;

                yield return null;
            }

            audioSource.StopAudioClip(audioSourcePoolParent.transform);
        }

        private IEnumerator DestroyWhenAllSoundsFinishRoutine()
        {
            yield return new WaitUntil(() => !isPlaying);

            Destroy(gameObject);
        }
    }
}