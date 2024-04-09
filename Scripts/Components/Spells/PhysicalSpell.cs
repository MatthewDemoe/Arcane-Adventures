using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    //TODO: Maybe create derived class for each spell that keeps a reference to the GameSystem spell
    //Would allow for initialization of duration, etc. to be done in their own scripts, and probably simplify other sections
    public class PhysicalSpell : MonoBehaviour
    {
        [Header("Spell Effects")]
        public UnityEvent OnBeginCast = new UnityEvent();
        public UnityEvent OnChannel = new UnityEvent();
        public UnityEvent OnUpkeep = new UnityEvent();
        public UnityEvent OnEndCast = new UnityEvent();
        public UnityEvent OnDurationEnd = new UnityEvent();
        public UnityEvent OnCollision = new UnityEvent();

        public virtual Type playerSpellTargeter { get; } = typeof(SpellTargeter);
        public virtual Type enemySpellTargeter { get; } = typeof(SpellTargeter);

        [HideInInspector]
        public Transform spellSource;

        [SerializeField]
        bool isSpellPhase = false;

        [SerializeField]
        bool forceImpactParticlesUpright = false;

        [SerializeField]
        GameObject impactParticles = null;

        [SerializeField]
        bool _destroyOnCollision = true;

        [SerializeField]
        bool attachToCaster = false;

        [SerializeField]
        public bool finishCastImmediately = false;

        public bool isAttachedToCaster => attachToCaster;

        public bool isChanneled => correspondingSpell.channelInterval > 0.0f;

        public Spell correspondingSpell { get; private set; } = null;
        public SpellCaster spellCaster { get; private set; } = null;

        public bool hasBeenCast { get; protected set; } = false;
        public bool hasBeenChanneled { get; protected set; } = false;

        public HandSide handSide { get; protected set; } = HandSide.None;

        SpellAudioSourcePoolPlayer spellAudioSourceCreator;
        List<Animator> animators;

        private const float AnimationEndTime = 0.95f;

        public virtual bool IsType(Type type) => GetType() == type;


        protected virtual void Awake()
        {
            OnBeginCast.AddListener(() => hasBeenCast = true);
            OnChannel.AddListener(() => hasBeenChanneled = true);

            PhysicalWeapon physicalWeapon = GetComponentInParent<PhysicalWeapon>();
            spellAudioSourceCreator = GetComponentInChildren<SpellAudioSourcePoolPlayer>();
            animators = GetComponentsInChildren<Animator>().ToList();

            DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(gameObject, physicalWeapon.wielder.gameObject);

            for (int i = 0; i < transform.childCount; i++)
            {
                DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(transform.GetChild(i).gameObject, physicalWeapon.wielder.gameObject);
            }
        }

        protected void OnTriggerEnter(Collider other)
        {
            if (other.transform.IsChildOf(transform))
                return;

            OnCollision.Invoke();

            if (impactParticles != null)
            {
                GameObject impactParticlesInstance = Instantiate(impactParticles, other.ClosestPointOnBounds(transform.position), transform.rotation);

                if (forceImpactParticlesUpright)
                    impactParticlesInstance.transform.up = Vector3.up;

                if (impactParticlesInstance.TryGetComponent(out SpellReference spellReference))
                    spellReference.Initialize(this);

                spellAudioSourceCreator.DetachAllSounds();
            }

            if (_destroyOnCollision)
                DestroySpell();
        }

        public virtual void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {          
            this.handSide = handSide;

            correspondingSpell = spell;
            spellSource = playerSpellCaster.SpellSourceTransform(handSide);
            spellCaster = playerSpellCaster;

            if (!isSpellPhase)
                GetComponentsInChildren<ISpellReferencer>().ToList().ForEach(spellReferencer => spellReferencer.physicalSpell = this);

            InitializeSpellCollider();
            InitializeSpellDuration();

            OnDurationEnd.AddListener(spell.DurationEnd);
            OnEndCast.AddListener(spell.EndCast);

            if (attachToCaster)
                transform.parent = spellCaster.GetComponentInChildren<SpellEffectAnchor>().transform;            
        }

        private void InitializeSpellDuration()
        {
            if (correspondingSpell.hasDuration || correspondingSpell.hasUpkeep)
            {
                if (!TryGetComponent(out PhysicalSpellDurationTracker durationTracker))
                    durationTracker = gameObject.AddComponent<PhysicalSpellDurationTracker>();

                durationTracker.StopAllCoroutines();
                OnEndCast.AddListener(() => durationTracker.StartDuration(this));
            }

            if (isChanneled && !TryGetComponent(out SpellChannelTimer channelTimer))
            {
                channelTimer = gameObject.AddComponent<SpellChannelTimer>();
            }
        }

        private void InitializeSpellCollider()
        {
            if (TryGetComponent(out SphereCollider sphereCollider))
            {
                if (correspondingSpell.radius > 0.0f)
                    sphereCollider.radius = correspondingSpell.radius;
            }

            else if (TryGetComponent(out CapsuleCollider capsuleCollider))
            {
                capsuleCollider.radius = correspondingSpell.radius;
                capsuleCollider.height = correspondingSpell.range;

                if (!correspondingSpell.isRangeCentered)
                    capsuleCollider.center = new Vector3(0.0f, 0.0f, correspondingSpell.range / 2.0f);

                ParticleSystem attachedParticleSystem = GetComponentInChildren<ParticleSystem>();
                if (attachedParticleSystem != null)
                {
                    var lifetimeMultiplier = attachedParticleSystem.main.startLifetimeMultiplier;
                    lifetimeMultiplier = 3;
                }
            }
        }

        public bool IsSpellActive()
        {
            return spellAudioSourceCreator.isPlaying || animators.Any(animator => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < AnimationEndTime);
        }

        public void DestroySpell()
        {
            StartCoroutine(DestroySpellRoutine());
        }

        IEnumerator DestroySpellRoutine()
        {
            //Wait for final sound to start playing
            yield return null;

            spellAudioSourceCreator.DetachAllSounds();

            animators.RemoveAll(animator => !animator.gameObject.activeInHierarchy);

            yield return new WaitUntil(() => !IsSpellActive());

            Destroy(gameObject);
        }
    }
}