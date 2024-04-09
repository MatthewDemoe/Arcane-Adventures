using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Player;
using TheKiwiCoder;
using UnityEngine;
using UnityEngine.AI;
using NaughtyAttributes;
using System.Collections.Generic;
using System.Collections;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    public abstract class CreatureBehaviour : MonoBehaviour
    {
        public enum Role
        {
            NotAssigned,
            Resting,
            Patrol,
            PrimaryAttacker,
            SupportAttacker,
            PrimaryRangedAttacker,
            RangedSupportAttacker,
            MidrangeSupport,
        }

        public enum Team
        {
            Neutral,
            Ally,
            Enemy
        }

        public virtual float closeRangeDistance { get; } = 2.5f;
        public float maximumCloseRangeDistance => closeRangeDistance / 2.0f;

        public virtual List<Weapon> useableWeapons { get; protected set; } = new List<Weapon>() { };

        public CreatureReference creatureReference { get; protected set; }
        protected Rigidbody rigidBody;
        protected Animator animator;
        protected AnimationEventInvoker animationEventInvoker;

        protected BehaviourTreeRunner behaviourTreeRunner;
        public MobCoordinator mob;

        public Transform target { get; protected set; }
        protected Transform previousTarget { get; set; } = null;

        protected Vector3 lookAtCurrent = Vector3.zero;
        protected Vector3 lookAtOffset = Vector3.zero;
        public Vector3 GetTargetPosition => target.GetComponentInChildren<SpellEffectAnchor>().transform.position;
        public Vector3 GetMidpoint => GetComponentInChildren<SpellEffectAnchor>().transform.position;

        public float timeOfLastAttack = 0.0f;

        protected float _timeBetweenAttacks = 3.0f;        
        public float timeBetweenAttacksMultiplier = 1.0f;

        const float TimeBetweenLookAtTargetSwap = 5.0f;

        protected float timeBetweenAttacks => _timeBetweenAttacks * timeBetweenAttacksMultiplier * (role == Role.PrimaryAttacker ? 1.0f : 2.0f);

        protected NavMeshAgent navMeshAgent;

        protected bool speedChanged = false;
        protected bool isLookingAtTarget = true;

        protected virtual bool shouldUpdateMovementParameters => true;

        [SerializeField][ReadOnly]
        private Role _role = Role.NotAssigned;

        public Role role
        {
            get { return _role; }

            set
            {
                _role = value;
                SetBehaviourTree();
            }
        }

        public abstract List<Role> alternativeRoles { get; }
        public abstract Role preferredRole { get; }

        public virtual Team team { get; protected set; } = Team.Neutral;

        bool hasCombatRole => role == Role.PrimaryAttacker || role == Role.SupportAttacker;

        public abstract bool isAttacking { get; }

        protected virtual void Start()
        {
            if (TryGetComponent<NavMeshAgent>(out var navMeshAgent))
                this.navMeshAgent = navMeshAgent;

            creatureReference = GetComponent<CreatureReference>();
            lookAtCurrent = transform.forward;
            Initialize(creatureReference);

            StartCoroutine(RoamingGazeRoutine());
        }

        private void Awake()
        {
            InjectorContainer.Injector.GetInstance<CreatureTracker>().TrackCreature(this);

            behaviourTreeRunner = GetComponent<BehaviourTreeRunner>();
        }

        protected virtual void Initialize(CreatureReference creatureReference)
        {
            this.creatureReference = creatureReference;
            timeOfLastAttack = Time.time;

            animator = GetComponentInChildren<Animator>();
            rigidBody = creatureReference.gameObject.GetComponent<Rigidbody>();

            creatureReference.creature.modifiers.OnMoveSpeedChanged += AdjustMoveSpeed;
        }

        protected virtual void Update()
        {
            UpdateAnimatorParameters();
        }

        public void SetTarget(Transform target)
        {
            previousTarget = this.target;
            this.target = target;
        }

        public virtual void ResetTarget()
        {
            if (previousTarget == null)
                return;

            target = previousTarget;
            previousTarget = null;
        }

        private void UpdateAnimatorParameters()
        {
            if (navMeshAgent is null)
                return;

            if (speedChanged)
            {
                navMeshAgent.speed = creatureReference.creature.moveSpeed;
                speedChanged = false;
            }

            UpdateMovementAnimatorParameters();

            animator.SetBool(CharacterAnimatorParameters.Restrained, creatureReference.creature.statusConditionTracker.HasStatusCondition(AllStatusConditions.StatusConditionName.Restrained));          
        }

        protected virtual void UpdateMovementAnimatorParameters()
        {
            if (!shouldUpdateMovementParameters)
                return;

            var lowerBodyLayerWeight = (navMeshAgent.velocity.magnitude > 0.1) && creatureReference.creature.isMovementEnabled ? 1 : 0;
            animator.SetLayerWeight(CharacterAnimationLayers.LowerBodyLayer, lowerBodyLayerWeight);
            var normalizedVelocity = creatureReference.creature.isDead ? Vector3.zero : creatureReference.transform.InverseTransformDirection(navMeshAgent.velocity).normalized;

            normalizedVelocity *= lowerBodyLayerWeight;

            animator.SetFloat(CharacterAnimatorParameters.XVelocity, normalizedVelocity.x, 0.35f, Time.deltaTime);
            animator.SetFloat(CharacterAnimatorParameters.YVelocity, normalizedVelocity.z, 0.35f, Time.deltaTime);
        }

        public abstract bool Attack();

        public virtual void CloseGap() { }

        public virtual void FinishAttack()
        {
            timeOfLastAttack = Time.time;
        }

        public virtual void PickUpItem(PhysicalWeapon physicalWeapon)
        {
            animationEventInvoker.OnAnimationEnd.AddListener(() => creatureReference.GetComponent<ItemEquipper.ItemEquipper>().EquipItem(HandSide.Right, physicalWeapon));
            animationEventInvoker.OnAnimationEnd.AddListener(() => animationEventInvoker.OnAnimationEnd.RemoveAllListeners());
        }

        private void SetBehaviourTree()
        {
            behaviourTreeRunner.SwapTrees(BehaviourTreeCache.GetBehaviourTree(role.ToString()));

            if (hasCombatRole)
                return;

            if(!gameObject.TryGetComponent(out EnemyAlertTrigger _))
                gameObject.AddComponent<EnemyAlertTrigger>();
        }

        private void AdjustMoveSpeed()
        {
            speedChanged = true;
        }

        public void UpdateLookPosition()
        {
            if (animator == null)
                return;

            animator.SetLookAtPosition(lookAtCurrent);
            animator.SetLookAtWeight(1.0f);
        }

        private void ChooseNewLookTarget()
        {
            if (isLookingAtTarget)
            {
                lookAtOffset = Random.insideUnitSphere * 5.0f;
                lookAtOffset.y = 0;             
            }

            else
                lookAtOffset = Vector3.zero;

            isLookingAtTarget = !isLookingAtTarget;
        }

        private void OnDestroy()
        {
            InjectorContainer.Injector.GetInstance<CreatureTracker>().StopTrackingCreature(this);
        }

        IEnumerator RoamingGazeRoutine()
        {
            float timer = 0.0f;

            while (!creatureReference.creature.isDead)
            {
                yield return null;

                timer += Time.deltaTime;

                if (timer >= (isLookingAtTarget ? TimeBetweenLookAtTargetSwap : TimeBetweenLookAtTargetSwap / 2.0f))
                {
                    ChooseNewLookTarget();
                    timer -= TimeBetweenLookAtTargetSwap;
                }

                if (target != null)
                    lookAtCurrent = Vector3.MoveTowards(lookAtCurrent, isLookingAtTarget ? GetTargetPosition : GetTargetPosition + lookAtOffset, 0.1f);
            }
        }        
    }
}