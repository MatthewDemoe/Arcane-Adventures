using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells
{
    public class Stomp : Spell
    {
        public static Stomp Instance { get; } = new Stomp();
        private Stomp() : base() { }
        private Stomp(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new Stomp(ref _caster);
        }

        public override string name => "Stomp";
        public override string effectDescription => "Stomp...";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 0.0f;
        protected override float _radius => 3;
        public override bool hasDuration => false;

        public override float cooldown => 15.0f;

        protected override Damage _damage => new Damage(43.0f, DamageType.Bludgeoning);
    }
}
