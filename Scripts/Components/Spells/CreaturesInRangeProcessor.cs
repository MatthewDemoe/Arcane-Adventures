using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class CreaturesInRangeProcessor : MonoBehaviour
    {
        List<CreatureReference> creaturesInRange = new List<CreatureReference>();

        public UnityEvent<GameObject> OnProcessGameObjects = new UnityEvent<GameObject>();
        public UnityEvent<CreatureReference> OnProcessCreatures = new UnityEvent<CreatureReference>();

        public void ProcessGameObjects()
        {
            creaturesInRange.ForEach((creatureReference) => OnProcessGameObjects.Invoke(creatureReference.gameObject));
        }

        public void ProcessCreatures()
        {
            creaturesInRange.ForEach((creatureReference) => OnProcessCreatures.Invoke(creatureReference));
        }

        private void OnTriggerEnter(Collider other)
        {
            var collisionCreatureReference = other.GetComponentInParent<CreatureReference>();
            if (collisionCreatureReference != null && !creaturesInRange.Contains(collisionCreatureReference))
                creaturesInRange.Add(collisionCreatureReference);
        }

        private void OnTriggerExit(Collider other)
        {
            var collisionCreatureReference = other.GetComponentInParent<CreatureReference>();
            if (collisionCreatureReference != null && creaturesInRange.Contains(collisionCreatureReference))
                creaturesInRange.Remove(collisionCreatureReference);
        }
    }
}