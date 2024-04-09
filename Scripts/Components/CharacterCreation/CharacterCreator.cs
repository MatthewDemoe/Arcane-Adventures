using System.Collections;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UMA;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.SaveFiles;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UMA.CharacterSystem;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{
    [RequireComponent(typeof(CreatureReference))]
    public class CharacterCreator : MonoBehaviour
    {
        private static CharacterCreator _instance;
        public static CharacterCreator Instance => _instance;

        public CreatureReference creatureReference { get; private set; }
        private SaveFile _saveFile;

        [SerializeField]
        GameObject shadowCreatureInstance;

        [SerializeField]
        GameObject umaCreatureInstance;

        SkinnedMeshRenderer meshRenderer;

        public UMACreatureAssembler creatureAssembler { get; private set; }

        [SerializeField]
        GameObject swirlingCloud;

        [SerializeField]
        ItemSlot leftWeaponSlot;

        [SerializeField]
        ItemSlot rightWeaponSlot;

        private bool _saved = false;

        const float SwapTime = 1.5f;
        const float CloudMaxScale = 3.0f;
        float swapTimer = 0.0f;

        public bool swappingRace { get; private set; } = false;

        private void Awake()
        {
            _instance = this;
            creatureReference = GetComponent<CreatureReference>();

            InitializeUMA();
        }

        public void SetSaveFile(SaveFile newSaveFile)
        {
            _saveFile = newSaveFile;
            creatureReference.creature = _saveFile.playerCharacter;
            PlayerCharacterReference.Instance.gameObject.GetComponent<CreatureReference>().creature = creatureReference.creature;
        }

        public void Save()
        {
            if (_saved)
                return;

            _saved = true;

            var newPlayerCharacter = creatureReference.creature as PlayerCharacter;
            newPlayerCharacter.leftHandItem = leftWeaponSlot.item == null ? null : leftWeaponSlot.item.GetComponent<PhysicalWeapon>().weapon;
            newPlayerCharacter.rightHandItem = rightWeaponSlot.item == null ? null : rightWeaponSlot.item.GetComponent<PhysicalWeapon>().weapon;

            SaveFile.Save(_saveFile.slotNumber, newPlayerCharacter);
            CreatureBuilder.Build(SaveFile.Load(_saveFile.slotNumber).playerCharacter);
            Destroy(gameObject);
        }

        public void UpdateClass(CharacterClassAttributes newClass)
        {
            PlayerCharacter oldCreature = creatureReference.creature as PlayerCharacter;
            creatureReference.creature = new PlayerCharacter(name: oldCreature.playerName,
                new Stats(
                    strength: oldCreature.stats.baseStrength, 
                    vitality: oldCreature.stats.baseVitality, 
                    spirit: oldCreature.stats.baseSpirit,
                    level: oldCreature.stats.level,
                    characterClass: newClass
                ),                
                race: oldCreature.race, 
                gender: oldCreature.gender, 
                characterClass: newClass.identifier,
                leftHandItem: oldCreature.leftHandItem, 
                rightHandItem: oldCreature.rightHandItem);
        }

        public void UpdateRace(Race newRace)
        {
            if (swappingRace)
                return;

            PlayerCharacter oldCreature = creatureReference.creature as PlayerCharacter;

            creatureReference.creature = new PlayerCharacter(name: oldCreature.playerName,
                new Stats(
                    strength: oldCreature.stats.baseStrength, 
                    vitality: oldCreature.stats.baseVitality, 
                    spirit: oldCreature.stats.baseSpirit,
                    level: oldCreature.stats.level,
                    characterClass: oldCreature.characterClass.classAttributes
                ),
                race: newRace.identifier,
                gender: oldCreature.gender, 
                characterClass: oldCreature.characterClass.identifier,
                leftHandItem: oldCreature.leftHandItem, 
                rightHandItem: oldCreature.rightHandItem);

            StartCoroutine(nameof(SwapRaceModel));
        }

        IEnumerator SwapRaceModel()
        {
            swappingRace = true;

            StartCoroutine(ScaleCloud(0.0f, CloudMaxScale));

            yield return new WaitUntil(() => swapTimer >= SwapTime);

            if (shadowCreatureInstance != null)
            {
                Destroy(shadowCreatureInstance);

                meshRenderer.gameObject.SetActive(true);
            }

            creatureAssembler.UpdateRaceData(); 

            StartCoroutine(ScaleCloud(CloudMaxScale, 0.0f));

            yield return new WaitUntil(() => swapTimer >= SwapTime);

            swappingRace = false;
        }

        IEnumerator ScaleCloud(float scaleFrom, float scaleTo)
        {
            swapTimer = 0.0f;

            while (swapTimer <= SwapTime)
            {
                swirlingCloud.transform.localScale = GetCloudScale(scaleFrom, scaleTo);

                yield return null;
                swapTimer += Time.deltaTime;
            }
        }

        private Vector3 GetCloudScale(float scaleFrom, float scaleTo)
        {
            float cloudScale = UtilMath.Lmap(swapTimer, 0.0f, SwapTime, 0.0f, 1.0f);
            cloudScale = UtilMath.EasingFunction.EaseOutSine(scaleFrom, scaleTo, cloudScale);

            return Vector3.one * cloudScale;
        }

        private void InitializeUMA()
        {
            creatureAssembler = umaCreatureInstance.GetComponent<UMACreatureAssembler>();
            creatureAssembler.creatureReference = creatureReference;

            DynamicCharacterAvatar creatureAvatar = umaCreatureInstance.GetComponentInChildren<DynamicCharacterAvatar>();

            creatureAvatar.CharacterCreated.AddListener((data) =>
            {
                meshRenderer = umaCreatureInstance.GetComponentInChildren<SkinnedMeshRenderer>();

                meshRenderer.gameObject.SetActive(false);
            });
        }
    }
}
