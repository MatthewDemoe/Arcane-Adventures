using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Controls
{
    public static class Controllers
    {
        private static float LeftHapticFinishTime = 0;
        private static float RightHapticFinishTime = 0;
        private static float LeftHapticLastAmplitude;
        private static float RightHapticLastAmplitude;

        private static InputDevice LeftController;
        private static InputDevice RightController;

        public static InputDevice GetLeftController() => GetController(HandSide.Left);
        public static InputDevice GetRightController() => GetController(HandSide.Right);
        public static InputDevice GetHeadset() => GetInputDevice(InputDeviceCharacteristics.HeadMounted);

        private static InputDevice GetController(HandSide handSide)
        {
            if (handSide.Equals(HandSide.Left))
            {
                if (!LeftController.isValid)
                {
                    LeftController = GetInputDevice(InputDeviceCharacteristics.Left);
                }

                return LeftController;
            }
            else
            {
                if (!RightController.isValid)
                {
                    RightController = GetInputDevice(InputDeviceCharacteristics.Right);
                }

                return RightController;
            }
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

        public static void SendHapticImpulse(HandSide handSide, float amplitude, float duration)
        {
            var controller = GetController(handSide);

            if (!controller.TryGetHapticCapabilities(out var capabilities) || !capabilities.supportsImpulse)
            {
                UnityEngine.Debug.LogWarning("Controller does not have haptic capabilities");
                return;
            }

            if (handSide.Equals(HandSide.Left))
            {
                if (LeftHapticFinishTime < Time.time || amplitude >= LeftHapticLastAmplitude)
                {
                    LeftHapticLastAmplitude = amplitude;
                    controller.SendHapticImpulse(0u, amplitude, duration);
                    LeftHapticFinishTime = Time.time + duration;
                }
            }
            else
            {
                if (RightHapticFinishTime < Time.time || amplitude >= RightHapticLastAmplitude)
                {
                    RightHapticLastAmplitude = amplitude;
                    controller.SendHapticImpulse(0u, amplitude, duration);
                    RightHapticFinishTime = Time.time + duration;
                }
            }
        }
    }
}