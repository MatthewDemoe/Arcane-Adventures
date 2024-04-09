using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(SpellReference))]
    public class DoEffectsAtInterval : MonoBehaviour
    {
        public UnityEvent OnDamageIntervalEffects = new UnityEvent();

        [FormerlySerializedAs("OnOtherIntervalEffects")]
        public UnityEvent OnChannelIntervalEffects = new UnityEvent();
        public UnityEvent OnUpkeepIntervalEffects = new UnityEvent();

        SpellReference spellReference;

        private const float damageInterval = 1.0f;

        float channelInterval;
        float upkeepInterval;

        float durationScale = 1.0f;
        float _timer = 0.0f;


        private void Start()
        {
            spellReference = GetComponent<SpellReference>();

            channelInterval = spellReference.spell.channelInterval;
            upkeepInterval = spellReference.spell.upkeepInterval;

            StartCoroutine(PerformEffectsRoutine());
        }

        IEnumerator PerformEffectsRoutine()
        {
            _timer = 0.0f;

            float damageTimer = 0.0f;
            float channelTimer = 0.0f;
            float upkeepTimer = 0.0f;

            while (_timer < spellReference.spell.duration * durationScale)
            {
                yield return null;

                _timer += Time.deltaTime;
                damageTimer += Time.deltaTime;
                channelTimer += Time.deltaTime;
                upkeepTimer += Time.deltaTime;

                if (damageTimer >= damageInterval)
                {
                    damageTimer -= damageInterval;
                    OnDamageIntervalEffects.Invoke();
                }

                if (channelTimer >= channelInterval)
                {
                    channelTimer -= channelInterval;
                    OnChannelIntervalEffects.Invoke();
                }

                if (upkeepTimer >= upkeepInterval)
                {
                    upkeepTimer -= upkeepInterval;
                    OnUpkeepIntervalEffects.Invoke();
                }
            }
        }

        public void ScaleDuration(float scale)
        {
            durationScale *= scale;
        }
    }
}