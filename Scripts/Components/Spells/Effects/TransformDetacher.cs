using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class TransformDetacher : MonoBehaviour
    {
        public void DetachFromParent()
        {
            transform.SetParent(null);
        }
    }
}