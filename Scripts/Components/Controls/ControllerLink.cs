using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Player; 
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using Unity.Mathematics;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Controls
{
    public class ControllerLink : MonoBehaviour
    {
        private const float ForcePositionDifferenceMultiplier = 100000;
        private const float ForceVelocityCounterMultiplier = 1000;
        private bool isPlayerDazed => PlayerCharacterReference.Instance.HasStatusCondition(AllStatusConditions.StatusConditionName.Dazed);

        private static Quaternion GripAngleAdjustment = Quaternion.Euler(0, 90, -45);

        public static ControllerLink Left { get; private set; }
        public static ControllerLink Right { get; private set; }
        public static ControllerLink Get(HandSide handSide) => handSide.Equals(HandSide.Left) ? Left : Right;

        [SerializeField] private HandSide _handSide;
        [SerializeField] private bool useExtraRotationDisplacement = false;
        [SerializeField] private Vector3 extraRotationDisplacement; 

        private GameObject controller;
        private ConfigurableJoint joint;
        private int cachedStrength;
        private PhysicalHandHeldItem physicalHandHeldItem;
        private bool movementDelayed = false;
        private float wingspanScale;
        private ProjectSpecificTrackedPoseDriver projectSpecificTrackedPoseDriver;
        private float characterWingspan = 1.54f;

        private HandTransformUpdater handTransformUpdater => PlayerCharacterReference.Instance.handTransformUpdater;

        [Inject] protected SetupPhaseInputHandler.SetupPhaseSettings setupPhaseSettings;
        [Inject] protected PlayerItemEquipper playerItemEquipper;

        public bool isUsingStandardGrip { get; private set; } = true;
        public bool isSecondHandInTwoHandedWielding { get; private set; } = false;
        public Rigidbody rigidBody { get; private set; }
        public Vector3 positionDisplacement { get; private set; }
        public Quaternion rotationDisplacement { get; private set; }

        public bool isWieldingWithTwoHands => false;//TODO: Implement again, this time with separate grips on the same weapon.
        public PhysicalHandHeldItem connectedItem => physicalHandHeldItem;
        public float gripHeight => isHoldingItem ? joint.connectedAnchor.x : 0;
        public bool isHoldingItem => physicalHandHeldItem != null;
        public HandSide handSide => _handSide;

        private void Awake()
        {
            joint = GetComponent<ConfigurableJoint>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.maxAngularVelocity = CombatSettings.Controller.MaximumAngularVelocity;
        }
        
        private void UpdateDisplacement()
        {
            if (!isHoldingItem || physicalHandHeldItem.rightDisplacementReference == null)
            {
                positionDisplacement = Vector3.zero;
                rotationDisplacement = connectedItem.transform.rotation;
                return;
            }

            var reference = handSide.Equals(HandSide.Right)
                ?
                isUsingStandardGrip
                    ? connectedItem.rightDisplacementReference
                    : connectedItem.rightReverseDisplacementReference
                :
                isUsingStandardGrip
                    ? connectedItem.leftDisplacementReference
                    : connectedItem.leftReverseDisplacementReference;
            var newPositionDisplacement = reference.localPosition;
            var newRotationDisplacement = reference.rotation;

            if (isSecondHandInTwoHandedWielding)
            {
                newPositionDisplacement *= -1;

                var localEuler = reference.localRotation.eulerAngles;
                newRotationDisplacement = reference.parent.rotation * Quaternion.Euler(localEuler.x, -localEuler.y, -localEuler.z);
            }

            positionDisplacement = newPositionDisplacement;
            rotationDisplacement = newRotationDisplacement;
        }
        
        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            wingspanScale = characterWingspan / setupPhaseSettings.wingspan;
            projectSpecificTrackedPoseDriver = Camera.main.GetComponent<ProjectSpecificTrackedPoseDriver>();
        }

        public void SetHandSide(HandSide handSide)
        {
            _handSide = handSide;

            if (handSide.Equals(HandSide.Left))
            {
                Left = this;
            }
            else if (handSide.Equals(HandSide.Right))
            {
                Right = this;
            }
            else throw new System.Exception("Invalid Handside");

            controller = XRReferences.GetController(handSide);
        }

        public Vector3 GetHandPosition()
        {
            if (isHoldingItem)
            {
                var localPosition = joint.connectedAnchor + positionDisplacement;
                
                return connectedItem.transform.TransformPoint(localPosition);
            }

            return transform.position;
        }

        public void SetItem(PhysicalHandHeldItem physicalHandHeldItem, bool isSecondHandInTwoHandedWielding = false)
        {
            this.physicalHandHeldItem = physicalHandHeldItem;
            this.isSecondHandInTwoHandedWielding = isSecondHandInTwoHandedWielding;
            SetGrip();
            UpdateJointSettings(forceUpdate: true);
        }
        
        private void FixedUpdate()
        {
            if (PlayerCharacterReference.Instance is null || handSide.Equals(HandSide.None)) { return; }

            if (!PlayerCharacterReference.Instance.creature.isInputEnabled)
            {
                AddForce(handTransformUpdater.GetMiddleFingerBone(handSide).transform.position);
                AddAngularForce(controller.transform.rotation);
            }
            else if (PlayerCharacterReference.Instance.creature.isActionEnabled && !isPlayerDazed)
            {
                AddForce(controller.transform.position);
                AddAngularForce(controller.transform.rotation);
            }

            UpdateJointSettings();
            UpdateDisplacement();
        }

        private void AddForce(Vector3 targetPosition)
        {
            if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
            {
                var target = controller.transform;
                targetPosition = target.localPosition + target.parent.localPosition;
                targetPosition.y = projectSpecificTrackedPoseDriver.GetGameWorldY(target.localPosition.y, clamp: false);
                targetPosition.x *= wingspanScale;
                targetPosition.z *= wingspanScale;

                targetPosition = target.parent.parent.TransformPoint(targetPosition);
            }

            var force = (((targetPosition - transform.position) * ForcePositionDifferenceMultiplier) - (rigidBody.velocity * ForceVelocityCounterMultiplier)) * Time.deltaTime;

            rigidBody.AddForce(force, ForceMode.Impulse);
        }

        private void AddAngularForce(Quaternion targetRotation)
        {
            var adjustment = GripAngleAdjustment;
            
            if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
            {
                var displacement = Quaternion.Euler(0, 0, handSide.Equals(HandSide.Left) ? 90 : -90);
                targetRotation *= displacement;

                var tiltWeapon = isHoldingItem && physicalHandHeldItem.name.Equals("SmallWand");
                adjustment = tiltWeapon ? Quaternion.Euler(new Vector3(0, 90, 0)) : Quaternion.identity;
            }

            var difference = targetRotation * adjustment * Quaternion.Inverse(transform.rotation);
            difference.ToAngleAxis(out var angle, out var axis);
            
            if (angle > 180f)
                angle -= 360f;

            if (Mathf.Approximately(angle, 0))
            {
                rigidBody.AddTorque(-rigidBody.angularVelocity, ForceMode.Impulse);
                return;                
            }
            
            var radians = angle * Mathf.Deg2Rad;
            var torque = axis * radians / Time.deltaTime;
            rigidBody.AddTorque(torque - rigidBody.angularVelocity, ForceMode.Impulse);
        }

        private void UpdateJointSettings(bool forceUpdate = false)
        {
            if (!isHoldingItem) return;

            var currentStrength = PlayerCharacterReference.Instance == null || PlayerCharacterReference.Instance.creature == null ?
                0 : PlayerCharacterReference.Instance.creature.stats.subtotalStrength;

            if (!forceUpdate && cachedStrength == currentStrength) return;

            cachedStrength = currentStrength;
            joint.connectedMassScale = physicalHandHeldItem.GetConnectedMassScale();

            var playerCharacterCreatureController = PlayerCharacterReference.Instance.GetComponent<PlayerCharacterCreatureController>();
            playerCharacterCreatureController.ReportProficiencyLevel(handSide, physicalHandHeldItem.GetProficiencyLevel());
        }

        public void DisconnectPhysicalHandHeldItem()
        {
            joint.connectedBody = null;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Free;
            joint.xDrive = joint.yDrive = joint.zDrive = joint.slerpDrive = PhysicalHandHeldItem.PowerlessJointDrive;
            physicalHandHeldItem = null;
            isUsingStandardGrip = true;
            isSecondHandInTwoHandedWielding = false;
            SetGrip();

            var otherHandSide = handSide.Equals(HandSide.Left) ? HandSide.Right : HandSide.Left;
            var otherControllerLink = Get(otherHandSide);

            if (otherControllerLink.isSecondHandInTwoHandedWielding)
            {
                otherControllerLink.isSecondHandInTwoHandedWielding = false;
                playerItemEquipper.UnequipItem(otherHandSide);//TODO: Measure to prevent the displaced grips that occur when the "primary" hand drops the weapon. 
            }
        }
        
        public void SwitchGrip()
        {
            if (!isHoldingItem) return;

            Controllers.SendHapticImpulse(handSide, 0.25f, 0.1f);
            isUsingStandardGrip = !isUsingStandardGrip;
            SetGrip();
        }

        private void SetGrip()
        {
            var yModifier = handSide.Equals(HandSide.Right) == isUsingStandardGrip ? 0 : 180;
            joint.targetRotation = Quaternion.Euler(90, yModifier, 0);
        }

        public void SetGripPoint(float gripPoint)
        {
            //TODO: Once the UMA hand implementation is complete, reintroduce this (or equivalent): PlayerCharacterHand.GetHand(handSide).UpdateGrip();
            var min = physicalHandHeldItem.gripPoint.x - (physicalHandHeldItem.gripRange * 0.5f);
            var max = min + physicalHandHeldItem.gripRange;
            gripPoint = Mathf.Clamp(gripPoint, min, max);
            var connectedAnchor = joint.connectedAnchor;
            connectedAnchor.x = gripPoint;
            joint.connectedAnchor = connectedAnchor;
        }

        public void StartDelayingHandMovements()
        {
            StartCoroutine(DelayHandMovements());
        }

        IEnumerator DelayHandMovements()
        {
            movementDelayed = true;

            float elapsedTime = 0.0f;
            float delayAmount = 0.5f;

            List<Vector3> delayedHandPositions = new List<Vector3>();
            List<Quaternion> delayedHandRotations = new List<Quaternion>();

            while (isPlayerDazed)
            {
                delayedHandPositions.Add(controller.transform.position);
                delayedHandRotations.Add(controller.transform.rotation);

                if (elapsedTime >= delayAmount)
                {
                    AddForce(delayedHandPositions[0]);
                    AddAngularForce(delayedHandRotations[0]);

                    delayedHandPositions.RemoveAt(0);
                    delayedHandRotations.RemoveAt(0);
                }                

                elapsedTime += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            movementDelayed = false;
        }
    }
}