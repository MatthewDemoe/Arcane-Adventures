using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class NPCControllerLink : MonoBehaviour
    {
        private const float ForcePositionDifferenceMultiplier = 100000;
        private const float ForceVelocityCounterMultiplier = 1000;
        private const float BaseAngularSpeedModifier = 1;
        private const float RestrictionDistanceSafetyMultiplier = 0.95f;

        [SerializeField] private HandSide _handSide;
        [SerializeField] private GameObject target;
        [SerializeField] private Vector3 displacement;
        [SerializeField] private Transform restrictor;
        [SerializeField] private float restrictionDistance;



        private static Quaternion GripAngleAdjustment = Quaternion.Euler(0, 90, 0);

        private PhysicalHandHeldItem physicalHandHeldItem;
        private ConfigurableJoint joint;

        private float angularSpeedModifier => BaseAngularSpeedModifier;
        private float timeLeftToSleepInSeconds;

        public Rigidbody rigidBody { get; private set; }
        public HandSide handSide => _handSide;
        public PhysicalHandHeldItem connectedItem => physicalHandHeldItem;
        public bool isHoldingItem => physicalHandHeldItem != null;

        private void Awake()
        {
            joint = GetComponent<ConfigurableJoint>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.maxAngularVelocity = CombatSettings.Controller.MaximumAngularVelocity;
        }

        private void FixedUpdate()
        {
            if (timeLeftToSleepInSeconds > 0)
                timeLeftToSleepInSeconds -= Time.deltaTime;

            ApplyForce();
            ApplyAngularForce();
        }

        public void Sleep(float timeToSleepInSeconds)
        {
            timeLeftToSleepInSeconds = timeToSleepInSeconds;
        }

        public void SetTarget(GameObject target)
        {
            this.target = target;
        }

        private void ApplyForce()
        {
            if (!EnforceDistanceRestriction() || target == null || timeLeftToSleepInSeconds > 0)
                return;

            var targetPosition = target.transform.TransformPoint(displacement, ignoreScale: true);
            AddForce(targetPosition);
        }

        private void ApplyAngularForce()
        {
            if (target == null || timeLeftToSleepInSeconds > 0)
                return;

            var targetRotation = target.transform.rotation * GripAngleAdjustment;
            AddAngularForce(targetRotation);
        }

        private bool EnforceDistanceRestriction()
        {
            if (restrictor == null ||/*TODO: Just to avoid exceptions at this point.*/ Vector3.Distance(restrictor.position, transform.position) < restrictionDistance)
                return true;

            var direction = (transform.position - restrictor.position).normalized;
            var targetPosition = restrictor.position + (direction * restrictionDistance * RestrictionDistanceSafetyMultiplier);

            AddForce(targetPosition);

            return false;
        }

        private void AddForce(Vector3 targetPosition)
        {
            var force = (((targetPosition - transform.position) * ForcePositionDifferenceMultiplier) - (rigidBody.velocity * ForceVelocityCounterMultiplier)) * Time.deltaTime;

            rigidBody.AddForce(force, ForceMode.Impulse);
        }

        private void AddAngularForce(Quaternion targetRotation)
        {
            var difference = targetRotation * Quaternion.Inverse(transform.rotation);
            difference.ToAngleAxis(out var angle, out var axis);
            var rotationIsAlreadyAligned = float.IsInfinity(axis.x);

            if (rotationIsAlreadyAligned)
                return;

            var speed = Time.deltaTime * angularSpeedModifier;
            var torque = Mathf.Deg2Rad * angle / speed * axis.normalized;
            rigidBody.AddTorque(-rigidBody.angularVelocity, ForceMode.Impulse);
            rigidBody.AddTorque(torque, ForceMode.Impulse);
        }

        public void Disconnect()
        {
            if (!isHoldingItem) { return; }

            joint.connectedBody = null;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Free;
            joint.xDrive = joint.yDrive = joint.zDrive = joint.slerpDrive = PhysicalHandHeldItem.PowerlessJointDrive;
            physicalHandHeldItem = null;
        }

        public void SetGripPoint(float gripPoint)
        {
            var min = physicalHandHeldItem.gripPoint.x - (physicalHandHeldItem.gripRange * 0.5f);
            var max = min + physicalHandHeldItem.gripRange;
            gripPoint = Mathf.Clamp(gripPoint, min, max);
            var connectedAnchor = joint.connectedAnchor;
            connectedAnchor.x = gripPoint;
            joint.connectedAnchor = connectedAnchor;
        }

        public void SetItem(PhysicalHandHeldItem physicalHandHeldItem)
        {
            this.physicalHandHeldItem = physicalHandHeldItem;
        }

        public Vector3 GetConnectedItemGripPosition(Vector3 displacement)
        {
            if (!isHoldingItem) { return Vector3.zero; }

            return connectedItem.transform.TransformPoint(joint.connectedAnchor + displacement);
        }
    }
}