using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public abstract class CreatureReference : MonoBehaviour
    {
        public UnityEvent OnDeath = new UnityEvent();
        public UnityEvent<PhysicalWeapon> OnWeaponClash = new UnityEvent<PhysicalWeapon>();

        private Creature _creature;
        private ResourceBar healthBar;
        private ResourceBar spiritBar;

        protected abstract Type creatureType { get; }
        protected Animator animator;

        public Creature creature 
        { 
            get 
            {
                if (_creature == null)
                {
                    SetCreature(DefaultCreatureResolver.GetDefaultCreature(creatureType));
                }

                return _creature; 
            }
            set { SetCreature(value); } 
        }

        public virtual bool isAttacking => false;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            OnDeath.AddListener(() => animator.SetBool(CharacterAnimatorParameters.IsDead, true));
            OnWeaponClash.AddListener(ProcessWeaponClash);
        }

        protected virtual void Start()
        {
            if (!typeof(Creature).IsAssignableFrom(creatureType))
            {
                throw new Exception($"{nameof(creatureType)} is not assignable from {nameof(Creature)}.");
            }

            #if UNITY_EDITOR
            if (creature == null)
            {
                creature = DefaultCreatureResolver.GetDefaultCreature(creatureType);
            }
            #endif

            var resourceBars = GetComponentsInChildren<ResourceBar>();

            resourceBars.ToList().ForEach((resourceBar) =>
            {
                if (resourceBar.trackedResource == Resource.Health)
                    healthBar = resourceBar;
                else
                    spiritBar = resourceBar;
            });
        }

        public virtual void ProcessDamage()
        {
            healthBar.ProcessResourceChange();

            if (creature.isDead)
                OnDeath.Invoke();
        }

        public virtual void ProcessSpiritChange()
        {
            if (spiritBar is null)
                return;

            spiritBar.ProcessResourceChange();
        }

        private void SetCreature(Creature creatureToSet)
        {
            ValidateCreatureType(creatureToSet.GetType());
            _creature = creatureToSet;

            creature.OnDamageTaken = ProcessDamage;
            creature.OnSpiritChange = ProcessSpiritChange;

            StartCoroutine(RegenerateSpiritRoutine());
        }
        
        private void ValidateCreatureType(Type type)
        {
            if (!creatureType.IsAssignableFrom(type))
            {
                throw new Exception($"Creature validation failed: {type.Name} is not {creatureType.Name}.");
            }
        }

        public bool HasStatusCondition(AllStatusConditions.StatusConditionName statusCondition)
        {
            return creature.statusConditionTracker.HasStatusCondition(statusCondition);
        }

        public bool HasOneOfStatusCondition(List<AllStatusConditions.StatusConditionName> statusConditionList)
        {
            return creature.statusConditionTracker.HasOneOfStatusConditions(statusConditionList);
        }

        protected virtual void ProcessWeaponClash(PhysicalWeapon physicalWeapon)
        {
            if (isAttacking)
            {
                physicalWeapon.SetManualStrikeType(StrikeType.NotStrike);
            }
        }

        private IEnumerator RegenerateSpiritRoutine()
        {
            while (!creature.isDead)
            {
                yield return new WaitForSeconds(Creature.SpiritRegenerationInterval);

                creature.TryRegenerateSpirit();
            }
        }
    }
}