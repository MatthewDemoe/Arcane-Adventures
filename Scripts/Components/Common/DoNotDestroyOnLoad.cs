using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}