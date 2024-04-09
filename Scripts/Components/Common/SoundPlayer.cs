using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    //TODO: Consider object pooling 
    public class SoundPlayer : MonoBehaviour
    {
        public static SoundPlayer Instance;

        [SerializeField]
        AudioClip earRinging;

        [SerializeField]
        AudioClip success;

        [SerializeField]
        AudioClip failure;

        public enum AudioClipNames { EarRinging, Success, Failure }

        public Dictionary<AudioClipNames, AudioClip> AudioClipsByName => new Dictionary<AudioClipNames, AudioClip>()
        {
            { AudioClipNames.EarRinging , earRinging},
            { AudioClipNames.Success , success},
            { AudioClipNames.Failure , failure},
        };

        private void Awake()
        {
            if (Instance is null)
                Instance = this;
        }

        public AudioSource CreateSoundObject(AudioClipNames audioClipToPlay, Transform parent = null, bool isLooping = false)
        {
            GameObject soundInstance = Instantiate(Prefabs.Load(Prefabs.PrefabNames.SoundObject), parent);

            AudioSource audioSource = soundInstance.GetComponent<AudioSource>();

            audioSource.clip = AudioClipsByName[audioClipToPlay];
            audioSource.loop = isLooping;

            audioSource.Play();

            return audioSource;
        }

        public void FadeOutAudioSource(AudioSource audioSource, float duration = 1.0f)
        {
            StartCoroutine(FadeOutRoutine(audioSource, duration));
        }

        IEnumerator FadeOutRoutine(AudioSource audioSource, float duration = 1.0f)
        {
            float timer = 0.0f;
            float normalizedTime = 0.0f;

            yield return null;

            while (timer < duration)
            {
                timer += Time.deltaTime;

                normalizedTime = UtilMath.Lmap(timer, 0.0f, duration, 1.0f, 0.0f);

                audioSource.volume = normalizedTime;

                yield return null;
            }

            Destroy(audioSource.gameObject);
        }
    }
}