using System;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Debug;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using UnityEngine;
using UnityEngine.InputSystem.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class EagleModeController : MonoBehaviour
    {
        [SerializeField] private float speed = 20;
        [Inject] private OculusControllerInputReader oculusControllerInputReader;

        private Transform previousParent;
        private TrackedPoseDriver previousTrackedPoseDriver;
        private TrackedPoseDriver currentTrackedPoseDriver;
        private CameraOffsetUpdater cameraOffsetUpdater;
        
        public static bool IsActive { get; private set; }
        
        public static void Activate() => Activate(XRReferences.Instance.gameObject);
        
        public static void Activate(GameObject xrContainer)
        {
            var trackedPoseDrivers = xrContainer.GetComponentsInChildren<TrackedPoseDriver>();
            
            var eagleModeController = xrContainer.AddComponent<EagleModeController>();
            eagleModeController.previousParent = xrContainer.transform.parent;
            eagleModeController.cameraOffsetUpdater = xrContainer.GetComponentInChildren<CameraOffsetUpdater>();
            eagleModeController.previousTrackedPoseDriver = trackedPoseDrivers.Single(trackedPoseDriver => trackedPoseDriver.enabled);
            eagleModeController.currentTrackedPoseDriver = trackedPoseDrivers.Single(trackedPoseDriver => !trackedPoseDriver.GetType().IsSubclassOf(typeof(TrackedPoseDriver)));
            
            eagleModeController.cameraOffsetUpdater.enabled = false;
            eagleModeController.previousTrackedPoseDriver.enabled = false;
            eagleModeController.currentTrackedPoseDriver.enabled = true;

            xrContainer.transform.parent = null;
            
            if (PlayerCharacterReference.Instance != null)
                PlayerCharacterReference.Instance.gameObject.SetActive(false);

            IsActive = true;
        }

        public static void Deactivate()
        {
            Destroy(XRReferences.Instance.gameObject.GetComponent<EagleModeController>());
            IsActive = false;
        }
        
        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            oculusControllerInputReader.OnUpdated.AddListener(ProcessInput);    
        }
        
        private void ProcessInput()
        {
            if (oculusControllerInputReader.DidButtonStartBeingPressed(OculusControllerInputReader.Controller.Left, OculusControllerInputReader.Button.Secondary))
                DebugMenu.FlipSwitch();

            var leftAxis = oculusControllerInputReader.GetStickInput(OculusControllerInputReader.Controller.Left);
            
            if (leftAxis.magnitude > 0.02f)
            {
                var input = new Vector3(leftAxis.x, 0, leftAxis.y) * Time.deltaTime * speed;
                cameraOffsetUpdater.transform.position += Camera.main.transform.TransformPoint(input) - Camera.main.transform.position;
            }
        }

        private void OnDestroy()
        {
            currentTrackedPoseDriver.enabled = false;
            previousTrackedPoseDriver.enabled = true;
            cameraOffsetUpdater.enabled = true;
            XRReferences.Instance.gameObject.transform.parent = previousParent;
            
            if (PlayerCharacterReference.Instance != null)
                PlayerCharacterReference.Instance.gameObject.SetActive(true);
            
            oculusControllerInputReader.OnUpdated.RemoveListener(ProcessInput);
        }
    }
}