using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Player;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using UMA.CharacterSystem;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using Injection;
using UnityEditor.XR.LegacyInputHelpers;
using UnityEngine.InputSystem;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences
{
    public class PlayerCharacterReference : CreatureReference, IInjectable
    {
        public static PlayerCharacterReference PreviousInstance { get; private set; }
        public static PlayerCharacterReference Instance { get; private set; }

        protected override Type creatureType { get { return typeof(PlayerCharacter); } }

        public PlayerItemEquipper playerItemEquipper { get; private set; }
        public PlayerAvatar playerAvatar { get; private set; }
        public bool isGhost => creature.race.Equals(Identifiers.Race.Ghost);
        public HandTransformUpdater handTransformUpdater { get; private set; }

        public Animator avatarAnimator { get; private set; }

        private GameScene currentGameScene => (GameScene)SceneManager.GetActiveScene().buildIndex;

        public Transform headTransform => playerAvatar.headTransform;

        public bool isUsingPuppetMaster { get; private set; }
        
        protected override void Awake()
        {
            PreviousInstance = Instance;
            Instance = this;
            InjectorContainer.Injector.Bind(this);

            playerItemEquipper = GetComponent<PlayerItemEquipper>();
            playerAvatar = GetComponent<PlayerAvatar>();
            handTransformUpdater = GetComponent<HandTransformUpdater>();
            isUsingPuppetMaster = InjectorContainer.Injector.TryGetInstance<SetupPhaseInputHandler.SetupPhaseSettings>(out var setupPhaseSettings) && setupPhaseSettings.mode.Equals(SetupPhaseInputHandler.SetupPhaseSettings.Mode.PuppetMaster);
            
            if (!isUsingPuppetMaster)
                avatarAnimator = playerAvatar.GetComponentInChildren<Animator>();

            ControllerLink.Left.SetHandSide(HandSide.Left);
            ControllerLink.Right.SetHandSide(HandSide.Right);
        }

        protected override void Start()
        {
            base.Start();

            InitializePlayerCharacter();

            if (!isGhost)
            {
                InstantiateEquippedWeapons();
            }

            if (!isUsingPuppetMaster)
            {
                var poseDriver = Camera.main.transform.parent.GetComponentInChildren<PlayerCharacterTrackedPoseDriver>(includeInactive: true);
                playerAvatar.OnCharacterUpdated.AddListener(poseDriver.ResetStartingPosition);                
            }
        }

        protected void InstantiateEquippedWeapons()
        {
            var character = creature as Character;

            if (character.leftHandItem is HandHeldItem)
                playerItemEquipper.InstantiateEquippedItem(character.leftHandItem, HandSide.Left);

            if (character.rightHandItem is HandHeldItem)
                playerItemEquipper.InstantiateEquippedItem(character.rightHandItem, HandSide.Right);
        }

        private void InitializePlayerCharacter()
        {
            if (isGhost)
            {
                Destroy(SpellMenu.Instance.gameObject);
                GetComponentsInChildren<ResourceBar>().ToList().ForEach((bar) => bar.gameObject.SetActive(false));
            }
            else if (!isUsingPuppetMaster)
            {
                GetComponentInChildren<DynamicCharacterAvatar>().enabled = true;
                gameObject.AddComponent<ControllerLinkJointUpdater>();
            }

            GhostHand.Set(on: isGhost);

            XRReferences.Instance.transform.parent = isGhost || !isUsingPuppetMaster ? transform : transform.parent;
            XRReferences.Instance.transform.localRotation = Quaternion.Euler(0, 0, 0);

            var useProjectSpecificTrackedPoseDriver = !isGhost && isUsingPuppetMaster;
            Camera.main.GetComponent<PlayerCharacterTrackedPoseDriver>().enabled = !useProjectSpecificTrackedPoseDriver;
            Camera.main.GetComponent<ProjectSpecificTrackedPoseDriver>().enabled = useProjectSpecificTrackedPoseDriver;
            Camera.main.transform.parent.GetComponent<CameraOffsetUpdater>().enabled = !useProjectSpecificTrackedPoseDriver;


            if (!(PreviousInstance is null))
            {
                transform.forward = PreviousInstance.transform.forward;
                Destroy(PreviousInstance.gameObject);
            }
            
        }

        public void EndGame()//TODO: Move to conceptually more appropriate place?
        {
            //TODO: Resurrect in more appropriate way.
        }

        public void Die()
        {
            playerItemEquipper.UnequipItem(HandSide.Left);
            playerItemEquipper.UnequipItem(HandSide.Right);
        }
    }
}