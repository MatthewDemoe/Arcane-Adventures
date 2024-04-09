using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class TriggerEvents : MonoBehaviour
    {
        public ColliderTriggerEvents colliderEvents = new ColliderTriggerEvents();
        public GameObjectTriggerEvents gameObjectEvents = new GameObjectTriggerEvents();
        public CreatureTriggerEvents creatureEvents = new CreatureTriggerEvents();

        private List<CreatureReference> creaturesEntered = new List<CreatureReference>();
        private Dictionary<CreatureReference, int> numberOfSurfacesByCreatureReference = new Dictionary<CreatureReference, int>();

        [SerializeField]
        bool ignorePlayer = false;

        private void OnTriggerEnter(Collider other)
        {
            CreatureReference collisionCreature = GetCreature(other);

            if (ignorePlayer && (collisionCreature is PlayerCharacterReference))
                return;

            colliderEvents.OnEnter.Invoke(other);
            gameObjectEvents.OnEnter.Invoke(other.gameObject);

            if (collisionCreature == null)
                return;

            if (creaturesEntered.Contains(collisionCreature))
            {
                numberOfSurfacesByCreatureReference[collisionCreature]++;
                return;
            }

            creatureEvents.OnEnter.Invoke(collisionCreature);
            creaturesEntered.Add(collisionCreature);
            numberOfSurfacesByCreatureReference.Add(collisionCreature, 1);            
        }

        private void OnTriggerStay(Collider other)
        {
            CreatureReference collisionCreature = GetCreature(other);

            if (ignorePlayer && (collisionCreature is PlayerCharacterReference))
                return;

            colliderEvents.OnStay.Invoke(other);
            gameObjectEvents.OnStay.Invoke(other.gameObject);            

            if (collisionCreature != null)
                creatureEvents.OnStay.Invoke(collisionCreature);
        }

        private void OnTriggerExit(Collider other)
        {
            CreatureReference collisionCreature = GetCreature(other);

            if (ignorePlayer && (collisionCreature is PlayerCharacterReference))
                return;

            colliderEvents.OnExit.Invoke(other);
            gameObjectEvents.OnExit.Invoke(other.gameObject);

            if (collisionCreature == null || !creaturesEntered.Contains(collisionCreature))
                return;

            if (--numberOfSurfacesByCreatureReference[collisionCreature] > 0)
                return;

            creatureEvents.OnExit.Invoke(collisionCreature);
            creaturesEntered.Remove(collisionCreature);
            numberOfSurfacesByCreatureReference.Remove(collisionCreature);
        }

        private CreatureReference GetCreature(Collider collider)
        {
            CreatureReference collisionCreatureReference = collider.GetComponentInParent<CreatureReference>();

            if (collisionCreatureReference == null)
                return null;

            return collisionCreatureReference;
        }

        private void OnDestroy()
        {
            creaturesEntered.ForEach((creatureReference) =>
            {
                creatureEvents.OnExit.Invoke(creatureReference);
            });

            creaturesEntered.Clear();
        }
    }

    [Serializable]
    public class ColliderTriggerEvents
    {
        [SerializeField]
        public UnityEvent<Collider> OnEnter = new UnityEvent<Collider>();

        [SerializeField]
        public UnityEvent<Collider> OnStay = new UnityEvent<Collider>();

        [SerializeField]
        public UnityEvent<Collider> OnExit = new UnityEvent<Collider>();
    }

    [Serializable]
    public class GameObjectTriggerEvents
    {
        [SerializeField]
        public UnityEvent<GameObject> OnEnter = new UnityEvent<GameObject>();

        [SerializeField]
        public UnityEvent<GameObject> OnStay = new UnityEvent<GameObject>();

        [SerializeField]
        public UnityEvent<GameObject> OnExit = new UnityEvent<GameObject>();
    }

    [Serializable]
    public class CreatureTriggerEvents
    {
        [SerializeField]
        public UnityEvent<CreatureReference> OnEnter = new UnityEvent<CreatureReference>();

        [SerializeField]
        public UnityEvent<CreatureReference> OnStay = new UnityEvent<CreatureReference>();

        [SerializeField]
        public UnityEvent<CreatureReference> OnExit = new UnityEvent<CreatureReference>();
    }
}