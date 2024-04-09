using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class StatAssignPage : BookPage
    {
        [SerializeField] TextMeshProUGUI classAndRace;
        [SerializeField] TextMeshProUGUI remainingPoints;
        [SerializeField] TextMeshProUGUI strengthText;
        [SerializeField] TextMeshProUGUI vitalityText;
        [SerializeField] TextMeshProUGUI spiritText;
        [SerializeField] TextMeshProUGUI healthPoolText;
        [SerializeField] TextMeshProUGUI spiritPoolText;
        [SerializeField] GameObject equipmentSetPrefab;
        [SerializeField] GameObject equipmentSetParent;
        [SerializeField] WeaponSetToggler weaponToggler;

        private int currentEquipmentSet = 0;

        CharacterCreator cc;

        private void Start()
        {
            cc = CharacterCreator.Instance;
            UpdateStatText();

            foreach (EquipmentSet equipmentSet in (CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).characterClass.equipmentSets)
            {
                GameObject equipmentSetInstance = Instantiate(equipmentSetPrefab, equipmentSetParent.transform);

                equipmentSetInstance.GetComponent<EquipmentSetUI>().InitializeWithEquipmentSet(equipmentSet);
                equipmentSetInstance.SetActive(false);
            }

            ChangeEquipmentSet(0);
        }

        public void UpdateStatText()
        {
            if(cc == null)
                cc = CharacterCreator.Instance;

            var raceName = Race.Get(cc.creatureReference.creature.race).name;

            classAndRace.text = $"{raceName} {(cc.creatureReference.creature as PlayerCharacter).characterClass.classAttributes.name}";
            remainingPoints.text = $"Remaining Points: {cc.creatureReference.creature.stats.remainingStatPoints}";
            strengthText.text = $"Strength: {cc.creatureReference.creature.stats.subtotalStrength}";
            vitalityText.text = $"Vitality: {cc.creatureReference.creature.stats.subtotalVitality}";
            spiritText.text = $"Spirit: {cc.creatureReference.creature.stats.subtotalSpirit}";
            healthPoolText.text = $"{cc.creatureReference.creature.stats.maxHp}";
            spiritPoolText.text = $"{cc.creatureReference.creature.stats.maxSpirit}";

        }

        public void SetCharacterName(string name)
        {
            (CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).playerName = name;
        }

        public void AdjustStrength(int amount)
        {
            cc.creatureReference.creature.stats.UseStatPoint(Stats.Stat.Strength, amount);
            UpdateStatText();
        }

        public void AdjustVitality(int amount)
        {
            cc.creatureReference.creature.stats.UseStatPoint(Stats.Stat.Vitality, amount);
            UpdateStatText();
        }

        public void AdjustSpirit(int amount)
        {
            cc.creatureReference.creature.stats.UseStatPoint(Stats.Stat.Spirit, amount);
            UpdateStatText();
        }

        public void ChangeEquipmentSet(int direction)
        {
            if (((currentEquipmentSet + direction) < 0) || ((currentEquipmentSet + direction) > equipmentSetParent.transform.childCount - 1))
                return;

            if (weaponToggler.isTransitioning)
                direction = 0;

            equipmentSetParent.transform.GetChild(currentEquipmentSet).gameObject.SetActive(false);
            currentEquipmentSet = Mathf.Clamp(currentEquipmentSet + direction, 0, equipmentSetParent.transform.childCount - 1);
            equipmentSetParent.transform.GetChild(currentEquipmentSet).gameObject.SetActive(true);

            if (weaponToggler.isTransitioning)
                return;

            weaponToggler.EnableWeapons((CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).characterClass.equipmentSets[currentEquipmentSet]);
        }
    }
}