using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public abstract class DebugMenuPage : MonoBehaviour
    {
        public void Initialize()
        {
            initialized = TryInitialize();
        }

        protected abstract bool TryInitialize();
        public virtual void ResetValues() { }

        public bool initialized { get; private set; }
    }
}