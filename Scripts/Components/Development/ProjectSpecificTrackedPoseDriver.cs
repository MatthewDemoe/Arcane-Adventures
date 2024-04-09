using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using static com.AlteredRealityLabs.ArcaneAdventures.Components.Development.SetupPhaseInputHandler;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    [Serializable]
    public class ProjectSpecificTrackedPoseDriver : TrackedPoseDriver
    {
        private const string CameraMarkerObjectName = "Camera Marker";
        
        [SerializeField] private Transform target;
        [SerializeField] private Transform controllers;
        [SerializeField] private float characterHeight;
        [SerializeField] private float lookDownYDisplacement;
        
        [Inject] protected SetupPhaseSettings setupPhaseSettings;

        public float CharacterHeight => characterHeight;
        
        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            target = transform.parent.parent.parent.FindChildRecursive(CameraMarkerObjectName);
            GetComponentInParent<CameraOffsetUpdater>().enabled = false;
        }

        protected override void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
        {
            transform.localRotation = newRotation;
            var targetPosition = target.position;
            var offsetPosition = new Vector3(targetPosition.x, targetPosition.y - characterHeight, targetPosition.z);
            transform.parent.position = offsetPosition;

            var amountLookedDown = Vector3.Dot(transform.forward, Vector3.down);
            var yDisplacement = lookDownYDisplacement * amountLookedDown;
            var baseY = newPosition.y + yDisplacement;
            var gameWorldY = GetGameWorldY(baseY, clamp: true);
            var cameraPosition = offsetPosition;
            cameraPosition.y += gameWorldY;
            transform.position = cameraPosition;

            controllers.localPosition = new Vector3(-newPosition.x, 0, -newPosition.z);
        }

        public float GetGameWorldY(float realWorldY, bool clamp)
        {
            if (setupPhaseSettings == null)
                return realWorldY;
            
            var amountLookedDown = Vector3.Dot(transform.forward, Vector3.down);
            var yDisplacement = lookDownYDisplacement * amountLookedDown;
            var baseY = realWorldY + yDisplacement;
            
            var realWorldYPercentageDecimal = clamp?
                Mathf.InverseLerp(0, setupPhaseSettings.height, baseY) :
                InverseLerpUnclamped(0, setupPhaseSettings.height, baseY);
            var gameWorldY = clamp ? Mathf.Lerp(0, characterHeight, realWorldYPercentageDecimal) : 
                Mathf.LerpUnclamped(0, setupPhaseSettings.height, realWorldYPercentageDecimal);

            return gameWorldY;
        }
     
        private static float InverseLerpUnclamped(float a, float b, float value) => (double) a != (double) b ? (float) (((double) value - (double) a) / ((double) b - (double) a)) : 0.0f;
    }
}