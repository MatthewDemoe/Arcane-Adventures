using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.SaveFiles;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using RayFire;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{

    public class Portal : MonoBehaviour
    {
        [SerializeField]
        int saveFileNumber = 0;

        SaveFile saveFile;

        [SerializeField]
        Transform thisPortalTeleportPoint;

        [SerializeField]
        Transform destinationPortal;

        [SerializeField]
        GameScene nextScene;

        [SerializeField]
        GameObject reflectiveMirror;

        [SerializeField]
        GameObject characterCanvas;
        [SerializeField]
        GameObject characterIcon;

        [SerializeField]
        GameObject mirrorFragments;
        RayfireRigid fragmentController;

        [SerializeField]
        GameObject deleteCharacterUI;

        private bool fragmentsInitialized => fragmentController.initialized;

        private bool isSceneTransitionTriggered = false;
        private bool isBroken = false;

        // Start is called before the first frame update
        void Start()
        {
            InitializeMirror();
        }

        private void InitializeMirror()
        {
            saveFile = SaveFile.Load(saveFileNumber);

            fragmentController = mirrorFragments.transform.GetChild(0).GetComponent<RayfireRigid>();
            ChooseWhatToDisplay();
            StartCoroutine(nameof(WaitForRayfireToInitialize));

            if (saveFile != null)
                characterIcon.GetComponent<Image>().sprite = CharacterClassUI.GetCharacterClassIconsByName()[saveFile.playerCharacter.characterClass.classAttributes.name].GetComponent<Image>().sprite;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.IsPlayerCharacter()&& !isBroken)
            {
                InteractorHeldItemDropper.DropHeldItems();

                if (saveFile == null)
                {
                    TeleportToDestinationPortal();
                }
                else
                {
                    GoToNextScene();
                }
            }

            if (other.gameObject.layer == (int)Layers.Interactables)
            {
                if (saveFile.playerCharacter.playerName.Equals(string.Empty))
                    return;

                isBroken = true;

                ActiveMirrorFragments.Instance.SetActiveMirror(mirrorFragments);
                deleteCharacterUI.SetActive(true);
                deleteCharacterUI.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                deleteCharacterUI.GetComponentInChildren<ParticleSystem>().transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

                reflectiveMirror.SetActive(false);
                characterCanvas.SetActive(false);
            }
        }

        void ChooseWhatToDisplay()
        {
            bool setToTransparent = saveFile == null;

            reflectiveMirror.SetActive(true);
            characterCanvas.SetActive(!setToTransparent);
        }

        public void TeleportToDestinationPortal()
        {
            Quaternion fromTo = Quaternion.FromToRotation(transform.forward, -destinationPortal.forward);

            destinationPortal.GetComponent<Portal>().destinationPortal = transform;
            gameObject.GetComponent<TeleportPlayer>().TeleportCharacter(destinationPortal.GetComponent<Portal>().thisPortalTeleportPoint.position, fromTo * PlayerCharacterReference.Instance.transform.forward);

            CharacterCreator.Instance.SetSaveFile(SaveFile.GetNewSaveFile(saveFileNumber));
        }

        public void GoToNextScene()
        {
            if (isSceneTransitionTriggered)
            {
                return;
            }

            isSceneTransitionTriggered = true;

            StartCoroutine(CreatureBuilder.GetAsyncBuildCoroutine(saveFile.playerCharacter));
            SceneLoader.LoadWithFadeEffect(nextScene);
        }

        public void DeleteCharacter()
        {
            SaveFile.Delete(saveFileNumber);
            saveFile = SaveFile.Load(saveFileNumber);

            ActiveMirrorFragments.Instance.StartFragmentFadeOut();
            StartFadeMirrorIn();
        }

        public void RestoreMirror()
        {
            ActiveMirrorFragments.Instance.StartFragmentFadeOut();
            StartFadeMirrorIn();
        }

        public void StartFadeMirrorIn()
        {
            StartCoroutine(FadeMirrorIn());
        }

        IEnumerator WaitForRayfireToInitialize()
        {
            while (!fragmentsInitialized)
            {
                yield return null;
            }

            mirrorFragments.SetActive(false);
        }

        IEnumerator FadeMirrorIn()
        {
            const float duration = 2.0f;

            float elapsedTime = 0.0f;

            ChooseWhatToDisplay();
            Material mirrorMaterial = reflectiveMirror.GetComponent<MeshRenderer>().material;

            Transform particleTransform = deleteCharacterUI.GetComponentInChildren<ParticleSystem>().transform;
            characterIcon.transform.localScale = Vector3.zero;

            float normalizedTime = 0.0f;
            float inverseTime = 0.0f;
            isBroken = false;


            while (elapsedTime < duration)
            {
                yield return null;

                elapsedTime += Time.deltaTime;

                normalizedTime = UtilMath.Lmap(elapsedTime, 0.0f, duration, 0.0f, 1.0f);
                inverseTime = 1.0f - normalizedTime;
                
                mirrorMaterial.SetFloat(ShaderProperties.ClipAmount, inverseTime);

                deleteCharacterUI.transform.localScale = new Vector3(inverseTime, inverseTime, inverseTime);
                particleTransform.localScale = new Vector3(inverseTime, inverseTime, inverseTime);
                characterIcon.transform.localScale = new Vector3(normalizedTime, normalizedTime, normalizedTime);
            }

            deleteCharacterUI.SetActive(false);
        }
    }
}