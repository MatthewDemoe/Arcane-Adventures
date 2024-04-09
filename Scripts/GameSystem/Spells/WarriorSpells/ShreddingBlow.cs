using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class ShreddingBlow : Spell
    {
        public static ShreddingBlow Instance { get; } = new ShreddingBlow();
        private ShreddingBlow() { caster = null; }
        private ShreddingBlow(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ShreddingBlow(ref _caster);
        }

        public override string name => "Shredding Blow";
        public override string effectDescription => "When you cast this spell with a slashing or Piercing weapon you empower your weapon to...";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Assault;
        public override float initialCost => 25.0f;

        public override float channelCost => 0.0f;
        protected override float baseDuration => 30.0f;
        public override bool hasDuration => true;

        public override float cooldown => 45.0f;
    }
}
