using TMPro;
using UnityEngine;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class EquipmentSetUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI equipmentSetName;
        [SerializeField] TextMeshProUGUI equipmentSetDescription;
        [SerializeField] Button leftArrow;
        [SerializeField] Button rightArrow;

        private void Start()
        {
            StatAssignPage statAssignPage = GetComponentInParent<StatAssignPage>();

            leftArrow.onClick.AddListener(() => statAssignPage.ChangeEquipmentSet(-1));
            rightArrow.onClick.AddListener(() => statAssignPage.ChangeEquipmentSet(1));
        }

        public void InitializeWithEquipmentSet(EquipmentSet equipmentSet)
        {
            equipmentSetName.text = equipmentSet.name;
            equipmentSetDescription.text = equipmentSet.contentDescription;
        }
    }
}