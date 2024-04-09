using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class TunnelVision : Trait
    {
        CreatureEffect tunnelVisionEffect;
        Creature targetCreature = null;
        public override string name => "Tunnel Vision";
        public override string description => $"Consecutive attacks against one target grant you a stacking {DamageIncrease}% damage buff to a maximum of {(MaximumDamageIncrease - 1) * 100}% to the same target. Attacking a different creature immediately removes all prior stacks on the original target and begins stacking onto the new target.";

        const float DamageIncrease = 0.025f;
        const float MaximumDamageIncrease = 1.15f;

        float weaponDamageDealt = 1.0f;

        public static TunnelVision Instance { get; } = new TunnelVision();
        private TunnelVision() { }

        protected TunnelVision(Creature creature) : base(creature)
        {
            tunnelVisionEffect = new CreatureEffect(
                name: name + " Effect",
                description: $"Increased Weapon Damage by {(weaponDamageDealt - 1) * 100}% to single target",
                source: name,
                weaponDamageDealt: weaponDamageDealt
                );

            creature.OnWeaponHitBeforeReport += CheckAttackTarget;
        }

        public override Trait Get(Creature creature)
        {
            return new TunnelVision(creature);
        }

        private void CheckAttackTarget(Creature target)
        {
            if (targetCreature == null)
                targetCreature = target;

            if (targetCreature != target)
            {
                creature.modifiers.RemoveEffect(tunnelVisionEffect);
                return;
            }

            UpdateCreatureEffect();
        }

        private void UpdateCreatureEffect()
        {
            if (tunnelVisionEffect.weaponDamageDealt >= MaximumDamageIncrease)
                return;

            weaponDamageDealt += DamageIncrease;

            creature.modifiers.RemoveEffect(tunnelVisionEffect);

            tunnelVisionEffect = new CreatureEffect(
                name: name + " Effect",
                description: $"Increased Weapon Damage by {(weaponDamageDealt - 1) * 100}% to single target",
                source: name,
                weaponDamageDealt: weaponDamageDealt
                );

            creature.modifiers.AddEffect(tunnelVisionEffect);
        }

        public override void Disable()
        {
            base.Disable();

            creature.OnWeaponHitBeforeReport -= CheckAttackTarget;
            creature.modifiers.RemoveEffect(tunnelVisionEffect);
        }
    }
}
