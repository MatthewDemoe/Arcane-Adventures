using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Navigation;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellReference : MonoBehaviour
    {
        [Header("Spell Effects")]
        public UnityEvent OnBeginCast = new UnityEvent();
        public UnityEvent OnChannel = new UnityEvent();
        public UnityEvent OnUpkeep = new UnityEvent();
        public UnityEvent OnEndCast = new UnityEvent();
        public UnityEvent OnDurationEnd = new UnityEvent();

        public PhysicalSpell physicalSpell = null;
        public Spell spell = null;

        [SerializeField]
        bool useAsAudioSource = true;

        SpellAudioSourcePoolPlayer spellAudioSourceCreator;
        List<Animator> animators;

        private const float AnimationEndTime = 0.95f;

        public void Initialize(PhysicalSpell physicalSpell)
        {
            this.physicalSpell = physicalSpell;

            spell = physicalSpell.correspondingSpell;

            InitializeScale();

            GetComponentsInChildren<ISpellReferencer>().ToList().ForEach(spellReferencer => spellReferencer.physicalSpell = physicalSpell);

            PhysicalSpellDurationTracker durationTracker = GetComponentInChildren<PhysicalSpellDurationTracker>();
            if (durationTracker != null)
                durationTracker.StartDuration(physicalSpell);

            if (TryGetComponent(out NavMeshModifierInitializer navMeshModifierInitializer))
            {
                if (!navMeshModifierInitializer.shouldPlayOnAwake)
                    navMeshModifierInitializer.InitializeArea();
            }

            DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(gameObject, physicalSpell.spellCaster.gameObject);

            for (int i = 0; i < transform.childCount; i++)
            {
                DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(transform.GetChild(i).gameObject, physicalSpell.spellCaster.gameObject);
            }

            physicalSpell.OnBeginCast.AddListener(OnBeginCast.Invoke);
            physicalSpell.OnChannel.AddListener(OnChannel.Invoke);
            physicalSpell.OnUpkeep.AddListener(OnUpkeep.Invoke);
            physicalSpell.OnEndCast.AddListener(OnEndCast.Invoke);
            physicalSpell.OnDurationEnd.AddListener(OnDurationEnd.Invoke);

            spellAudioSourceCreator = physicalSpell.GetComponentInChildren<SpellAudioSourcePoolPlayer>();
            animators = GetComponentsInChildren<Animator>().ToList();

            if (useAsAudioSource)
                spellAudioSourceCreator.targetAudioSourceParent = transform;
        }

        private void InitializeScale()
        {
            if (spell.radius <= 0.0f)
                return;

            transform.localScale = Vector3.one * spell.radius * 2.0f;
        }

        public void EndDuration()
        {
            physicalSpell.OnDurationEnd.Invoke();
        }

        public void DestroySpell()
        {
            StartCoroutine(DestroySpellRoutine());
        }

        IEnumerator DestroySpellRoutine()
        {
            //Wait for final sound to start playing
            yield return null;

            animators.RemoveAll(animator => !animator.gameObject.activeInHierarchy);

            yield return new WaitUntil(() => !physicalSpell.IsSpellActive());

            Destroy(gameObject);
        }
    }
}