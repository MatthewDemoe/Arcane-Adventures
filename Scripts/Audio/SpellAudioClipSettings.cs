using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Audio
{
    [Serializable]
    public class SpellAudioClipSettings 
    {
        public bool isNull => leadingAudioClip.audioClip == null && mainAudioClip.audioClip == null;

        public bool overlapping = false;

        public AudioClipSettings leadingAudioClip;
        public AudioClipSettings mainAudioClip;
    }
}