using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public abstract class Hit
    {
        public Creature hitSource { get; protected set; }
        public Creature target { get; protected set; }
        public bool reported { get; protected set; } = false;
        public int? healthChange { get; protected set; } = null;
        public int? hpAfterChange { get; protected set; } = null;
        public float damageFearMultiplier { get; set; } = 1.0f;

        public Hit(Creature attacker, Creature target)
        {
            this.hitSource = attacker;
            this.target = target;
        }

        public virtual void Report(int healthChange, int hpAfterChange)
        {
            if (reported)
            {
                throw new Exception("Cannot report hit twice");
            }

            this.healthChange = healthChange;
            this.hpAfterChange = hpAfterChange;
            reported = true;

            if (healthChange == 0)
                return;

            else if (healthChange > 0)
            {
                this.healthChange = (int)target.OnProcessHit.Invoke(this);
                target.OnDamageTaken();
            }

            else
            {
                this.healthChange = (int)target.OnProcessHeal.Invoke(this);
                target.OnHealthGained.Invoke();
            }
        }

        public void UpdateDamageFearMultiplier()
        {
            if (hitSource is PlayerCharacter && hitSource.statusConditionTracker.HasStatusCondition(AllStatusConditions.StatusConditionName.Taunted))
            {
                Taunted tauntedCondition = hitSource.statusConditionTracker.GetStatusCondition(AllStatusConditions.StatusConditionName.Taunted) as Taunted;

                if (tauntedCondition.tauntedStatusSettings.sourceCreature != target)
                    damageFearMultiplier = 0.25f;
            }
        }

        public abstract void CalculateHealthChange();
    }
}