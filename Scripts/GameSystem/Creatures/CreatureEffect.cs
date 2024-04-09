using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures
{
    public class CreatureEffect : ITraceable
    {
        public string name { get; protected set; }
        public string description { get; protected set; }
        public string source { get; protected set; }

        public float overwhelmFlatModifier { get; protected set; }
        public float overwhelmMultiplier { get; protected set; }

        public float moveSpeed { get; protected set; }

        public float knockBackDealt { get; protected set; }
        public float knockBackTaken { get; protected set; }

        public float criticalHitChance { get; protected set; }
        public float lifeSteal { get; protected set; }

        public float weaponDamageDealt { get; protected set; }
        public float spellDamageDealt { get; protected set; }
        public float trueDamageDealt { get; protected set; }
        public float damageTaken { get; protected set; }

        public float statusConditionDurationModifier { get; protected set; }

        public int contestedCheckBonus { get ; protected set; } = 0;
        public int vitalityContestedCheckBonus { get; protected set; } = 0;
        public int spiritContestedCheckBonus { get; protected set; } = 0;
        public int strengthContestedCheckBonus { get; protected set; } = 0;

        public int traitStatPoints { get; protected set; } = 0;

        public Dictionary<StrikeType, float> DamageMultiplierByStrikeType { get; protected set; } = new Dictionary<StrikeType, float>()
        {
            { StrikeType.Imperfect, 1.0f },
            { StrikeType.Incomplete, 1.0f },
            { StrikeType.Perfect, 1.0f },
        };

        public Dictionary<StrikeType, float> KnockbackMultiplierByStrikeType { get; protected set; } = new Dictionary<StrikeType, float>()
        {
            { StrikeType.Imperfect, 1.0f },
            { StrikeType.Incomplete, 1.0f },
            { StrikeType.Perfect, 1.0f },
        };

        public List<AllStatusConditions.StatusConditionName> immuneStatusConditions { get; protected set; } = new List<AllStatusConditions.StatusConditionName>();


        public CreatureEffect(
            string name,
            string description,
            string source,
            float overwhelmFlatModifier = 0,
            float overwhelmMultiplier = 0,
            float moveSpeed = 1.0f,
            float knockBackDealt = 1.0f,
            float knockBackTaken = 1.0f,
            float criticalHitChance = 1.0f,
            float lifeSteal = 1.0f,
            float weaponDamageDealt = 1.0f,
            float spellDamageDealt = 1.0f,
            float trueDamageDealt = 1.0f,
            float damageTaken = 1.0f,
            float statusConditionDurationModifier = 1.0f,

            int contestedCheckBonus = 0,
            int vitalityContestedCheckBonus = 0,
            int spiritContestedCheckBonus = 0,
            int strengthContestedCheckBonus = 0,

            int traitStatPoints = 0,

            Dictionary<StrikeType, float> KnockbackMultiplierByStrikeType = null,
            Dictionary<StrikeType, float> DamageMultiplierByStrikeType = null,
            List<AllStatusConditions.StatusConditionName> immuneStatusConditions = null
            )

        {
            this.name = name;
            this.description = description;
            this.source = source;

            this.contestedCheckBonus = contestedCheckBonus;
            this.vitalityContestedCheckBonus = vitalityContestedCheckBonus;
            this.spiritContestedCheckBonus = spiritContestedCheckBonus;
            this.strengthContestedCheckBonus = strengthContestedCheckBonus;

            this.traitStatPoints = traitStatPoints;

            this.overwhelmFlatModifier = overwhelmFlatModifier;
            this.overwhelmMultiplier = overwhelmMultiplier;

            this.moveSpeed = moveSpeed;
            this.knockBackDealt = knockBackDealt;
            this.knockBackTaken = knockBackTaken;
            this.criticalHitChance = criticalHitChance;
            this.lifeSteal = lifeSteal;

            this.weaponDamageDealt = weaponDamageDealt;
            this.spellDamageDealt = spellDamageDealt;
            this.trueDamageDealt = trueDamageDealt;
            this.damageTaken = damageTaken;
            this.statusConditionDurationModifier = statusConditionDurationModifier;

            if(!(KnockbackMultiplierByStrikeType is null))
                KnockbackMultiplierByStrikeType.Keys.ToList().ForEach(key => this.KnockbackMultiplierByStrikeType[key] *= KnockbackMultiplierByStrikeType[key]);

            if(!(DamageMultiplierByStrikeType is null))
                DamageMultiplierByStrikeType.Keys.ToList().ForEach(key => this.DamageMultiplierByStrikeType[key] *= DamageMultiplierByStrikeType[key]);

            if (!(immuneStatusConditions is null))
                this.immuneStatusConditions = immuneStatusConditions;
        }
    }
}