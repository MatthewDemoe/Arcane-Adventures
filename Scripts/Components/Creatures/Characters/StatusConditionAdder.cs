using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public abstract class StatusConditionAdder : MonoBehaviour
    {
        protected List<StatusCondition> statusConditionsToRemove = new List<StatusCondition>();

        protected CreatureReference creatureReference;
        protected CreatureBehaviour creatureBehaviour;

        protected virtual void Start()
        {
            creatureReference = GetComponentInParent<CreatureReference>();
            creatureBehaviour = creatureReference.GetComponent<CreatureBehaviour>();

            creatureReference.creature.statusConditionTracker.OnConditionAdded += HandleStatusConditionAdded;
            creatureReference.creature.statusConditionTracker.OnConditionRemoved += HandleStatusConditionRemoved;
        }

        private void Update()
        {
            if (statusConditionsToRemove.Any())
                RemoveConditions();
        }

        protected virtual void HandleStatusConditionAdded(StatusCondition statusCondition)
        {
            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Disarmed)
            {
                GetComponentInParent<ItemEquipper.ItemEquipper>().UnequipItem(HandSide.Left);
                GetComponentInParent<ItemEquipper.ItemEquipper>().UnequipItem(HandSide.Right);
            }

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Dazed)
            {
                creatureBehaviour.timeBetweenAttacksMultiplier *= 2.0f;
            }
        }

        private void HandleStatusConditionRemoved(StatusCondition statusCondition)
        {
            statusConditionsToRemove.Add(statusCondition);            
        }

        protected virtual void RemoveConditions()
        {
            statusConditionsToRemove.ForEach((statusCondition) =>
            {
                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Dazed)
                {
                    creatureBehaviour.timeBetweenAttacksMultiplier /= 2.0f;
                }
            });
        }
    }
}