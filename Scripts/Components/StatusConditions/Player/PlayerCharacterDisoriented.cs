using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions.Player
{
    public class PlayerCharacterDisoriented : MonoBehaviour
    {
        private AudioSource earRingingAudio;

        void Start()
        {
            VignetteEnabler.Instance.TurnOn();
            earRingingAudio = SoundPlayer.Instance.CreateSoundObject(SoundPlayer.AudioClipNames.EarRinging, transform, isLooping: true);
        }

        private void OnDestroy()
        {
            VignetteEnabler.Instance.TurnOff();
            SoundPlayer.Instance.FadeOutAudioSource(earRingingAudio);
        }
    }
}