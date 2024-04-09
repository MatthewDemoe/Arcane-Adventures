using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items
{
    [RequireComponent((typeof(Rigidbody)))]
    public class PhysicalHandHeldItem : MonoBehaviour
    {
        private const float DefaultSlerpDrivePositionSpring = 20000;
        private const float DefaultSlerpDrivePositionDamper = 0;
        private const float DefaultConnectedMassScale = 1;
        private const float WaterDrag = 10;

        public UnityEvent OnEquipped = new UnityEvent();
        public UnityEvent OnUnequipped = new UnityEvent();

        public static JointDrive PowerlessJointDrive => GetJointDrive(positionSpring: 0);
        public static JointDrive DefaultPositionalJointDrive => GetJointDrive(positionSpring: 10000);
        public static JointDrive GetJointDrive(float positionSpring, float positionDamper = 0, float maximumForce = float.MaxValue) => new JointDrive
        {
            positionSpring = positionSpring,
            positionDamper = positionDamper,
            maximumForce = maximumForce
        };

        [SerializeField] [ReadOnly] private CreatureReference _wielder;
        [SerializeField] [ReadOnly] private ItemSlot itemSlot;

        [SerializeField] protected ItemAsset itemAsset;
        [SerializeField] private bool preventManualEquipping;
        [SerializeField] public Transform rightDisplacementReference;
        [SerializeField] public Transform rightReverseDisplacementReference;
        [SerializeField] public Transform leftDisplacementReference;
        [SerializeField] public Transform leftReverseDisplacementReference;

        [SerializeField] Vector3 _gripPoint;
        public Vector3 gripPoint => _gripPoint;

        [SerializeField] private float _gripRadius;
        public float gripRadius => _gripRadius;

        [SerializeField] private Vector3 centerOfMass;

        [SerializeField] bool hasAdjustableGrip;
        [ShowIf(nameof(hasAdjustableGrip))] [SerializeField] float _gripRange;
        public float gripRange => hasAdjustableGrip ? _gripRange : 0;

        [SerializeField] bool useCustomJointSettings;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private float slerpDrivePositionSpring;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private float slerpDrivePositionDamper;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private float connectedMassScaleWhenDragging;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private float connectedMassScaleWhenNotProficient;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private float connectedMassScaleWhenProficient;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private int strengthRequiredForNotProficient;
        [ShowIf(nameof(useCustomJointSettings))] [SerializeField] private int strengthRequiredForProficient;

        private bool isFirstOnLevelWasLoaded = true;
        public HandHeldItem item { get; protected set; }
        public new Rigidbody rigidbody { get; private set; }

        public bool isWielded => _wielder != null;
        public CreatureReference wielder => _wielder;
        public HandSide handSide { get; private set; }
        public ItemUI itemUI { get; private set; }
        public Collider[] colliders { get; private set; }
        public bool isUnderWater { get; private set; } = false;

        public bool isWieldedByPlayer => isWielded && _wielder is PlayerCharacterReference;

        private ConfigurableJoint connectedJoint = null;

        private bool IsInPlayerItemSlot() => itemSlot != null && itemSlot.GetComponentInParent<PlayerCharacterReference>() != null;

        protected virtual void Awake()
        {
            this.transform.parent = null;
            DontDestroyOnLoad(this.gameObject);
            rigidbody = GetComponent<Rigidbody>();
            colliders = GetComponents<Collider>();

            if (itemAsset != null)
            {
                item = ItemCache.GetItem(itemAsset.itemName) as HandHeldItem;
            }
        }

        protected virtual void Start()
        {
            rigidbody.centerOfMass = centerOfMass;

            if (!preventManualEquipping)
            {
                itemUI = Instantiate(Prefabs.Load(Prefabs.PrefabNames.ItemUI), this.transform).GetComponent<ItemUI>();
            }
        }

        private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
        private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

        public void SetUnderWater(bool isUnderWater)
        {
            this.isUnderWater = isUnderWater;
            rigidbody.drag = isUnderWater ? WaterDrag : 0;
            rigidbody.angularDrag = isUnderWater ? WaterDrag : 0;
        }

        public virtual void AddWieldingHand(CreatureReference wielder, GameObject target, ConfigurableJoint joint, HandSide handSide)
        {
            var tryingToUseBothHands = _wielder == wielder;

            if (tryingToUseBothHands)
            {
                ConnectToJoint(joint);
                this.handSide = HandSide.Both;
            }
            else
            {
                _wielder = wielder;
                transform.rotation = joint.transform.rotation;
                ConnectToJoint(joint);
                rigidbody.detectCollisions = true;
                this.handSide = handSide;

                if (itemSlot != null)
                {
                    itemSlot.GetComponent<Joint>().connectedBody = null;
                    itemSlot = null;
                }
            }
        }

        private void ConnectToJoint(ConfigurableJoint joint)
        {
            var newSlerpDrivePositionSpring = useCustomJointSettings ? slerpDrivePositionSpring : DefaultSlerpDrivePositionSpring;
            var newSlerpDrivePositionDamper = useCustomJointSettings ? slerpDrivePositionDamper : DefaultSlerpDrivePositionDamper;

            joint.connectedBody = rigidbody;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Locked;
            joint.connectedAnchor = gripPoint;
            joint.xDrive = joint.yDrive = joint.zDrive = DefaultPositionalJointDrive;
            joint.slerpDrive = GetJointDrive(newSlerpDrivePositionSpring, newSlerpDrivePositionDamper);
            joint.connectedMassScale = GetConnectedMassScale();

            connectedJoint = joint;
        }

        public void RefreshConnectedJoint()
        {
            if (connectedJoint == null)
                return;

            connectedJoint.connectedAnchor = gripPoint;

        }

        public void PutInItemSlot(ItemSlot newItemSlot)
        {
            if (newItemSlot.disableWeaponPhysicsWhenInSlot)
            {
                rigidbody.detectCollisions = false;
            }

            itemSlot = newItemSlot;
            var joint = itemSlot.GetComponent<Joint>();
            transform.rotation = joint.transform.rotation;
            joint.connectedBody = rigidbody;
        }

        public virtual void RemoveWieldingHand(HandSide handSideToRemove)
        {
            if (!this.handSide.Equals(HandSide.Both) || handSideToRemove.Equals(HandSide.Both))
            {
                _wielder = null;
                this.handSide = HandSide.None;
            }
            else if (!handSideToRemove.Equals(HandSide.None))
            {
                this.handSide = handSideToRemove.Equals(HandSide.Left) ? HandSide.Right : HandSide.Left;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (isFirstOnLevelWasLoaded)
            {
                isFirstOnLevelWasLoaded = false;
            }
            else if (!isWieldedByPlayer && !IsInPlayerItemSlot())
            {
                Destroy(this.gameObject);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(gripPoint, gripRadius);

            if (hasAdjustableGrip)
            {
                var gripRangeFrom = gripPoint;
                gripRangeFrom.x -= gripRange * 0.5f;
                var gripRangeTo = gripPoint;
                gripRangeTo.x += gripRange * 0.5f;
                Gizmos.DrawLine(gripRangeFrom, gripRangeTo);
            }

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(centerOfMass, 0.05f);
        }
#endif

        public float GetConnectedMassScale()
        {
            if (!useCustomJointSettings)
                return DefaultConnectedMassScale;

            var proficiencyLevel = GetProficiencyLevel();

            return proficiencyLevel switch
            {
                ProficiencyLevel.Proficient => connectedMassScaleWhenProficient,
                ProficiencyLevel.NotProficient => connectedMassScaleWhenNotProficient,
                ProficiencyLevel.Dragging => connectedMassScaleWhenDragging,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public ProficiencyLevel GetProficiencyLevel()
        {
            var strength = wielder.creature.stats.subtotalStrength;

            if (strength < strengthRequiredForNotProficient)
                return ProficiencyLevel.Dragging;
            
            return strength >= strengthRequiredForProficient ?
                ProficiencyLevel.Proficient : ProficiencyLevel.NotProficient;
        }

        public enum ProficiencyLevel
        {
            Proficient,
            NotProficient,
            Dragging
        }
    }
}