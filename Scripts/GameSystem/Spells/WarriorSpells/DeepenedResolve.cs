using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class DeepenedResolve : Spell
    {
        public static DeepenedResolve Instance { get; } = new DeepenedResolve();
        private DeepenedResolve() { caster = null; }
        private DeepenedResolve(ref Creature _caster) : base(ref _caster)
        {
            spellEffects = new List<SpellEffect>()
            {
                new AddCreatureEffectToCasterWithUpdateWhenDamageTaken(ref caster, 
                () => new CreatureEffect(
                    name: name, 
                    description: effectDescription, 
                    source: name,
                    trueDamageDealt: 1.0f + ((1.0f - caster.stats.currentHealthPercent) * MaxBuffAmount),
                    damageTaken: 1.0f - ((1.0f - caster.stats.currentHealthPercent) * MaxBuffAmount)
                    ))
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new DeepenedResolve(ref _caster);
        }

        public override string name => "Deepened Resolve";
        public override string effectDescription => "The closer you come to death, the more your resolve deepens. Gain bonuses to damage and damage reduction as your health decreases.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Control;
        public override float initialCost => 0;
        public override float upkeepCost => 8;
        public override bool hasUpkeep => true;

        public override float cooldown => 10;

        private const float MaxBuffAmount = 0.5f;
    }
}
