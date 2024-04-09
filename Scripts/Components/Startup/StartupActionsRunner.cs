using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.SaveFiles;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using System;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using SetupPhaseSettings = com.AlteredRealityLabs.ArcaneAdventures.Components.Development.SetupPhaseInputHandler.SetupPhaseSettings;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Startup
{
    public class StartupActionsRunner : MonoBehaviour
    {
        private static bool FirstRun = true;
        private static bool IsShadowRealmScene => SceneManager.GetActiveScene().name.Equals(nameof(GameScene.ShadowRealm));
        private static bool IsTestScene => SceneManager.GetActiveScene().name.Contains("TestScene");

        [SerializeField] private SetupPhaseSettings.Mode mode;
        [SerializeField] private SaveFileSelection saveFileSelection;

        [Header("Puppet Master Settings")]
        [SerializeField] private float height;
        [SerializeField] private float wingspan;
        
        [Header("Prefabs")]
        [SerializeField] private GameObject xrPrefab;
        [SerializeField] private GameObject fullScreenCanvasPrefab;
        [SerializeField] private GameObject controllerLinkPrefab;
        [SerializeField] private GameObject debugMenuPrefab;
        [SerializeField] private GameObject debugBarPrefab;

        private GameObject xrContainer;
        
        private void Awake()
        {
            if (!FirstRun)
            {
                Destroy(this);
                return;
            }

            FirstRun = false;
            DontDestroyOnLoad(gameObject);

            if (!InjectorContainer.Injector.TryGetInstance<SetupPhaseSettings>(out var setupPhaseSettings))
                setupPhaseSettings = StoreSetupPhaseSettings();

            SetupDependencyInjection();
            SetupRequiredGameObjects();

            var isEagleMode = setupPhaseSettings.mode.Equals(SetupPhaseSettings.Mode.Eagle);
            
            if (isEagleMode)
                EagleModeController.Activate(xrContainer);
            else if (IsShadowRealmScene)
                CreatureBuilder.Build(new PlayerGhost());
            else
                LoadFirstAvailableSaveFile();
        }

        private SetupPhaseSettings StoreSetupPhaseSettings()
        {
            //TODO: Use active height and wingspan instead.
            var setupPhaseSettings = new SetupPhaseSettings();
            setupPhaseSettings.mode = mode;
            setupPhaseSettings.height = height;
            setupPhaseSettings.wingspan = wingspan;

            InjectorContainer.Injector.Bind(setupPhaseSettings);

            return setupPhaseSettings;
        }
        
        private void SetupRequiredGameObjects()
        {
            xrContainer = XRReferences.Exists ? XRReferences.Instance.gameObject : Instantiate(xrPrefab);
            
            InstantiateFullScreenCanvas();
            InstantiateControllerLink(HandSide.Right);
            InstantiateControllerLink(HandSide.Left);
            Instantiate(debugMenuPrefab);
            Instantiate(debugBarPrefab);
        }

        private void InstantiateFullScreenCanvas()
        {
            var fullScreenCanvas = Instantiate(fullScreenCanvasPrefab);
            var uiCamera = Camera.main.GetUniversalAdditionalCameraData().cameraStack.Single();
            fullScreenCanvas.GetComponent<Canvas>().worldCamera = uiCamera;
        }

        private void InstantiateControllerLink(HandSide handSide)
        {
            Instantiate(controllerLinkPrefab)
                .GetComponent<ControllerLink>()
                .SetHandSide(handSide);
        }

        private void SetupDependencyInjection()
        {
            InjectorContainer.Injector.Bind<ICombatSystem>(new CombatSystem());
            InjectorContainer.Injector.Bind(new InGameMaterialCollisionProcessor());
            InjectorContainer.Injector.Bind(new CreatureTracker());
            InjectorContainer.Injector.Bind(new PhysicalWeaponTracker());
            InjectorContainer.Injector.Bind(new ImpactSoundCache());
        }

        private void LoadFirstAvailableSaveFile()
        {               
            var saveFile = IsTestScene ? FindFirstAvailableSaveFile() : SaveFile.Load(saveFileSelection.slotNumber);

            if (saveFile == null)
            {
                throw new Exception("Save file not found.");
            }

            UnityEngine.Debug.Log($"Save file in slot number {saveFile.slotNumber} loaded.");

            StartCoroutine(CreatureBuilder.GetAsyncBuildCoroutine(saveFile.playerCharacter));
        }

        private SaveFile FindFirstAvailableSaveFile()
        {
            SaveFile saveFile;

            for (int i = 0; i < 4; i++)
            {
                saveFile = SaveFile.Load(i);

                if (saveFile != null)
                    return saveFile;
            }

            return null;
        }
    }
}