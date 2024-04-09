using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using Injection;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class SetupPhaseInputHandler : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI tPoseRecordingText;
        [SerializeField] private UnityEngine.UI.Button newButton;
        [SerializeField] private UnityEngine.UI.Button classicButton;
        [SerializeField] private GameObject tPoseScreen;
        [SerializeField] private GameObject selectModeScreen;
        [SerializeField] private GameObject[] destroyAtEndOfScene;

        private SetupPhaseState state = SetupPhaseState.WaitingForTPose;
        private SetupPhaseSettings setupPhaseSettings;
        [Inject] private OculusControllerInputReader oculusControllerInputReader;

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            oculusControllerInputReader.OnUpdated.AddListener(ProcessInput);
            
            setupPhaseSettings = new SetupPhaseSettings();
            newButton.onClick.AddListener(() => { SelectMode(SetupPhaseSettings.Mode.PuppetMaster); });
            classicButton.onClick.AddListener(() => { SelectMode(SetupPhaseSettings.Mode.UMA); });
        }

        private void ProcessInput()
        {
            if (state.Equals(SetupPhaseState.WaitingForTPose) && 
                oculusControllerInputReader.IsButtonBeingHeld(OculusControllerInputReader.Controller.Right, OculusControllerInputReader.Button.Trigger) && 
                oculusControllerInputReader.IsButtonBeingHeld(OculusControllerInputReader.Controller.Left, OculusControllerInputReader.Button.Trigger))
            {
                SaveTPoseValues();
            }
        }
        
        private void SelectMode(SetupPhaseSettings.Mode mode)
        {
            setupPhaseSettings.mode = mode;
            selectModeScreen.SetActive(false);
            InjectorContainer.Injector.Bind(setupPhaseSettings);

            foreach (var gameObjectToDestroy in destroyAtEndOfScene)
                Destroy(gameObjectToDestroy);

            SceneLoader.Load(GameScene.ShadowRealm);
        }

        private void SaveTPoseValues()
        {
            setupPhaseSettings.height = Camera.main.transform.localPosition.y;

            var leftControllerPosition = oculusControllerInputReader.GetControllerPosition(OculusControllerInputReader.Controller.Left);
            var rightControllerPosition = oculusControllerInputReader.GetControllerPosition(OculusControllerInputReader.Controller.Right);
            setupPhaseSettings.wingspan = Math.Abs(leftControllerPosition.x - rightControllerPosition.x);

            tPoseRecordingText.text = $"Successfully recorded:\n\tHeight: {(setupPhaseSettings.height * 100) + 6}cm\n\tWingspan: {setupPhaseSettings.wingspan * 100}cm";

            state = SetupPhaseState.WaitingForModeSelection;

            tPoseScreen.SetActive(false);
            selectModeScreen.SetActive(true);
        }

        private enum SetupPhaseState
        {
            WaitingForTPose,
            WaitingForModeSelection
        }

        public class SetupPhaseSettings : IInjectable
        {
            public float height;
            public float wingspan;
            public Mode mode;

            public enum Mode
            {
                PuppetMaster,
                UMA,
                Eagle
            }
        }

        private void OnDestroy()
        {
            oculusControllerInputReader.OnUpdated.RemoveListener(ProcessInput);
            Destroy(Camera.main.GetComponent<SetupPhaseTrackedPoseDriver>());
        }
    }
}