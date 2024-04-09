using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PersistentSpellEffectStatContest : StatContestEventInvoker, ISpellReferencer
    {
        private Creature _targetCreature;
        private Creature _initiatingCreature;

        [SerializeField]
        bool performOnStart = false;

        private PhysicalSpell _physicalSpell;

        public PhysicalSpell physicalSpell
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;
                soundPlayer = _physicalSpell.GetComponentInChildren<AudioSourcePoolPlayer>();
            }
        }

        protected void Start()
        {
            if (performOnStart)
                PerformStatContest();
        }

        public void PerformStatContest()
        {
            CreatureReference targetCreatureReference = GetComponentInParent<CreatureReference>();
            _targetCreature = targetCreatureReference.creature;

            _initiatingCreature = GetComponentInParent<SpellReference>().spell.GetCaster();

            bool contestResult = StatContest.PerformStatContest(_targetCreature, _initiatingCreature, targetStat, initiatorStat);

            if (contestResult)
            {
                gameObjectStatContestEvents.OnSuccess.Invoke(targetCreatureReference.gameObject);
                creatureStatContestEvents.OnSuccess.Invoke(targetCreatureReference);
            }

            else
            {
                gameObjectStatContestEvents.OnFailure.Invoke(targetCreatureReference.gameObject);
                creatureStatContestEvents.OnFailure.Invoke(targetCreatureReference);
            }
        }
    }
}
