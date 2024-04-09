using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class PredictedMovement : Spell
    {
        public static PredictedMovement Instance { get; } = new PredictedMovement();
        private PredictedMovement() : base() { }
        private PredictedMovement(ref Creature _caster) : base(ref _caster) { }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new PredictedMovement(ref _caster);
        }

        public override string name => "Predicted Movement";
        public override string effectDescription => "Use Spirit to enhance your natural awareness, buffing you against an enemy.";
        public override int spellLevel => 1;
        public override float initialCost => 0;

        public override SpellType spellType => SpellType.Control;
        public override float upkeepCost => 5.0f;
        public override bool hasUpkeep => true;
        public override float cooldown => 20;
        protected override float _range => 5.0f;
    }
}
