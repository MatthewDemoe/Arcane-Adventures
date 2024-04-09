using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public class StatusConditionSpell : ProjectileSpell
    {
        public static StatusConditionSpell Instance { get; } = new StatusConditionSpell();
        private StatusConditionSpell() : base() { }
        private StatusConditionSpell(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new StatusConditionSpell(ref _caster);
        }

        public override string name => "Status Condition Spell";
        public override string effectDescription => "Add a status condtition to target creature.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 0;

        public override float channelCost => 0;
        protected override float baseDuration => 5;
        protected override float _radius => 1.0f;
        public override bool hasDuration => true;

        public override float cooldown => 1;

        protected override float _range => 30.0f;
        protected override float _force => CombatSettings.Spells.ProjectileForce;
        public override float knockbackDistance { get; } = 1.0f;
        protected override Damage _damage => new Damage(0f, Combat.DamageType.Magical);

        public AllStatusConditions.StatusConditionName statusCondition = AllStatusConditions.StatusConditionName.Hasted;
    }
}
