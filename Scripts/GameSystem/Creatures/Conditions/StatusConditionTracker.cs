using System;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions
{
    public class StatusConditionTracker
    {
        protected List<StatusCondition> statusConditions = new List<StatusCondition>();

        public List<StatusCondition> GetStatusConditions => statusConditions;

        public delegate void ReportConditionAdded(StatusCondition statusCondition);
        public ReportConditionAdded OnConditionAdded = (statusCondition) => { };

        public delegate void ReportConditionRemoved(StatusCondition statusCondition);
        public ReportConditionRemoved OnConditionRemoved = (statusCondition) => { };
        public StatusCondition GetStatusCondition(AllStatusConditions.StatusConditionName statusConditionName)
        {
            return statusConditions.SingleOrDefault((statusCondition) => statusCondition.statusConditionName == statusConditionName);
        }

        public bool HasStatusCondition(AllStatusConditions.StatusConditionName statusConditionName) => GetStatusCondition(statusConditionName) != null;
        public bool HasOneOfStatusConditions(List<AllStatusConditions.StatusConditionName> statusConditionList)
        {
            foreach(AllStatusConditions.StatusConditionName statusCondition in statusConditionList)
            {
                if (HasStatusCondition(statusCondition))
                    return true;
            }

            return false;
        }


        public void AddStatusCondition(StatusCondition statusConditionToAdd)
        {
            if (HasStatusCondition(statusConditionToAdd.statusConditionName) || statusConditionToAdd.affectedCreature.modifiers.IsImmuneToStatusCondition(statusConditionToAdd.statusConditionName))
                return;

            statusConditions.Add(statusConditionToAdd);
            OnConditionAdded.Invoke(statusConditionToAdd);
        }

        public void RemoveStatusCondition(AllStatusConditions.StatusConditionName statusConditionName)
        {
            StatusCondition statusCondition = GetStatusCondition(statusConditionName);

            if (statusCondition == null)
                return;

            statusConditions.Remove(statusCondition);
            statusCondition.RemoveCondition();
            OnConditionRemoved.Invoke(statusCondition);
        }
    }
}