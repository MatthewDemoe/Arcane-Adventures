using System.Collections;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Materials;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{
    public class WeaponSetToggler : MonoBehaviour
    {
        NPCItemEquipper itemEquipper;

        [SerializeField]
        ItemSlot leftSlot;

        [SerializeField]
        ItemSlot rightSlot;

        [SerializeField]
        float dissolveDuration = 1.0f;

        public bool isTransitioning { get; private set; } = false;

        private void Start()
        {
            itemEquipper = GetComponent<NPCItemEquipper>();
        }

        public void EnableWeapons(EquipmentSet equipmentSet)
        {
            StopAllCoroutines();
            StartCoroutine(DissolveRoutine(equipmentSet));
        }

        IEnumerator DissolveRoutine(EquipmentSet equipmentSet)
        {
            isTransitioning = true;

            DissolveWeaponInSlot(leftSlot);
            DissolveWeaponInSlot(rightSlot);

            yield return new WaitForSeconds(dissolveDuration);

            AddNewWeaponToSlot(HandSide.Left, equipmentSet);
            AddNewWeaponToSlot(HandSide.Right, equipmentSet);

            isTransitioning = false;
        }
        private void DissolveWeaponInSlot(ItemSlot itemSlot)
        {
            if (itemSlot.item != null)
            {
                MeshRenderer weaponMesh = itemSlot.item.GetComponentInChildren<MeshRenderer>();

                if (weaponMesh.TryGetComponent(out DissolveEffect dissolveEffect))
                    dissolveEffect.StartDissolveFromCurrent(1.0f, 1.0f);

                else
                    DissolveEffect.Dissolve(weaponMesh, 0.0f, 1.0f, dissolveDuration);
            }
        }

        private void AddNewWeaponToSlot(HandSide handSide, EquipmentSet equipmentSet)
        {
            ItemSlot itemSlot = handSide == HandSide.Left ? leftSlot : rightSlot;

            if (itemSlot.item != null)          
                Destroy(itemSlot.item);

            var item = (handSide == HandSide.Left) ? equipmentSet.leftHandItem : equipmentSet.rightHandItem;
            
            if (item != null)
            {
                itemEquipper.InstantiateEquippedItem(item, handSide);

                DissolveEffect.Dissolve(itemSlot.item.GetComponentInChildren<MeshRenderer>(), 1.0f, 0.0f, dissolveDuration);
            }
        }
    }
}
