using System;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Debug;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using UnityEngine.XR;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class PlayerCharacterCreatureController : MonoBehaviour, ICreatureController
    {
        private const float SpeedMultiplier = 10;
        private const float DraggingSpeedMultiplier = 2;
        private const float MinimumInputMagnitude = 0.001f;
        private const float TargetPositionDistanceMultiplier = 100;
        private const string PuppetMasterObjectName = "PuppetMaster";
        private const string RootBoneObjectName = "DEF-Hips";
        
        private static Vector3 RestrictorJointAnchorPosition = new Vector3(0.0022f, 0.005f, -0.0017f);
        
        [Inject] private OculusControllerInputReader oculusControllerInputReader;
        private PlayerCharacterReference playerCharacterReference;
        private PlayerItemEquipper playerItemEquipper;
        private PlayerSpellCaster playerSpellCaster;
        private Animator animator;//TODO: Remove (doesn't belong here)
        private PlayerCharacterRotator playerCharacterRotator;
        private PhysicalHandHeldItem.ProficiencyLevel rightHandProficiencyLevel;
        private PhysicalHandHeldItem.ProficiencyLevel leftHandProficiencyLevel;
        private ConfigurableJoint rightControllerLinkRestrictorJoint;
        private ConfigurableJoint leftControllerLinkRestrictorJoint;
        private bool isDraggingItem = false;

        public UnityEvent<HandSide> OnTriggerPressed = new UnityEvent<HandSide>();
        public Vector3 targetPosition { get; private set; }
        public float targetSpeed { get; private set; }
        public Vector3 lookPosition { get; private set; }
        public bool crouch { get; private set; }
        public bool jump { get; private set; }

        private void Awake()
        {
            playerItemEquipper = GetComponent<PlayerItemEquipper>();
            playerSpellCaster = GetComponent<PlayerSpellCaster>();
            playerCharacterReference = GetComponent<PlayerCharacterReference>();
            animator = GetComponentInChildren<Animator>();
            playerCharacterRotator = new PlayerCharacterRotator(GetComponent<Rigidbody>(), Camera.main);
            GetComponent<CreatureControlInterpreter>().creatureController = this;
        }

        private void Start() {
            InjectorContainer.Injector.Inject(this);
            oculusControllerInputReader.OnUpdated.AddListener(ProcessInput);
            AddControllerLinkRestrictorJoints();
        }

        public void ReportProficiencyLevel(HandSide handSide, PhysicalHandHeldItem.ProficiencyLevel proficiencyLevel)
        {
            switch (handSide)
            {
                case HandSide.Right: rightHandProficiencyLevel = proficiencyLevel; break;
                case HandSide.Left: leftHandProficiencyLevel = proficiencyLevel; break;
                default: throw new ArgumentOutOfRangeException();
            }

            isDraggingItem =
                rightHandProficiencyLevel.Equals(PhysicalHandHeldItem.ProficiencyLevel.Dragging) ||
                leftHandProficiencyLevel.Equals(PhysicalHandHeldItem.ProficiencyLevel.Dragging);

            SetLinearLimitLimit(rightControllerLinkRestrictorJoint, GetLinearLimitLimit(rightHandProficiencyLevel), GetLinearLimitSpring(rightHandProficiencyLevel));
            SetLinearLimitLimit(leftControllerLinkRestrictorJoint, GetLinearLimitLimit(leftHandProficiencyLevel), GetLinearLimitSpring(leftHandProficiencyLevel));
        }
        
        private float GetLinearLimitLimit(PhysicalHandHeldItem.ProficiencyLevel proficiencyLevel)
            => proficiencyLevel.Equals(PhysicalHandHeldItem.ProficiencyLevel.Dragging) ? 0.4f : 0.65f;
        
        private float GetLinearLimitSpring(PhysicalHandHeldItem.ProficiencyLevel proficiencyLevel)
            => proficiencyLevel.Equals(PhysicalHandHeldItem.ProficiencyLevel.Dragging) ? 0 : 100000;

        private void SetLinearLimitLimit(ConfigurableJoint joint, float limit, float spring)
        {
            var linearLimit = joint.linearLimit;
            linearLimit.limit = limit;

            var linearLimitSpring = joint.linearLimitSpring;
            linearLimitSpring.spring = spring;
            
            joint.linearLimit = linearLimit;
            joint.linearLimitSpring = linearLimitSpring;
        }

        private void AddControllerLinkRestrictorJoints()
        {
            var pelvis = transform.parent.FindChildRecursive(PuppetMasterObjectName).FindChildRecursive(RootBoneObjectName).gameObject;
            SetupControllerLinkRestrictorJoint(pelvis, isRight: true);
            SetupControllerLinkRestrictorJoint(pelvis, isRight: false);
        }

        private void SetupControllerLinkRestrictorJoint(GameObject root, bool isRight)
        {
            var connectedBody = (isRight ? ControllerLink.Right : ControllerLink.Left).GetComponent<Rigidbody>();
            var anchorPosition = RestrictorJointAnchorPosition;

            if (!isRight)
                anchorPosition.x *= -1;
            
            var joint = root.AddComponent<ConfigurableJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = Vector3.zero;
            joint.anchor = anchorPosition;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Limited;
            joint.linearLimitSpring = new SoftJointLimitSpring { spring = 100000 };
            joint.linearLimit = new SoftJointLimit { limit = 0.75f };
            joint.connectedMassScale = 1000;
            joint.connectedBody = connectedBody;

            if (isRight)
                rightControllerLinkRestrictorJoint = joint;
            else
                leftControllerLinkRestrictorJoint = joint;
        }

        private void ProcessInput()
        {
            if (!playerCharacterReference.creature.isInputEnabled)
                return;

            if (oculusControllerInputReader.DidButtonStartBeingPressed(OculusControllerInputReader.Controller.Left, OculusControllerInputReader.Button.Primary))
                playerSpellCaster.CancelSpells();

            if (oculusControllerInputReader.DidButtonStartBeingPressed(OculusControllerInputReader.Controller.Left, OculusControllerInputReader.Button.Secondary))
                DebugMenu.FlipSwitch();

            if (oculusControllerInputReader.DidButtonStartBeingPressed(OculusControllerInputReader.Controller.Right, OculusControllerInputReader.Button.Secondary))
                SpellMenu.Instance.ToggleSpellMenu();

            CheckGripButtonInput(OculusControllerInputReader.Controller.Left);
            CheckGripButtonInput(OculusControllerInputReader.Controller.Right);

            CheckSwitchGripInput(OculusControllerInputReader.Controller.Left);
            CheckSwitchGripInput(OculusControllerInputReader.Controller.Right);

            CheckSpellInput(OculusControllerInputReader.Controller.Left);
            CheckSpellInput(OculusControllerInputReader.Controller.Right);
            
            crouch = oculusControllerInputReader.IsButtonBeingHeld(OculusControllerInputReader.Controller.Left, OculusControllerInputReader.Button.Stick);
            jump = !isDraggingItem && oculusControllerInputReader.DidButtonStartBeingPressed(OculusControllerInputReader.Controller.Right, OculusControllerInputReader.Button.Primary);

            var leftAxis = oculusControllerInputReader.GetStickInput(OculusControllerInputReader.Controller.Left);
            
            if (leftAxis.magnitude > MinimumInputMagnitude)
            {
                var speedMultiplier = isDraggingItem ? DraggingSpeedMultiplier : SpeedMultiplier;
                targetSpeed = leftAxis.magnitude * speedMultiplier;
                targetPosition = GetTargetPosition(leftAxis);
            }
            else targetSpeed = 0;
            
            var rightAxis = oculusControllerInputReader.GetStickInput(OculusControllerInputReader.Controller.Right);
            playerCharacterRotator.Rotate(rightAxis.x);
            playerCharacterRotator.AutoRotateToCamera();
        }

        private Vector3 GetTargetPosition(Vector2 input)
        {
            var localPosition = new Vector3(input.x, 0, input.y);
            var worldPosition = Camera.main.transform.TransformPoint(localPosition.normalized);
            worldPosition.y = transform.position.y;
            var direction = transform.rotation * animator.transform.InverseTransformPoint(worldPosition).normalized;
            
            return direction * TargetPositionDistanceMultiplier;
        }
        
        private void CheckSwitchGripInput(OculusControllerInputReader.Controller controller)
        {
            var handSide = controller.Equals(OculusControllerInputReader.Controller.Left) ? HandSide.Left : HandSide.Right;
            
            if (oculusControllerInputReader.IsButtonBeingHeld(controller, OculusControllerInputReader.Button.Grip) && oculusControllerInputReader.DidButtonStartBeingPressed(controller, OculusControllerInputReader.Button.Trigger) && playerItemEquipper.IsEquipped(handSide))
            {
                ControllerLink.Get(handSide).SwitchGrip();
                oculusControllerInputReader.SetReleaseNeutralized(controller, OculusControllerInputReader.Button.Grip);
            }
        }

        private void CheckGripButtonInput(OculusControllerInputReader.Controller controller)
        {
            var handSide = controller.Equals(OculusControllerInputReader.Controller.Left) ? HandSide.Left : HandSide.Right;
            
            if (playerItemEquipper.IsEquipped(handSide))
            {
                if (oculusControllerInputReader.WasButtonReleased(controller, OculusControllerInputReader.Button.Grip))
                {
                    Controllers.SendHapticImpulse(handSide, 0.25f, 0.1f);
                    playerItemEquipper.UnequipItem(handSide);
                }
            }
            else if (oculusControllerInputReader.DidButtonStartBeingPressed(controller, OculusControllerInputReader.Button.Grip) && playerItemEquipper.TryEquipDetectedItem(handSide))
            {
                oculusControllerInputReader.SetReleaseNeutralized(controller, OculusControllerInputReader.Button.Grip);
            }
            else
            {
                var inputDevice = oculusControllerInputReader.GetInputDevice(controller);
                inputDevice.TryGetFeatureValue(CommonUsages.grip, out var gripAmount);
                var layer = handSide.Equals(HandSide.Left) ? 2 : 1;
                animator.Play("Grip", layer, gripAmount);//TODO: Don't do it statically.
            }
        }

        private void CheckSpellInput(OculusControllerInputReader.Controller controller)
        {
            if (oculusControllerInputReader.IsButtonBeingHeld(controller, OculusControllerInputReader.Button.Grip))
                return;

            var handSide = controller.Equals(OculusControllerInputReader.Controller.Left) ? HandSide.Left : HandSide.Right;

            if (oculusControllerInputReader.DidButtonStartBeingPressed(controller, OculusControllerInputReader.Button.Trigger))
            {
                playerSpellCaster.HandleButtonPressed(handSide);
                OnTriggerPressed.Invoke(handSide);
            }

            if (oculusControllerInputReader.IsButtonBeingHeld(controller, OculusControllerInputReader.Button.Trigger))
                playerSpellCaster.ChannelSpell(handSide);

            if (oculusControllerInputReader.WasButtonReleased(controller, OculusControllerInputReader.Button.Trigger))
                playerSpellCaster.HandleButtonReleased(handSide);
        }
    }
}