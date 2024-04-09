using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Audio
{
    public class FootstepPlayer : RandomAudioPlayer
    {
        public void PlayFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight <= 0.5f)
                return;

            PlayAudio();
        }
    }
}