using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;

using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AddStatusConditionToTarget : MonoBehaviour, ISpellReferencer
    {
        Creature creature;

        [SerializeField]
        AllStatusConditions.StatusConditionName statusCondition;

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

                if (physicalSpell.correspondingSpell is StatusConditionSpell)
                    statusCondition = (physicalSpell.correspondingSpell as StatusConditionSpell).statusCondition;
            }
        }

        [SerializeField]
        int durationInMilliseconds = 5000;

        public void AddStatusCondition(Creature target)
        {
            if (!target.statusConditionTracker.HasStatusCondition(statusCondition))
            {
                StatusCondition statusConditionToAdd = AllStatusConditions.ConvertEnumToStatusCondition(statusCondition, ref target,
                    statusConditionSettings: new StatusConditionSettings(durationInMilliseconds: durationInMilliseconds) , physicalSpell.correspondingSpell.name
                    );

                target.statusConditionTracker.AddStatusCondition(statusConditionToAdd);
                creature = target;
            }
        }

        public void AddStatusCondition(GameObject target) => AddStatusCondition(target.GetComponent<CreatureReference>());

        public void AddStatusCondition(CreatureReference target)
        {
            Creature targetCreature = target.creature;

            AddStatusCondition(targetCreature);
        }


        public void ScaleConditionDuration(int amount)
        {
            durationInMilliseconds *= amount;
        }

        public void RemoveStatusCondition()
        {
            creature.statusConditionTracker.RemoveStatusCondition(statusCondition);
        }
    }
}