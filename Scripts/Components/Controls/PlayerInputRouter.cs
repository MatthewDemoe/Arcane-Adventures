using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Debug;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Controls
{
    [RequireComponent(
        typeof(PlayerCharacterReference), 
        typeof(PlayerItemEquipper), 
        typeof(PlayerCharacterMovementController)
    )]
    public class PlayerInputRouter : MonoBehaviour
    {
        private static IReadOnlyList<HandSide> HandSides = new List<HandSide>{ HandSide.Left, HandSide.Right };
        private static IReadOnlyList<Button> Buttons = EnumExtensions.GetValues<Button>();

        private readonly Dictionary<HandSide, Dictionary<Button, bool>> isPressedByButtonByHandSide = new Dictionary<HandSide, Dictionary<Button, bool>>();
        private readonly Dictionary<HandSide, Dictionary<Button, bool>> wasPressedByButtonByHandSide = new Dictionary<HandSide, Dictionary<Button, bool>>();
        private readonly Dictionary<HandSide, Dictionary<Button, bool>> isReleaseNeutralizedByButtonByHandSide = new Dictionary<HandSide, Dictionary<Button, bool>>();

        private InputDevice leftController;
        private InputDevice rightController;
        private PlayerCharacterReference playerCharacterReference;
        private PlayerItemEquipper playerItemEquipper;
        private PlayerCharacterMovementController playerCharacterMovementController;
        private PlayerSpellCaster playerSpellCaster;

        private void Start()
        {
            playerItemEquipper = GetComponent<PlayerItemEquipper>();
            playerCharacterMovementController = GetComponent<PlayerCharacterMovementController>();
            playerSpellCaster = GetComponent<PlayerSpellCaster>();
            playerCharacterReference = GetComponent<PlayerCharacterReference>();
            PopulateDictionaries();
        }

        private void PopulateDictionaries()
        {
            foreach (var handSide in EnumExtensions.GetValues<HandSide>())
            {
                isPressedByButtonByHandSide.Add(handSide, new Dictionary<Button, bool>());
                wasPressedByButtonByHandSide.Add(handSide, new Dictionary<Button, bool>());
                isReleaseNeutralizedByButtonByHandSide.Add(handSide, new Dictionary<Button, bool>());

                foreach (var button in EnumExtensions.GetValues<Button>())
                {
                    isPressedByButtonByHandSide[handSide].Add(button, false);
                    wasPressedByButtonByHandSide[handSide].Add(button, false);
                    isReleaseNeutralizedByButtonByHandSide[handSide].Add(button, false);
                }
            }
        }

        private void Update()
        {
            if (!playerCharacterReference.creature.isInputEnabled)
                return;

            leftController = Controllers.GetLeftController();
            rightController = Controllers.GetRightController();
            UpdateButtonStates();
            CheckInput();
        }

        private void UpdateButtonStates()
        {
            foreach (var handSide in HandSides)
            {
                foreach (var button in Buttons)
                {
                    UpdateButtonState(handSide, button);
                }
            }
        }

        private void UpdateButtonState(HandSide handSide, Button button)
        {
            wasPressedByButtonByHandSide[handSide][button] = isPressedByButtonByHandSide[handSide][button];
            var controller = handSide.Equals(HandSide.Left) ? leftController : rightController;
            controller.TryGetFeatureValue(GetInputFeatureUsage(button), out var isPressed);
            isPressedByButtonByHandSide[handSide][button] = isPressed;

            if (!isPressedByButtonByHandSide[handSide][button] && 
                !wasPressedByButtonByHandSide[handSide][button] &&
                isReleaseNeutralizedByButtonByHandSide[handSide][button])
            {
                isReleaseNeutralizedByButtonByHandSide[handSide][button] = false;
            }
        }

        private InputFeatureUsage<bool> GetInputFeatureUsage(Button button)
        {
            return button switch
            {
                Button.Primary => CommonUsages.primaryButton,
                Button.Secondary => CommonUsages.secondaryButton,
                Button.Trigger => CommonUsages.triggerButton,
                Button.Grip => CommonUsages.gripButton,
                Button.Stick => CommonUsages.primary2DAxisClick,
                _ => throw new NotImplementedException(),
            };
        }

        private void HandleXButton()
        {
            if (DidButtonStartBeingPressed(HandSide.Left, Button.Primary))
            {
                playerSpellCaster.CancelSpells();
            }
        }

        private void HandleYButton()
        {
            if (DidButtonStartBeingPressed(HandSide.Left, Button.Secondary))
            {
                DebugMenu.FlipSwitch();
            }
        }

        private void HandleAButton()
        {
            if (DidButtonStartBeingPressed(HandSide.Right, Button.Primary))
            {
                playerCharacterMovementController.Jump();
            }
        }

        private void HandleBButton()
        {
            if (DidButtonStartBeingPressed(HandSide.Right, Button.Secondary))
            {
                playerCharacterReference.EndGame();
                SpellMenu.Instance.ToggleSpellMenu();
            }
        }

        private void CheckInput()
        {
            HandleXButton();
            HandleYButton();
            HandleAButton();
            HandleBButton();

            if (DidButtonStartBeingPressed(HandSide.Right, Button.Stick))
            {
                Camera.main.GetComponent<PlayerCharacterTrackedPoseDriver>().ResetStartingPosition();
            }

            CheckGripButtonInput(HandSide.Left);
            CheckGripButtonInput(HandSide.Right);

            CheckSwitchGripInput(HandSide.Left);
            CheckSwitchGripInput(HandSide.Right);

            CheckMovementInput();
            CheckSpellInput(HandSide.Left);
            CheckSpellInput(HandSide.Right);
        }

        private bool DidButtonStartBeingPressed(HandSide handSide, Button button)
        {
            return isPressedByButtonByHandSide[handSide][button] && 
                !wasPressedByButtonByHandSide[handSide][button];
        }

        private bool IsButtonBeingHeld(HandSide handSide, Button button)
        {
            return isPressedByButtonByHandSide[handSide][button] && 
                wasPressedByButtonByHandSide[handSide][button];
        }

        private bool WasButtonReleased(HandSide handSide, Button button)
        {
            return !isPressedByButtonByHandSide[handSide][button] &&
                wasPressedByButtonByHandSide[handSide][button] &&
                !isReleaseNeutralizedByButtonByHandSide[handSide][button];
        }

        private void CheckSwitchGripInput(HandSide handSide)
        {
            if (IsButtonBeingHeld(handSide, Button.Grip) && DidButtonStartBeingPressed(handSide, Button.Trigger) && playerItemEquipper.IsEquipped(handSide))
            {
                ControllerLink.Get(handSide).SwitchGrip();
                isReleaseNeutralizedByButtonByHandSide[handSide][Button.Grip] = true;
            }
        }

        private void CheckGripButtonInput(HandSide handSide)
        {
            if (playerItemEquipper.IsEquipped(handSide))
            {
                if (WasButtonReleased(handSide, Button.Grip))
                {
                    Controllers.SendHapticImpulse(handSide, 0.25f, 0.1f);
                    playerItemEquipper.UnequipItem(handSide);
                }
            }
            else if (DidButtonStartBeingPressed(handSide, Button.Grip) && playerItemEquipper.TryEquipDetectedItem(handSide))
            {
                isReleaseNeutralizedByButtonByHandSide[handSide][Button.Grip] = true;
            }
            else
            {
                var controller = handSide.Equals(HandSide.Left) ? leftController : rightController;
                controller.TryGetFeatureValue(CommonUsages.grip, out var gripAmount);
                PlayerCharacterReference.Instance.playerAvatar?.SetStaticGrip(handSide, gripAmount);
            }
        }

        private void CheckMovementInput()
        {
            leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out var leftAxis);
            rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out var rightAxis);

            playerCharacterMovementController.SetMovementInput(leftAxis);
            playerCharacterMovementController.SetRotationInput(rightAxis.x);
        }

        private void CheckSpellInput(HandSide handSide)
        {
            if (IsButtonBeingHeld(handSide, Button.Grip))
                return;

            if (DidButtonStartBeingPressed(handSide, Button.Trigger))
                playerSpellCaster.HandleButtonPressed(handSide);

            if (IsButtonBeingHeld(handSide, Button.Trigger))
                playerSpellCaster.ChannelSpell(handSide);

            if (WasButtonReleased(handSide, Button.Trigger))
                playerSpellCaster.HandleButtonReleased(handSide);
        }

        private enum Button
        {
            Primary,
            Secondary,
            Trigger,
            Grip,
            Stick
        }
    }
}