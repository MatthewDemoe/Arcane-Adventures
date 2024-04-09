using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class MagicHit : Hit
    {
        List<Damage> spellDamage;

        public MagicHit(Creature attacker, Creature target, List<Damage> damage) : base(attacker, target)
        {
            spellDamage = damage;
        }

        public override void CalculateHealthChange()
        {
            int healthChange = 0;

            UpdateDamageFearMultiplier();

            foreach (Damage hitDamage in spellDamage)
            {
                healthChange += (int)(hitDamage.damage * target.modifiers.effects.Select((effect) => effect.damageTaken).Product()
                    * hitSource.modifiers.effects.Select((effect) => effect.spellDamageDealt).Product()
                    * hitSource.modifiers.effects.Select((effect) => effect.trueDamageDealt).Product());
            }

            //TODO: Determine appropriate handside
            base.healthChange = (int)(healthChange * ((hitSource as IItemWielder).rightHandItem as Weapon).spellDamageMultiplier) * (int)(healthChange < 0 ? 1.0f : damageFearMultiplier);
        }
    }
}
