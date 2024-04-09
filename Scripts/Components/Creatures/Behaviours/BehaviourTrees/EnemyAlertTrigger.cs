using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class EnemyAlertTrigger : MonoBehaviour
    {
        SphereCollider alertCollider;
        const float Radius = 7.5f;

        void Start()
        {
            alertCollider = gameObject.AddComponent<SphereCollider>();
            alertCollider.isTrigger = true;
            alertCollider.radius = Radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            CreatureReference collidingCreatureReference = other.GetComponentInParent<CreatureReference>();

            if (collidingCreatureReference == null)
                return;

            if (collidingCreatureReference is PlayerCharacterReference)
            {
                CreatureBehaviour creatureBehaviour = GetComponent<CreatureBehaviour>();
                creatureBehaviour.mob.AlertMob(collidingCreatureReference.transform, creatureBehaviour);
            }
        }

        private void OnDestroy()
        {
            Destroy(alertCollider);
        }
    }
}