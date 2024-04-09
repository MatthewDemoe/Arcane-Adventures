using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{    
    public class StatusConditionModifier : MonoBehaviour
    {
        [SerializeField]
        AllStatusConditions.StatusConditionName statusCondition;

        private List<Creature> _targets = new List<Creature>();

        [SerializeField]
        bool removeOnDestroy = true;

        public void AddStatusCondition(Creature target)
        {
            if (_targets.Contains(target))
                return;

            _targets.Add(target);
            //TODO: Determine if this is still used
            target.statusConditionTracker.AddStatusCondition(AllStatusConditions.ConvertEnumToStatusCondition(statusCondition, ref target, new StatusConditionSettings(), "TODO: Determine if this is still used", startDuration : false));
        }

        public void RemoveStatusCondition(Creature target)
        {
            StatusCondition condition = target.statusConditionTracker.GetStatusCondition(statusCondition);

            if (condition == null)
                return;

            target.statusConditionTracker.RemoveStatusCondition(statusCondition);
            _targets.Remove(target);
        }

        public void AddStatusCondition(CreatureReference target)
        {
            AddStatusCondition(target.creature);
        }

        public void RemoveStatusCondition(CreatureReference target)
        {
            RemoveStatusCondition(target.creature);
        }

        public void StartDuration(Creature target)
        {
            target.statusConditionTracker.GetStatusCondition(statusCondition).StartDuration();
        }

        private void OnDestroy()
        {
            if (!removeOnDestroy)
                return;

            List<Creature> intermediateList = new List<Creature>(_targets);
        
            intermediateList.ForEach((target) => RemoveStatusCondition(target));
        }
    }
}