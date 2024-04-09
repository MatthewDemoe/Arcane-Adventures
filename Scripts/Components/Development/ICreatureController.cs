using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public interface ICreatureController
    {
        public Vector3 targetPosition { get; }
        public float targetSpeed { get; }
        public Vector3 lookPosition { get; }
        public bool crouch { get; }
        public bool jump { get; }
    }
}