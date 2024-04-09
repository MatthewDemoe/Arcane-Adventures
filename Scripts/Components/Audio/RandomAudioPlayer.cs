using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public class RandomAudioPlayer : MonoBehaviour
    {
        [SerializeField]
        AudioSourcePoolPlayer audioSourcePoolPlayer;

        [SerializeField]
        List<AudioClipSettings> audioClips;

        public void PlayAudio()
        {
            audioSourcePoolPlayer.PlayAudioClip(audioClips[Random.Range(0, audioClips.Count)]);
        }
    }
}