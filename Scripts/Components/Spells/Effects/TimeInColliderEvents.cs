using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class TimeInColliderEvents : MonoBehaviour
{
    [SerializeField]
    float timeNeededInCollider = 1.0f;

    [SerializeField]
    UnityEvent<Creature> OnExitTimeReached = new UnityEvent<Creature>();

    [SerializeField]
    UnityEvent<Creature> OnExitTimeNotReached = new UnityEvent<Creature>();

    Dictionary<CreatureReference, float> timeInColliderByCreatureReference = new Dictionary<CreatureReference, float>();

    private void OnTriggerEnter(Collider other)
    {
        var collisionCreatureReference = other.GetComponentInParent<CreatureReference>();
        if (collisionCreatureReference != null && !timeInColliderByCreatureReference.ContainsKey(collisionCreatureReference))
            timeInColliderByCreatureReference.Add(collisionCreatureReference, Time.time);
    }

    private void OnTriggerExit(Collider other)
    {
        var collisionCreatureReference = other.GetComponentInParent<CreatureReference>();

        InvokeEvent(collisionCreatureReference);
    }

    private void OnDestroy()
    {
        var intermediateCreatureReferences = timeInColliderByCreatureReference;

        intermediateCreatureReferences.Keys.ToList().ForEach((creatureReference) => InvokeEvent(creatureReference));
    }

    private void InvokeEvent(CreatureReference creatureReference)
    {
        if (creatureReference != null && timeInColliderByCreatureReference.ContainsKey(creatureReference))
        {
            float elapsedTime = Time.time - timeInColliderByCreatureReference[creatureReference];
            
            if (elapsedTime >= timeNeededInCollider)
                OnExitTimeReached.Invoke(creatureReference.creature);

            else
                OnExitTimeNotReached.Invoke(creatureReference.creature);

            timeInColliderByCreatureReference.Remove(creatureReference);
        }
    }
}
