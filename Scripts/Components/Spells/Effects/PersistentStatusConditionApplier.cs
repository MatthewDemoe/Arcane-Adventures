using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using UnityEngine;
using UnityEngine.Serialization;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PersistentStatusConditionApplier : MonoBehaviour, ISpellReferencer
    {
        Creature creature;

        [SerializeField]
        AllStatusConditions.StatusConditionName statusCondition;

        [SerializeField]
        int durationInMilliseconds = 5000;

        [SerializeField][FormerlySerializedAs("applyOnAwake")]
        bool applyOnSpellSet = true;

        private PhysicalSpell _physicalSpell;
        public PhysicalSpell physicalSpell 
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;
                if (applyOnSpellSet)
                    AddStatusCondition(creature);

                physicalSpell.OnDurationEnd.AddListener(RemoveStatusConditionFromTarget);
            }
        }

        private void Awake()
        {
            creature = GetComponentInParent<CreatureReference>().creature;
        }

        public void AddStatusCondition(Creature target)
        {            
            target.statusConditionTracker.AddStatusCondition(AllStatusConditions.ConvertEnumToStatusCondition(statusCondition, ref target, 
                statusConditionSettings: new StatusConditionSettings(durationInMilliseconds: durationInMilliseconds), physicalSpell.correspondingSpell.name
                ));            
        }

        public void AddStatusConditionToTarget()
        {
            AddStatusCondition(creature);
        }

        public void RemoveStatusConditionFromTarget()
        {
            creature.statusConditionTracker.RemoveStatusCondition(statusCondition);
        }


        public void ScaleConditionLength(int amount)
        {
            durationInMilliseconds *= amount;
        }

        private void OnDestroy()
        {
            RemoveStatusConditionFromTarget();
        }
    }
}