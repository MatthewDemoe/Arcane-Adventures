using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WarriorSpells
{
    public class UnrelentingForce : Spell
    {
        public static UnrelentingForce Instance { get; } = new UnrelentingForce();
        private UnrelentingForce() { caster = null; }
        private UnrelentingForce(ref Creature _caster) : base(ref _caster)
        {
            spellEffects = new List<SpellEffect>()
            {
                new AddCreatureEffectToCaster( ref caster, 
                new CreatureEffect(
                    name: name, 
                    description: effectDescription, 
                    source: name, 
                    moveSpeed: MoveSpeedBuff
                    )),
                new AdjustContestedCheckBonus(ref caster, 3),
            };
        }

        public override Spell CreateSpell(ref Creature _caster)
        {
            return new UnrelentingForce(ref _caster);
        }

        public override string name => "Unrelenting Force";
        public override string effectDescription => "Spirit flows through you, enhancing your natural strength and abilities. Gain movement speed, overwhelm chance, and a bonus to resist contested checks.";
        public override int spellLevel => 1;

        public override SpellType spellType => SpellType.Enhancement;
        public override float initialCost => 0;
        public override float upkeepCost => 5;
        public override bool hasUpkeep => true;

        public override float cooldown => 20;

        private const float MoveSpeedBuff = 1.15f;
    }
}
