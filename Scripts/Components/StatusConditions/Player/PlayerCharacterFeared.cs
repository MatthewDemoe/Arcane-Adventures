using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions.Player
{
    public class PlayerCharacterFeared : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> creaturesInRange = new List<GameObject>();

        PlayerCharacterMovementController playerMovementController;

        SphereCollider fearCollider;

        void Start()
        {
            playerMovementController = PlayerCharacterReference.Instance.GetComponent<PlayerCharacterMovementController>();

            fearCollider = gameObject.AddComponent<SphereCollider>();
         
            fearCollider.isTrigger = true;
            fearCollider.radius = 10.0f;
        }

        void Update()
        {
            playerMovementController.SetMovementInput(GetFearDirection(), true);
        }

        private void OnTriggerEnter(Collider other)
        {
            CreatureReference creatureInRange;

            if(!other.TryGetComponent(out creatureInRange))
                creatureInRange = other.gameObject.GetComponentInParent<CreatureReference>();

            if (creatureInRange == null || creatureInRange == PlayerCharacterReference.Instance || creaturesInRange.Contains(creatureInRange.gameObject))
                return;

            creaturesInRange.Add(creatureInRange.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            CreatureReference creatureInRange;

            if (!other.TryGetComponent(out creatureInRange))
                creatureInRange = other.gameObject.GetComponentInParent<CreatureReference>();

            if (creatureInRange == null || !creaturesInRange.Contains(creatureInRange.gameObject))
                return;

            creaturesInRange.Remove(creatureInRange.gameObject);
        }

        private Vector2 GetFearDirection()
        {
            Vector3 averagePosition = Vector3.zero;

            creaturesInRange.ForEach((creature) =>
            {
                averagePosition += creature.transform.position;
            });

            averagePosition /= creaturesInRange.Count;

            Vector2 averageXZ = new Vector2(averagePosition.x, averagePosition.z);
            Vector2 playerXZ = new Vector2(transform.position.x, transform.position.z);

            return (playerXZ - averageXZ).normalized; 
        }

        private void OnDestroy()
        {
            Destroy(fearCollider);
        }
    }
}