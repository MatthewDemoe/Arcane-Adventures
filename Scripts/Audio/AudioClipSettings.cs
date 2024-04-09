using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Audio
{
    [Serializable]
    public class AudioClipSettings
    {
        public AudioClip audioClip;
        
        public bool interruptPreviousSounds = false;

        public GeneralAudioClipSettings settings;
    }
}