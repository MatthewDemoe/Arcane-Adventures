using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class TemporaryStatBuff : SpellEffect
    {
        private Stats.Stat _statToBuff;
        private int _amount;

        public TemporaryStatBuff(ref Creature _caster, Stats.Stat statToBuff, int amount) : base(ref _caster)
        {
            _statToBuff = statToBuff;
            _amount = amount;
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            caster.stats.AdjustStatByName(_statToBuff, _amount);
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();
            caster.stats.AdjustStatByName(_statToBuff, -_amount);
        }
    }
}
