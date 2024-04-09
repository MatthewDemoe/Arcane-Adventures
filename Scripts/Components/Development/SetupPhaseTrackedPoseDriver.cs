using System;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    [Serializable]
    public class SetupPhaseTrackedPoseDriver : TrackedPoseDriver
    {
        [SerializeField] private Transform controllers;

        protected override void SetLocalTransform(Vector3 newPosition, Quaternion newRotation)
        {
            transform.localRotation = newRotation;
            transform.localPosition = new Vector3(0, newPosition.y, 0);
            controllers.localPosition = -new Vector3(newPosition.x, 0, newPosition.z);
        }
    }
}