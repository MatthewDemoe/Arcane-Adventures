using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    public enum CreatureModifier { MoveSpeed, DamageTaken, TrueDamageDealt, CriticalHitChance, LifeSteal, KnockBack, WeaponDamageDealt, SpellDamageDealt ,}

    public class CreatureModifiers
    {
        public List<CreatureEffect> effects { get; private set; } = new List<CreatureEffect>();

        public delegate void MoveSpeedChanged();
        public MoveSpeedChanged OnMoveSpeedChanged = () => { };

        public void AddEffect(CreatureEffect creatureEffect)
        {
            effects.Add(creatureEffect);

            if (creatureEffect.moveSpeed != 1.0f)
                OnMoveSpeedChanged.Invoke();
        }

        public void RemoveEffect(CreatureEffect creatureEffect)
        {
            effects.Remove(creatureEffect);

            if (creatureEffect.moveSpeed != 1.0f)
                OnMoveSpeedChanged.Invoke();
        }

        public bool IsImmuneToStatusCondition(AllStatusConditions.StatusConditionName statusCondition)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                if (effects[i].immuneStatusConditions.Contains(statusCondition))
                    return true;
            }

            return false;
        }

        public float GetDamageMultiplierByStrikeType(StrikeType strikeType)
        {
            float modifierAmount = 1.0f;

            effects.ToList().ForEach(effect => modifierAmount *= effect.DamageMultiplierByStrikeType[strikeType]);

            return modifierAmount;
        }

        public float GetKnockbackMultiplierByStrikeType(StrikeType strikeType)
        {
            float modifierAmount = 1.0f;

            effects.ToList().ForEach(effect => modifierAmount *= effect.KnockbackMultiplierByStrikeType[strikeType]);

            return modifierAmount;
        }

        public float GetOverwhelmFlatModifer()
        {
            return effects.Sum(effect => effect.overwhelmFlatModifier);
        }

        public float GetOverwhelmMultiplier()
        {
            return effects.Sum(effect => effect.overwhelmMultiplier);
        }
    }
}