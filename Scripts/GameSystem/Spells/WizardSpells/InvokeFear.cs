using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells
{
    public class InvokeFear : Spell
    {
        public static InvokeFear Instance { get; } = new InvokeFear();
        private InvokeFear() : base() { }
        private InvokeFear(ref Creature _caster) : base(ref _caster) { }


        public override Spell CreateSpell(ref Creature _caster)
        {
            return new InvokeFear(ref _caster);
        }

        public override string name => "Invoke Fear";
        public override string effectDescription => "Waves of corruption emanate from you, instilling fear in the minds of enemies around you.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 0;

        public override float upkeepCost => 5.0f;
        public override float upkeepInterval => 5.0f;

        protected override float _radius => 2;

        public override bool hasUpkeep => true;

        public override float cooldown => 15.0f;

        public override bool isSelfTargeted => true;

    }
}
