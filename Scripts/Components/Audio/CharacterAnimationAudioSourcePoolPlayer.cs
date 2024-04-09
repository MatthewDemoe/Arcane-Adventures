using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using System.Collections.Generic;
using System;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public class CharacterAnimationAudioSourcePoolPlayer : AudioSourcePoolPlayer
    {
        public List<AnimationAudioClipSettings> audioClipSettings = new List<AnimationAudioClipSettings>();

        public void PlayAudioClipByName(string name)
        {
            AnimationAudioClipSettings settings = audioClipSettings.FirstOrDefault(audioClipSettings => audioClipSettings.name.Equals(name));

            if (settings == null)
                throw new Exception($"Audio Clip {name} not found in list");
            
            if (!IsPlaying(settings))
                PlayAudioClip(settings);
        }
    }
}