using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits
{
    public class UnstableEssence : Trait
    {
        Maddened maddenedCondition;

        float lastHitTime = 0;

        const float TraitCooldown = 15.0f;

        public override string name => "Unstable Essence";
        public override string description => "Every 15 seconds when a creature strikes you with a melee attack they must make a Spirit save or become maddened for 5 seconds.";

        public static UnstableEssence Instance { get; } = new UnstableEssence();
        private UnstableEssence() { }

        protected UnstableEssence(Creature creature) : base(creature)
        {
            creature.OnProcessHit += ForceCheckOnAttacker;
        }

        public override Trait Get(Creature creature)
        {
            return new UnstableEssence(creature);
        }

        private float ForceCheckOnAttacker(Hit hit)
        {
            if (Time.time - lastHitTime < TraitCooldown)
                return 0;

            bool passedSavingThrow = SavingThrow.PerformSavingThrow(hit.hitSource, Stats.Stat.Spirit);

            if (passedSavingThrow)
                return 0;

            lastHitTime = Time.time;

            Creature attackingCreature = hit.hitSource;
            maddenedCondition = new Maddened(attackingCreature, new MaddenedStatusSettings(durationInMilliseconds: 5000), name);
            hit.hitSource.statusConditionTracker.AddStatusCondition(maddenedCondition);

            return 0;
        }

        public override void Disable()
        {
            base.Disable();

            creature.OnProcessHit -= ForceCheckOnAttacker;
        }
    }
}
