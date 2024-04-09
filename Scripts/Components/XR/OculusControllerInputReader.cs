using System;
using System.Collections.Generic;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using Injection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR
{
    public class OculusControllerInputReader : MonoBehaviour, IInjectable
    {
        private static readonly IReadOnlyList<Controller> Controllers = EnumExtensions.GetValues<Controller>();
        private static readonly IReadOnlyList<Button> Buttons = EnumExtensions.GetValues<Button>();

        private readonly Dictionary<Controller, Dictionary<Button, bool>> isPressedByButtonByController = new Dictionary<Controller, Dictionary<Button, bool>>();
        private readonly Dictionary<Controller, Dictionary<Button, bool>> wasPressedByButtonByController = new Dictionary<Controller, Dictionary<Button, bool>>();
        private readonly Dictionary<Controller, Dictionary<Button, bool>> isReleaseNeutralizedByButtonByController = new Dictionary<Controller, Dictionary<Button, bool>>();
        private readonly Dictionary<Controller, InputDevice> inputDeviceByController = new Dictionary<Controller, InputDevice>();
        private readonly Dictionary<Controller, Vector2> stickInputByController = new Dictionary<Controller, Vector2>();

        [SerializeField] private UnityEvent _onUpdated;
        public UnityEvent OnUpdated => _onUpdated;
        
        private void Awake()
        {
            PopulateDictionaries();
            InjectorContainer.Injector.Bind(this);
        }

        private void PopulateDictionaries()
        {
            foreach (var controller in Controllers)
            {
                isPressedByButtonByController.Add(controller, new Dictionary<Button, bool>());
                wasPressedByButtonByController.Add(controller, new Dictionary<Button, bool>());
                isReleaseNeutralizedByButtonByController.Add(controller, new Dictionary<Button, bool>());
                stickInputByController.Add(controller, Vector2.zero);

                foreach (var button in Buttons)
                {
                    isPressedByButtonByController[controller].Add(button, false);
                    wasPressedByButtonByController[controller].Add(button, false);
                    isReleaseNeutralizedByButtonByController[controller].Add(button, false);
                }
            }
        }

        private void Update()
        {
            foreach (var controller in Controllers)
            {
                RefreshController(controller);
                UpdateStickInput(controller);
                UpdateButtonStates(controller);
            }
            
            _onUpdated.Invoke();
        }

        private void UpdateStickInput(Controller controller)
        {
            inputDeviceByController[controller].TryGetFeatureValue(CommonUsages.primary2DAxis, out var stickInput);
            stickInputByController[controller] = stickInput;
        }

        private void UpdateButtonStates(Controller controller)
        {
            foreach (var button in Buttons)
            {
                UpdateButtonState(controller, button);
            }
        }

        private void UpdateButtonState(Controller controller, Button button)
        {
            wasPressedByButtonByController[controller][button] = isPressedByButtonByController[controller][button];
            inputDeviceByController[controller].TryGetFeatureValue(GetInputFeatureUsage(button), out var isPressed);
            isPressedByButtonByController[controller][button] = isPressed;

            if (!isPressedByButtonByController[controller][button] && 
                !wasPressedByButtonByController[controller][button] &&
                isReleaseNeutralizedByButtonByController[controller][button])
            {
                isReleaseNeutralizedByButtonByController[controller][button] = false;
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

        public bool DidButtonStartBeingPressed(Controller controller, Button button) =>
            isPressedByButtonByController[controller][button] && 
            !wasPressedByButtonByController[controller][button];

        public bool IsButtonBeingHeld(Controller controller, Button button) =>
            isPressedByButtonByController[controller][button] && 
            wasPressedByButtonByController[controller][button];

        public bool WasButtonReleased(Controller controller, Button button) =>
            !isPressedByButtonByController[controller][button] &&
            wasPressedByButtonByController[controller][button] &&
            !isReleaseNeutralizedByButtonByController[controller][button];

        public void SetReleaseNeutralized(Controller controller, Button button)
        {
            isReleaseNeutralizedByButtonByController[controller][button] = true;
        }

        public InputDevice GetInputDevice(Controller controller) => inputDeviceByController[controller];
        
        public Vector2 GetStickInput(Controller controller) => stickInputByController[controller];

        public Vector3 GetControllerPosition(Controller controller)
        {
            inputDeviceByController[controller].TryGetFeatureValue(CommonUsages.devicePosition, out var position);

            return position;
        }
            
        private void RefreshController(Controller controller)
        {
            if (inputDeviceByController.ContainsKey(controller) && inputDeviceByController[controller].isValid)
                return;

            var inputDeviceCharacteristics = controller.Equals(Controller.Left)
                ? InputDeviceCharacteristics.Left
                : InputDeviceCharacteristics.Right;
                
            inputDeviceByController[controller] = GetInputDevice(inputDeviceCharacteristics);
        }
        
        private static InputDevice GetInputDevice(InputDeviceCharacteristics inputDeviceCharacteristics)
        {
            var inputDevices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(inputDeviceCharacteristics, inputDevices);
            var validInputDevices = inputDevices.Where(inputDevice => inputDevice.isValid);

            if (validInputDevices.Count() > 1)
            {
                UnityEngine.Debug.LogWarning("Multiple input devices qualified for queried characteristic");
            }

            return validInputDevices.FirstOrDefault();
        }

        public enum Controller
        {
            Left,
            Right
        }

        public enum Button
        {
            Primary,
            Secondary,
            Trigger,
            Grip,
            Stick
        }
    }
}