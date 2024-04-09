using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class MeteorCrash : Spell
    {
        public static MeteorCrash Instance { get; } = new MeteorCrash();
        private MeteorCrash() { caster = null; }
        private MeteorCrash(ref Creature _caster) : base(ref _caster)
        {
        }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new MeteorCrash(ref _caster);
        }

        public override string name => "Meteor Crash";
        public override string effectDescription => "Jump to a target location, damaging and stunning enemies that you land on.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 70;

        public override float cooldown => 30;
        protected override float _range => 15.0f;
        protected override float _radius => 1.5f;

        protected override Damage _damage => new Damage(caster.stats.subtotalStrength  * 3.0f, DamageType.Bludgeoning);
    }
}
