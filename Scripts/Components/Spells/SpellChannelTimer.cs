using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    [RequireComponent(typeof(PhysicalSpell))]
    public class SpellChannelTimer : MonoBehaviour, ISpellReferencer
    {
        Creature caster;

        [SerializeField]
        bool scaleWithTime = true;

        [SerializeField]
        bool giveHapticFeedback = true;

        [SerializeField]
        bool shouldSlowCaster = true;

        float initialScale = 1.0f;

        const float hapticFeedbackDuration = 0.1f;
        const float hapticFeedbackAmplitude = 0.25f;

        CreatureEffect slowedEffect;

        string slowedSource => $"Channeling {physicalSpell.handSide} Spell";

        public UnityEvent OnCastSucceeded = new UnityEvent();
        public UnityEvent OnCastFailed = new UnityEvent();

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

                physicalSpell.OnBeginCast.AddListener(() => InitializeSpellChannelTimer());

                if(scaleWithTime)
                    physicalSpell.OnBeginCast.AddListener(() => StartCoroutine(ScaleTransformInChannelInterval()));

                physicalSpell.OnEndCast.AddListener(() => CompleteChannel());
            }
        }

        void InitializeSpellChannelTimer()
        {
            caster = physicalSpell.correspondingSpell.GetCaster();

            initialScale = transform.localScale.magnitude;

            if (scaleWithTime)
                transform.localScale = Vector3.zero;

            slowedEffect = new CreatureEffect
            (
                name: "Channel Slow",
                description: "Movement speed reduced while channeling",
                source: slowedSource,
                moveSpeed: 0.5f
            );

            if(shouldSlowCaster)
                caster.modifiers.AddEffect(slowedEffect);
        }

        void CompleteChannel()
        {
            if (!physicalSpell.correspondingSpell.isChanneledFully)
                OnCastFailed.Invoke();

            else
                OnCastSucceeded.Invoke();

            RemoveSlowed(); 
        }

        private void RemoveSlowed()
        {
            if(shouldSlowCaster)
                caster.modifiers.RemoveEffect(slowedEffect);
        }

        private IEnumerator ScaleTransformInChannelInterval()
        {
            float startTime = Time.time;

            while (!physicalSpell.correspondingSpell.isChanneledFully)
            {
                transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * initialScale, UtilMath.Lmap(Time.time - startTime, 0.0f, physicalSpell.correspondingSpell.channelDuration, 0.0f, 1.0f));

                yield return null;
            }

            if(giveHapticFeedback)
                Controllers.SendHapticImpulse(physicalSpell.handSide, hapticFeedbackAmplitude, hapticFeedbackDuration);
        }

        private void OnDestroy()
        {
            RemoveSlowed();
        }
    }
}