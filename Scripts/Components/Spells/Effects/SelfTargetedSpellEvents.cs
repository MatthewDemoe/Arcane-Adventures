using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using UnityEngine.Events;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SelfTargetedSpellEvents : MonoBehaviour, ISpellReferencer
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

                Creature _caster = physicalSpell.correspondingSpell.GetCaster();

                physicalSpell.OnBeginCast.AddListener(() => OnBeginCast.Invoke(_caster));
                physicalSpell.OnChannel.AddListener(() => OnChannel.Invoke(_caster));
                physicalSpell.OnUpkeep.AddListener(() => OnUpkeep.Invoke(_caster));
                physicalSpell.OnEndCast.AddListener(() => OnEndCast.Invoke(_caster));
                physicalSpell.OnDurationEnd.AddListener(() => OnDurationEnd.Invoke(_caster));
            }
        }
    }
}