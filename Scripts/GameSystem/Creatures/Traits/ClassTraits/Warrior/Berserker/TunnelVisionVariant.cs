using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class TunnelVisionVariant : Trait
    {
        CreatureEffect tunnelVisionEffect;
        Creature targetCreature = null;

        public override string name => "Tunnel Vision";
        public override string description => $"Consecutive attacks against one target grant you a stacking {CriticalChance}% damage buff to a maximum of {(MaximumCriticalChance - 1) * 100}% to the same target. Attacking a different creature immediately removes all prior stacks on the original target and begins stacking onto the new target.";

        const float CriticalChance = 0.025f;
        const float MaximumCriticalChance = 1.15f;

        float criticalHitChance = 1.0f;

        public static TunnelVisionVariant Instance { get; } = new TunnelVisionVariant();
        private TunnelVisionVariant() { }

        protected TunnelVisionVariant(Creature creature) : base(creature)
        {
            tunnelVisionEffect = new CreatureEffect(
                name: name + " Effect",
                description: "Increased Critical Hit chance against a single target",
                source: name,
                criticalHitChance: criticalHitChance
                );

            creature.OnWeaponHitBeforeReport += CheckAttackTarget;
        }

        public override Trait Get(Creature creature)
        {
            return new TunnelVisionVariant(creature);
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
            if (tunnelVisionEffect.criticalHitChance >= MaximumCriticalChance)
                return;

            criticalHitChance += CriticalChance;

            creature.modifiers.RemoveEffect(tunnelVisionEffect);

            tunnelVisionEffect = new CreatureEffect(
                name: name + " Effect",
                description: $"Increased Critical Hit chance by {(criticalHitChance - 1) * 100}% against a single target",
                source: name,
                criticalHitChance: criticalHitChance
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
