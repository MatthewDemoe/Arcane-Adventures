using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using UnityEngine.Events;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class WeaponHitStatContest : MonoBehaviour
    {
        private Creature initiatingCreature;

        [SerializeField]
        protected Stats.Stat targetStat;

        [SerializeField]
        protected Stats.Stat initiatorStat;

        public UnityEvent<Creature> OnSuccess = new UnityEvent<Creature>();
        public UnityEvent<Creature> OnFailure = new UnityEvent<Creature>();

        private void Start()
        {
            initiatingCreature = GetComponentInParent<PhysicalSpell>().correspondingSpell.GetCaster();

            OnSuccess.AddListener((creature) => Instantiate(Prefabs.Spells.Load(Prefabs.Spells.SuccessBurst), gameObject.transform.position, gameObject.transform.rotation));
            OnSuccess.AddListener((creature) => Instantiate(SoundPlayer.Instance.CreateSoundObject(SoundPlayer.AudioClipNames.Success, gameObject.transform)));

            OnFailure.AddListener((creature) => Instantiate(Prefabs.Spells.Load(Prefabs.Spells.FailureBurst), gameObject.transform.position, gameObject.transform.rotation));
            OnFailure.AddListener((creature) => Instantiate(SoundPlayer.Instance.CreateSoundObject(SoundPlayer.AudioClipNames.Failure, gameObject.transform)));
        }

        public void PerformStatContest(Creature hitCreature)
        {

            bool contestResult = StatContest.PerformStatContest(hitCreature, initiatingCreature, targetStat, initiatorStat);

            if (contestResult)
                OnSuccess.Invoke(hitCreature);

            else
                OnFailure.Invoke(hitCreature);
        }
    }
}
