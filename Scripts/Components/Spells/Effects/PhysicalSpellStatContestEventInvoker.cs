using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PhysicalSpellStatContestEventInvoker : StatContestEventInvoker
    {
        Creature caster;

        protected void Start()
        {
            caster = GetComponentInParent <PhysicalSpell>().correspondingSpell.GetCaster();
        }

        public void PerformStatContest(GameObject target)
        {
            CreatureReference targetCreature = target.GetComponentInParent<CreatureReference>();

            if (targetCreature == null)
                return;

            bool contestResult = StatContest.PerformStatContest(targetCreature.creature, caster, targetStat, initiatorStat);

            if (contestResult)
            {
                gameObjectStatContestEvents.OnSuccess.Invoke(target);
                creatureStatContestEvents.OnSuccess.Invoke(targetCreature);
            }

            else
            {
                gameObjectStatContestEvents.OnFailure.Invoke(target);
                creatureStatContestEvents.OnFailure.Invoke(targetCreature);
            }
        }

        public void PerformStatContest(CreatureReference target)
        {
            bool contestResult = StatContest.PerformStatContest(target.creature, caster, targetStat, initiatorStat);

            if (contestResult)
            {
                gameObjectStatContestEvents.OnSuccess.Invoke(target.gameObject);
                creatureStatContestEvents.OnSuccess.Invoke(target);
            }

            else
            {
                gameObjectStatContestEvents.OnFailure.Invoke(target.gameObject);
                creatureStatContestEvents.OnFailure.Invoke(target);
            }
        }
    }
}