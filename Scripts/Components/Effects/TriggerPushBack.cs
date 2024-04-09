using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class TriggerPushBack : MonoBehaviour
    {
        [SerializeField]
        float force;

        private float radius;

        private void Start()
        {
            radius = GetComponent<CapsuleCollider>().radius * 1.15f;
        }

        public void PushBack(Collider other)
        {
            if (other.IsPlayerCharacter())
            {
                var direction = other.transform.position - transform.position;
                other.attachedRigidbody.AddForce(direction.normalized * force * (-direction.magnitude + radius) * Time.deltaTime);
            }
        }
    }
}
