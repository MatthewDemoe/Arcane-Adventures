using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.VFX
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemFunctionCaller : MonoBehaviour
    {
        ParticleSystem attachedParticleSystem;

        private void Awake()
        {
            attachedParticleSystem = GetComponent<ParticleSystem>();
        }

        public void StopEmittingButKeepAlive()
        {
            attachedParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }
}