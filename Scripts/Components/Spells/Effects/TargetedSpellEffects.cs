using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine.Events;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class TargetedSpellEffects : MonoBehaviour, ISpellReferencer
    {
        public UnityEvent<Creature> OnBeginCast = new UnityEvent<Creature>();
        public UnityEvent<Creature> OnChannel = new UnityEvent<Creature>();
        public UnityEvent<Creature> OnUpkeep = new UnityEvent<Creature>();
        public UnityEvent<Creature> OnEndCast = new UnityEvent<Creature>();
        public UnityEvent<Creature> OnDurationEnd = new UnityEvent<Creature>();

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

                physicalSpell.OnBeginCast.AddListener(() =>
                {
                    if (spellTargeter.targetedCreatureReference == null)
                        return;

                    targetCreature = spellTargeter.targetedCreatureReference.creature;
                    OnBeginCast.Invoke(targetCreature);
                });

                physicalSpell.OnChannel.AddListener(() => OnChannel.Invoke(targetCreature));
                physicalSpell.OnUpkeep.AddListener(() => OnUpkeep.Invoke(targetCreature));
                physicalSpell.OnEndCast.AddListener(() => OnEndCast.Invoke(targetCreature));
                physicalSpell.OnDurationEnd.AddListener(() => OnDurationEnd.Invoke(targetCreature));
            }
        }

        SpellSingleTargeter spellTargeter => physicalSpell.spellSource.GetComponent<SpellSingleTargeter>();

        Creature targetCreature;
    }
}