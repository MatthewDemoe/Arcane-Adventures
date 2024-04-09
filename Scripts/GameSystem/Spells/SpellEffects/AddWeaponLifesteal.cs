using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class AddWeaponLifesteal : SpellEffect
    {
        float lifestealAmount = 0.0f;

        CreatureEffect creatureEffect;

        public AddWeaponLifesteal(ref Creature caster, float lifestealAmount = 0.0f) : base(ref caster)
        {            
            this.lifestealAmount = lifestealAmount;        
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            creatureEffect = new CreatureEffect(
                name: "Weapon Lifesteal Buff",
                description: $"Add {lifestealAmount} percent lifesteal to weapon attacks.",
                source: spell.name, 
                lifeSteal: lifestealAmount
                );

            caster.modifiers.AddEffect(creatureEffect);
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.modifiers.RemoveEffect(creatureEffect);
        }
    }
}