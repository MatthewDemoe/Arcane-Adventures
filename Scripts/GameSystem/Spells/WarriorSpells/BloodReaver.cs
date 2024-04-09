using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class BloodReaver : Spell
    {
        public static BloodReaver Instance { get; } = new BloodReaver();
        private BloodReaver() { caster = null; }
        private BloodReaver(ref Creature _caster)
        {
            caster = _caster;
            spellEffects = new List<SpellEffect>() 
            { 
                new AddCreatureEffectToCaster(ref caster, 
                    new CreatureEffect(
                    name: name,
                    description: effectDescription,
                    source: $"{name} spell",
                    lifeSteal: LifeStealAmount
                    ))
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new BloodReaver(ref _caster);
        }

        public override string name => "Blood Reaver";
        public override string effectDescription => "Blood fuels you in combat, giving you the strength to keep fighting. You gain lifesteal on your attacks, and killing enemies motivates you.";
        public override int spellLevel => 2;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 25;
        public override float knockbackDistance => 3.0f;

        public override bool hasUpkeep => true;
        public override float upkeepCost => 7.0f;

        public override float cooldown => 20;

        private const float LifeStealAmount = 1.25f;
    }
}
