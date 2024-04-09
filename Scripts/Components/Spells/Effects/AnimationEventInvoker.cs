using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AnimationEventInvoker : MonoBehaviour
    {
        [SerializeField]
        CharacterAnimationAudioSourcePoolPlayer audioSourcePlayer;

        public UnityEvent OnAnimationEnd = new UnityEvent();
        public UnityEvent OnAnimationInitialize = new UnityEvent();
        public UnityEvent OnAnimationCancelled = new UnityEvent();

        private void Start()
        {

            OnAnimationCancelled.AddListener(() => OnAnimationInitialize.RemoveAllListeners());
            OnAnimationCancelled.AddListener(() => OnAnimationEnd.RemoveAllListeners());
        }

        public void InitializeAnimation()
        {
            OnAnimationInitialize.Invoke();
        }

        public void EndAnimation()
        {
            OnAnimationEnd.Invoke();
        }

        public void CancelAnimation()
        {
            OnAnimationCancelled.Invoke();
        }

        public void PlayAudioClipByName(string name)
        {
            audioSourcePlayer.PlayAudioClipByName(name);
        }

        public void StopAllSounds()
        {
            audioSourcePlayer.StopAllSounds();
        }
    }
}