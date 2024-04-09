using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class MoveToGroundLevel : MonoBehaviour
    {
        void Awake()
        {
            if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 100.0f, 1 << (int)Lookups.Layers.Environment))
            {
                transform.position = hit.point;
            }
        }
    }
}