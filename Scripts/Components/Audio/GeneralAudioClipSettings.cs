using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    [Serializable]
    public struct GeneralAudioClipSettings
    {
        public GeneralAudioClipSettings(bool loop = false, float volume = 1.0f, float pitch = 1.0f)
        {
            this.loop = loop;
            this.volume = volume;
            this.pitch = pitch;
        }

        public bool loop;
        public float volume;
        public float pitch;
    }
}