using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells
{
    public class TreeSlam : Spell
    {
        public static TreeSlam Instance { get; } = new TreeSlam();
        private TreeSlam() : base() { }
        private TreeSlam(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new TreeSlam(ref _caster);
        }

        public override string name => "Tree Slam";
        public override string effectDescription => "Tree Slam...";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 0.0f;
        protected override float _radius => 3;
        public override bool hasDuration => false;

        public override float cooldown => 5.0f;

        public override float knockbackDistance { get; } = 1.5f;

        protected override Damage _damage => new Damage(43.0f, DamageType.Bludgeoning);
    }
}
