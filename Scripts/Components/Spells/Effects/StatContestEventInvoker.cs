using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using UnityEngine;
using UnityEngine.Events;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public abstract class StatContestEventInvoker : MonoBehaviour
    {
        [SerializeField]
        protected Stats.Stat targetStat;

        [SerializeField]
        protected Stats.Stat initiatorStat;

        [SerializeField]
        AudioClipSettings successAudioSettings;

        [SerializeField]
        AudioClipSettings failureAudioSettings;

        public GameObjectStatContestEvents gameObjectStatContestEvents = new GameObjectStatContestEvents();
        public CreatureStatContestEvents creatureStatContestEvents = new CreatureStatContestEvents();

        protected AudioSourcePoolPlayer soundPlayer;

        protected virtual void Awake()
        {
            if(soundPlayer is null)
                soundPlayer = GetComponent<AudioSourcePoolPlayer>();

            gameObjectStatContestEvents.OnSuccess.AddListener((gameObject) => PlayAudioClip(successAudioSettings));
            gameObjectStatContestEvents.OnFailure.AddListener((gameObject) => PlayAudioClip(failureAudioSettings));
        }

        private void PlayAudioClip(AudioClipSettings audioSettings)
        {
            if (audioSettings.audioClip == null)
                return;

            soundPlayer.PlayAudioClip(audioSettings);
        }
    }    

    [Serializable]
    public class GameObjectStatContestEvents
    {
        public UnityEvent<GameObject> OnSuccess = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> OnFailure = new UnityEvent<GameObject>();
    }

    [Serializable]
    public class CreatureStatContestEvents
    {
        public UnityEvent<CreatureReference> OnSuccess = new UnityEvent<CreatureReference>();
        public UnityEvent<CreatureReference> OnFailure = new UnityEvent<CreatureReference>();
    }
}