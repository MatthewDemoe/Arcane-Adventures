using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class AddStatusConditionToCaster : SpellEffect
    {
        StatusCondition statusCondition;

        public AddStatusConditionToCaster(ref Creature caster, StatusCondition statusCondition) : base(ref caster)
        {
            this.statusCondition = statusCondition;
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            caster.statusConditionTracker.AddStatusCondition(statusCondition);
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.statusConditionTracker.RemoveStatusCondition(statusCondition.statusConditionName);
        }
    }
}