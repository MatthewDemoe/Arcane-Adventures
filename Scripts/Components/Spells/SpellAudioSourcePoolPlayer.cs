using com.AlteredRealityLabs.ArcaneAdventures.Audio;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using UnityEngine;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellAudioSourcePoolPlayer : AudioSourcePoolPlayer, ISpellReferencer
    {
        [SerializeField]
        SpellAudioClipSettings awakeAudioClip;

        [SerializeField]
        SpellAudioClipSettings beginCastAudioClip;

        [SerializeField]
        SpellAudioClipSettings channelEffectAudioClip;

        [SerializeField]
        SpellAudioClipSettings upkeepAudioClip;

        [SerializeField]
        SpellAudioClipSettings endCastAudioClip;

        [SerializeField]
        SpellAudioClipSettings durationEndAudioClip;

        [SerializeField]
        SpellAudioClipSettings collisionAudioClip;

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
                InitializePhysicalSpellEvents();
            }
        }

        public Transform targetAudioSourceParent { get; set; }

        protected override void Awake()
        {
            targetAudioSourceParent = audioSourcePoolParent.transform;

            base.Awake();
            PlayAudioClip(awakeAudioClip);
        }

        public void PlayAudioClip(SpellAudioClipSettings spellAudioClipSettings)
        {
            AudioSource initialAudioSource = GetFirstAvailableAudioSource();

            bool hasLeadingAudioClip = spellAudioClipSettings.leadingAudioClip.audioClip != null;

            PlayAudioClip(hasLeadingAudioClip ? spellAudioClipSettings.leadingAudioClip: spellAudioClipSettings.mainAudioClip, targetAudioSourceParent);

            if(hasLeadingAudioClip)
                StartCoroutine(QueueMainAudioClipRoutine(spellAudioClipSettings, initialAudioSource));
        }

        private void InitializePhysicalSpellEvents()
        {
            if (!beginCastAudioClip.isNull)
                physicalSpell.OnBeginCast.AddListener(() => PlayAudioClip(beginCastAudioClip));

            if(!channelEffectAudioClip.isNull)
                physicalSpell.OnChannel.AddListener(() => PlayAudioClip(channelEffectAudioClip));

            if(!upkeepAudioClip.isNull)
                physicalSpell.OnUpkeep.AddListener(() => PlayAudioClip(upkeepAudioClip));

            if(!endCastAudioClip.isNull)
                physicalSpell.OnEndCast.AddListener(() => PlayAudioClip(endCastAudioClip));

            if (!durationEndAudioClip.isNull)
            {
                physicalSpell.OnDurationEnd.AddListener(() => PlayAudioClip(durationEndAudioClip));
            }

            if (!collisionAudioClip.isNull)
                physicalSpell.OnCollision.AddListener(() => PlayAudioClip(collisionAudioClip));
        }        

        IEnumerator QueueMainAudioClipRoutine(SpellAudioClipSettings spellAudioClipSettings, AudioSource initialAudioSource)
        {
            yield return null;

            if (!spellAudioClipSettings.overlapping)
                yield return new WaitUntil(() => !initialAudioSource.isPlaying);

            PlayAudioClip(spellAudioClipSettings.mainAudioClip, targetAudioSourceParent);
        }        
    }
}