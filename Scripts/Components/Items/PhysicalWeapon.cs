using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using static com.AlteredRealityLabs.ArcaneAdventures.Combat.StrikeCalculator;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;
using System.Collections.Generic;
using NaughtyAttributes;
using com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items
{
    public class PhysicalWeapon : PhysicalHandHeldItem
    {
        private const float StrikeTimeoutOnPickupAndDrop = 0.5f;

        [SerializeField] private GameObject strikeEffect;
        [SerializeField] [ShowIf(nameof(destroyOnImpact))] private GameObject destructionEffect;
        [SerializeField] private PhysicalWeaponPart[] _parts;

        private Dictionary<HandSide, StrikeCalculator> strikeCalculatorByHandSide;
        private bool calculateStrikes = false;
        private StrikeType manualStrikeType = StrikeType.NotStrike;
        private Guid manualStrikeGuid = Guid.Empty;
        private float timeLeftOnStrikeTimeout;

        private List<Collider> ignoredEnvironmentColliders = new List<Collider>();

        public Weapon weapon { get; private set; }

        public Guid? currentStrikeGuid => calculateStrikes ? leadingStrikeCalculator.currentStrikeGuid : manualStrikeGuid;
        public StrikeType strikeType => calculateStrikes ?
            (leadingStrikeCalculator is null ?
                StrikeType.NotStrike :
                leadingStrikeCalculator.currentStrikeType) :
            manualStrikeType;
        public bool isStriking => !strikeType.Equals(StrikeType.NotStrike);
        public float strikeSpeed => calculateStrikes && isStriking ? leadingStrikeCalculator.currentStrike.averageVelocity : 0;

        public float knockbackMultiplier => wielder.creature.modifiers.effects.Select(effect => effect.knockBackDealt).Product();
        public Strike hitStrike => leadingStrikeCalculator.hitStrike;
        public Vector3 strikeDirection => calculateStrikes && isStriking ? leadingStrikeCalculator.currentStrike.direction : transform.forward;

        public GameObject spellSource;
        public Spell basicSpellInstance => EnumToSpellConverter.EnumToSpell(itemAsset.basicSpellType);

        public UnityEvent<bool> OnColliderEventWithWeaponLodgeSurface = new UnityEvent<bool>();

        public PhysicalWeaponPart[] parts => _parts;
        public WeaponSoundPlayer weaponSoundPlayer { get; protected set; }

        private bool destroyOnImpact => itemAsset?.destroyOnImpact == true;

        public UnityEvent<CreatureReference, StrikeType> OnAttack = new UnityEvent<CreatureReference, StrikeType>();

        public StrikeMovement strikeMovement
        {
            get
            {
                if (leadingStrikeCalculator is null || leadingStrikeCalculator.currentStrikeType.Equals(StrikeType.NotStrike))
                {
                    return StrikeMovement.None;
                }

                var localStrikeDirection = transform.InverseTransformDirection(leadingStrikeCalculator.currentStrike.direction);
                var isStabbing = Vector3.Dot(new Vector3(-1, 0, 0), localStrikeDirection) > 0.9;

                return isStabbing ? StrikeMovement.Stab : StrikeMovement.Slash;
            }
        }

        public StrikeCalculator GetStrikeCalculator(HandSide handSide) => strikeCalculatorByHandSide[handSide];

        private StrikeCalculator leadingStrikeCalculator
        {
            get
            {
                if (handSide.Equals(HandSide.Both))
                {
                    return strikeCalculatorByHandSide[HandSide.Right].currentStrikeType > strikeCalculatorByHandSide[HandSide.Left].currentStrikeType ?
                        strikeCalculatorByHandSide[HandSide.Right] :
                        strikeCalculatorByHandSide[HandSide.Left];
                }
                if (handSide.Equals(HandSide.None))
                {
                    return null;
                }
                else return strikeCalculatorByHandSide[handSide];
            }
        }

        protected override void Awake()
        {
            base.Awake();

            OnEquipped.AddListener(UpdateSpellSourceRotation);

            weaponSoundPlayer = GetComponentInChildren<WeaponSoundPlayer>();
            strikeCalculatorByHandSide = new Dictionary<HandSide, StrikeCalculator>
            {
                { HandSide.Left, new StrikeCalculator(weaponSoundPlayer) },
                { HandSide.Right, new StrikeCalculator(weaponSoundPlayer) }
            };
            weapon = item as Weapon;
        }

        protected override void Start()
        {
            base.Start();
            InjectorContainer.Injector.GetInstance<PhysicalWeaponTracker>().TrackWeapon(this);
        }        

        private void OnCollisionEnter(Collision collision)
        {
            if (weaponClashOutcomeByCollision.ContainsKey(collision))
                return;//TODO: Why can this happen? (it verifably happens, rework into more waterproof approach)

            //TODO: Disable if puppetmaster fixes collision issue 
            if ((wielder is Monster) && collision.collider.gameObject.layer == (int)Layers.Environment)
            {
                List<Collider> colliders = new List<Collider>();

                parts.ToList().ForEach(part => colliders.AddRange(part.colliders.ToList()));

                colliders.ToList().ForEach(collider => Physics.IgnoreCollision(collider, collision.collider, true));
                ignoredEnvironmentColliders.Add(collision.collider);
            }

            var magnitude = collision.relativeVelocity.magnitude;

            if (collision.TryGetCollidingComponent<PhysicalWeapon>(out var physicalWeapon, out var _))
            {
                var winner = GetWeaponClashWinner(new PhysicalWeapon[] { this, physicalWeapon }, collision.relativeVelocity.magnitude, out var isSameWielder);

                if (!isSameWielder)
                {

                    if (winner == null)
                    {
                        weaponClashOutcomeByCollision.Add(collision, WeaponClashOutcome.Draw);
                        opposingPhysicalWeaponByCollision.Add(collision, physicalWeapon);
                    }
                    else if (magnitude > 5f)
                    {
                        var direction = winner.isStriking ? winner.strikeDirection : winner.rigidbody.velocity.normalized;
                        var loser = winner == this ? physicalWeapon : this;

                        if (loser.wielder is ShadowDummy)
                        {
                            (loser.wielder as ShadowDummy).Recoil(magnitude);
                        }
                        else
                        {
                            weaponClashOutcomeByCollision.Add(collision, WeaponClashOutcome.Draw);
                            opposingPhysicalWeaponByCollision.Add(collision, physicalWeapon);
                        }
                    }
                }
            }            

            else if (collision.TryGetCollidingComponent<AttackableSurface>(out var attackableSurface, out var thisCollider))
                HandleAttack(attackableSurface, thisCollider);

            else if (isStriking && collision.gameObject.layer == (int)Layers.Environment)
                DestroyIfDestructible();
        }

        private void OnCollisionStay(Collision collision)
        {
            if (!weaponClashOutcomeByCollision.TryGetValue(collision, out var weaponClashOutcome))
                return;

            var otherPhysicalWeapon = opposingPhysicalWeaponByCollision[collision];

            if (weaponClashOutcome.Equals(WeaponClashOutcome.Draw))
            {
                this.rigidbody.AddForce(-this.rigidbody.velocity * 25, ForceMode.Impulse);
                otherPhysicalWeapon.rigidbody.AddForce(-otherPhysicalWeapon.rigidbody.velocity * 25, ForceMode.Impulse);
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (weaponClashOutcomeByCollision.TryGetValue(collision, out var _))
            {
                weaponClashOutcomeByCollision.Remove(collision);
                opposingPhysicalWeaponByCollision.Remove(collision);
            }
        }

        private enum WeaponClashOutcome
        {
            Unknown,
            Draw,
            Win,
            Loss
        }

        private Dictionary<Collision, WeaponClashOutcome> weaponClashOutcomeByCollision = new Dictionary<Collision, WeaponClashOutcome>();
        private Dictionary<Collision, PhysicalWeapon> opposingPhysicalWeaponByCollision = new Dictionary<Collision, PhysicalWeapon>();

        public int lastWeaponCollisionFrame;

        private static PhysicalWeapon GetWeaponClashWinner(PhysicalWeapon[] physicalWeapons, float collisionMagnitude, out bool isSameWielder)
        {
            if (physicalWeapons.Count() != 2)
                throw new Exception($"'{nameof(physicalWeapons)}' must contain exactly two elements");

            isSameWielder = physicalWeapons[0].wielder == physicalWeapons[1].wielder;

            if (physicalWeapons.Any(physicalWeapon => physicalWeapon.lastWeaponCollisionFrame == Time.frameCount || !physicalWeapon.isWielded))
                return null;

            foreach (var physicalWeapon in physicalWeapons)
                physicalWeapon.lastWeaponCollisionFrame = Time.frameCount;

            if (physicalWeapons.Any(physicalWeapon => physicalWeapon.isStriking))
                foreach (var physicalWeapon in physicalWeapons)
                    physicalWeapon.DestroyIfDestructible();

            if (isSameWielder)
                return null;

            foreach (var physicalWeapon in physicalWeapons)
                physicalWeapon.wielder.OnWeaponClash.Invoke(physicalWeapon);

            var winner = GetWinner(physicalWeapons);

            return winner;
        }

        private static PhysicalWeapon GetWinner(PhysicalWeapon[] physicalWeapons)
        {
            var physicalWeaponA = physicalWeapons[0];
            var physicalWeaponB = physicalWeapons[1];

            var combatSystem = InjectorContainer.Injector.GetInstance<ICombatSystem>();
            var winningCreature = combatSystem.GetOverwhelmWinner(
                creatureA: physicalWeaponA.wielder.creature,
                weaponA: physicalWeaponA.weapon,
                strikeTypeA: physicalWeaponA.strikeType,
                creatureB: physicalWeaponB.wielder.creature,
                weaponB: physicalWeaponB.weapon,
                strikeTypeB: physicalWeaponB.strikeType
            );

            if (winningCreature == null)
                return null;

            return physicalWeaponA.wielder.creature == winningCreature ? physicalWeaponA : physicalWeaponB;
        }

        private void HandleAttack(AttackableSurface attackableSurface, Collider thisCollider)
        {
            var collidingPhysicalWeaponPart = parts.Single(part => part.colliders.Contains(thisCollider));
            var collidingWeaponSurface = weapon.weaponSurfaces.Single(weaponSurface => weaponSurface.name == collidingPhysicalWeaponPart.name);

            var handleWeaponAttackCommand = new HandleWeaponAttackCommand(this, collidingWeaponSurface, attackableSurface);
            handleWeaponAttackCommand.Execute();
        }

        private void Update()
        {
            HideOrShowStrikeEffect();
        }

        private void FixedUpdate()
        {
            if (timeLeftOnStrikeTimeout > 0)
            {
                timeLeftOnStrikeTimeout = Mathf.Max(0, timeLeftOnStrikeTimeout - Time.deltaTime);

                if (timeLeftOnStrikeTimeout > 0)
                {
                    manualStrikeType = StrikeType.NotStrike;
                    return;
                }

                calculateStrikes = isWieldedByPlayer;
            }

            if (calculateStrikes)
            {
                foreach (var handSideAndStrikeCalculatorPair in strikeCalculatorByHandSide)
                {
                    handSideAndStrikeCalculatorPair.Value.Calculate();
                }
            }
            else if (!isWielded)
            {
                manualStrikeType = GetFlyingStrike();
            }
        }

        private StrikeType GetFlyingStrike()//TODO: Makeshift solution before proper implementation.
        {
            return rigidbody.velocity.magnitude > 2 ? StrikeType.Incomplete : StrikeType.NotStrike;
        }

        private void HideOrShowStrikeEffect()
        {
            if (strikeEffect == null)
            {
                return;
            }

            if (strikeEffect.gameObject.activeSelf && strikeType.Equals(StrikeType.NotStrike))
            {
                strikeEffect.gameObject.SetActive(false);
            }
            else if (!strikeEffect.gameObject.activeSelf && !strikeType.Equals(StrikeType.NotStrike))
            {
                strikeEffect.gameObject.SetActive(true);
            }
        }

        public void SetManualStrikeType(StrikeType strikeType)
        {
            if (!manualStrikeType.Equals(strikeType))
            {
                manualStrikeType = strikeType;

                if (!strikeType.Equals(StrikeType.NotStrike))
                {
                    manualStrikeGuid = Guid.NewGuid();
                }
            }
        }

        public override void AddWieldingHand(CreatureReference wielder, GameObject target, ConfigurableJoint joint, HandSide handSide)
        {
            var tryingToUseBothHands = this.wielder == wielder;
            strikeCalculatorByHandSide[handSide].SetTarget(target);

            if (!tryingToUseBothHands)
            {
                timeLeftOnStrikeTimeout = StrikeTimeoutOnPickupAndDrop;
            }

            base.AddWieldingHand(wielder, target, joint, handSide);
            OnEquipped.Invoke();
        }

        public override void RemoveWieldingHand(HandSide handSideToRemove)
        {
            var velocityMultiplier = strikeSpeed * (isStriking ? 2 : 1);
            var newVelocity = strikeDirection * velocityMultiplier;
            base.RemoveWieldingHand(handSideToRemove);
            strikeCalculatorByHandSide[handSideToRemove].SetTarget(null);

            if (this.handSide.Equals(HandSide.None))
            {
                rigidbody.velocity = newVelocity;
                calculateStrikes = false;
                ResetEnvironmentCollisions();
            }

            OnUnequipped.Invoke();
        }

        public void ReportHit()
        {
            if (calculateStrikes)
            {
                leadingStrikeCalculator.ReportHit();
            }

            DestroyIfDestructible();
        }

        public Guid? GetSecondaryStrikeGuid()
        {
            if (!handSide.Equals(HandSide.Both))
            {
                return null;
            }

            var secondaryStrikeHandSide = strikeCalculatorByHandSide[HandSide.Right].Equals(leadingStrikeCalculator) ?
                HandSide.Left :
                HandSide.Right;

            return strikeCalculatorByHandSide[secondaryStrikeHandSide].currentStrikeGuid;
        }

        private PhysicalWeaponPart GetPhysicalWeaponPart(Collider collider) => _parts.Single(part => part.colliders.Contains(collider));
        public DamageType GetColliderDamageType(Collider collider)
        {
            var targetPartName = GetPhysicalWeaponPart(collider).name;
            var targetWeaponSurface = weapon.weaponSurfaces.Single(surface => surface.name.Equals(targetPartName));

            return targetWeaponSurface.damageType;
        }

        public Identifiers.InGameMaterial GetColliderInGameMaterial(Collider collider) => GetPhysicalWeaponPart(collider).material;

        private void ResetEnvironmentCollisions()
        {
            var weaponColliders = GetComponents<Collider>().Where(collider => collider.gameObject.layer == (int)Layers.Weapon);

            weaponColliders.ToList().ForEach(weaponCollider => ignoredEnvironmentColliders.ForEach(environmentCollider => Physics.IgnoreCollision(weaponCollider, environmentCollider, false)));
        }

        private void UpdateSpellSourceRotation()
        {
            Vector3 forwardDirection = transform.up * (handSide == HandSide.Right ? 1.0f : -1.0f);
            Vector3 upDirection = -transform.right;

            spellSource.transform.forward = Vector3.RotateTowards(upDirection, forwardDirection, Mathf.Deg2Rad * CombatSettings.Spells.AimAngleAdjust, 0.0f);
        }

        private void DestroyIfDestructible()
        {
            if (weapon.destroyOnImpact)
            {
                if (destructionEffect != null)
                {
                    destructionEffect.transform.parent = null;
                    destructionEffect.SetActive(true);
                }

                Destroy(this.gameObject);
            }
        }

        [Serializable]
        public class PhysicalWeaponPart
        {
            [SerializeField] private string _name;
            [SerializeField] private Identifiers.InGameMaterial _material;
            [SerializeField] private Collider[] _colliders;

            public string name => _name;
            public Identifiers.InGameMaterial material => _material;
            public Collider[] colliders => _colliders;
        }

        private void OnDestroy()
        {
            InjectorContainer.Injector.GetInstance<PhysicalWeaponTracker>().StopTrackingWeapon(this);
        }
    }
}