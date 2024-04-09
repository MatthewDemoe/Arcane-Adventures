using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class WeaponHit : Hit
    {
        private ICombatSystem combatSystem;

        public StrikeType strikeType { get; private set; }
        public WeaponSurface attackSurface { get; private set; }

        public WeaponHit(Creature attacker, Creature target, StrikeType strikeType, WeaponSurface attackSurface) : base(attacker, target)
        {
            this.strikeType = strikeType;
            this.attackSurface = attackSurface;
            combatSystem = InjectorContainer.Injector.GetInstance<ICombatSystem>();
        }

        public override void Report(int damage, int hpReduced)
        {
            base.Report(damage, hpReduced);

            hitSource?.OnWeaponHitReported(target, strikeType);

            if (hitSource?.modifiers.effects.Select((effect) => effect.lifeSteal).Product() > 1.0f)
                hitSource?.stats.AdjustHealth((int)(damage * (hitSource?.modifiers.effects.Select((effect) => effect.lifeSteal).Product() - 1.0f)));
        }

        public override void CalculateHealthChange()
        {
            float damage = attackSurface.damageByStrikeType[strikeType];
            damage += combatSystem.settings.damageByStrikeTypeByWeightCategory[attackSurface.parentWeapon.weightCategory][strikeType];

            UpdateDamageFearMultiplier();

            if (hitSource is Creature)
            {
                Random randomNumberGenerator = new Random();

                damage += hitSource.stats.baseStrength * combatSystem.settings.strengthModifierByStrikeType[strikeType];

                damage *= hitSource.modifiers.effects.Select((effect) => effect.weaponDamageDealt).Product()
                    * hitSource.modifiers.effects.Select((effect) => effect.trueDamageDealt).Product()
                    * hitSource.modifiers.GetDamageMultiplierByStrikeType(strikeType) 
                    * damageFearMultiplier
                    * (randomNumberGenerator.NextDouble() < (hitSource.modifiers.effects.Select((effect) => effect.criticalHitChance).Product() - 1.0f) ? 2.0f : 1.0f);
            }

            this.healthChange = (int)(Math.Floor(damage) * target.modifiers.effects.Select((effect) => effect.damageTaken).Product());
        }
    }
}
