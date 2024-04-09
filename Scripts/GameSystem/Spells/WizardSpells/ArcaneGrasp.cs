using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class ArcaneGrasp : Spell
    {
        public static ArcaneGrasp Instance { get; } = new ArcaneGrasp();
        private ArcaneGrasp() : base(){ }
        private ArcaneGrasp(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new ArcaneGrasp(ref _caster);
        }

        public override string name => "Arcane Grasp";
        public override string effectDescription => "Use the force, Luke! Envelop one enemy in spirit and crush their body, immobilizing them and dealing damage over time.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 10.0f;

        public override float channelCost => 2.5f;
        public override float channelInterval => 5.0f;
        protected override float baseDuration => 30;
        public override bool hasDuration => true;

        public override float cooldown => 20;
        protected override float _range => 5.0f;

        protected override Damage _damage => new Damage(caster.stats.subtotalSpirit * baseDuration, Combat.DamageType.Bludgeoning);
        protected override bool scaleDamageOverDuration => true;
    }
}
