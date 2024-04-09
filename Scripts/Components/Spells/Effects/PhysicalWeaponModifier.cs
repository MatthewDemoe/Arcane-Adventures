using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public abstract class PhysicalWeaponModifier : MonoBehaviour, ISpellReferencer
    {
        protected GameObject leftHandWeapon = null;

        protected GameObject rightHandWeapon = null;

        public PhysicalSpell physicalSpell { get; set; }
        ItemEquipper.ItemEquipper itemEquipper;

        private void Initialize()
        {         
            itemEquipper = physicalSpell.spellCaster.GetComponent<ItemEquipper.ItemEquipper>();

            itemEquipper.OnEquipItem.AddListener(ModifyWeapon);
            itemEquipper.OnUnequipItem.AddListener(ResetWeapon);
        }

        public void ApplyModificationToWeapons()
        {
            GetEquippedWeapons();

            if (leftHandWeapon != null)
                ModifyWeapon(leftHandWeapon);

            if (rightHandWeapon != null)
                ModifyWeapon(rightHandWeapon);
        }

        public void RemoveModificationFromWeapons()
        {
            if (leftHandWeapon != null)
                ResetWeapon(leftHandWeapon);

            if (rightHandWeapon != null)
                ResetWeapon(rightHandWeapon);
        }

        protected abstract void ModifyWeapon(GameObject gameObject);

        protected abstract void ResetWeapon(GameObject gameObject);

        private void GetEquippedWeapons()
        {
            if (itemEquipper == null)
                Initialize();

            GameObject leftWeapon = itemEquipper.GetItemInHand(HandSide.Left);

            GameObject rightWeapon = itemEquipper.GetItemInHand(HandSide.Right);

            if (leftWeapon.GetComponentInChildren<MeshRenderer>() != null)
                leftHandWeapon = leftWeapon;

            if (rightWeapon.GetComponentInChildren<MeshRenderer>() != null)
                rightHandWeapon = rightWeapon;
        }

        private void OnDestroy()
        {
            itemEquipper.OnEquipItem.RemoveListener(ModifyWeapon);
            itemEquipper.OnUnequipItem.RemoveListener(ResetWeapon);
        }
    }
}