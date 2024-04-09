using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Equipment Set", order = 3)]
    public class EquipmentSetAsset : ScriptableObject
    {
        public string setName;
        public string description;
        public ItemAsset leftHandItem;
        public ItemAsset rightHandItem;

        public EquipmentSet ToEquipmentSet() => new EquipmentSet(setName, description, GetWeapon(leftHandItem), GetWeapon(rightHandItem));

        private Weapon GetWeapon(ItemAsset itemAsset)
        {
            if (itemAsset == null || !itemAsset.itemType.Equals(ItemType.Weapon))
            {
                return null;
            }

            return ItemCache.GetItem(itemAsset.name) as Weapon;
        }

        public static EquipmentSetAsset[] LoadAll() => Resources.LoadAll<EquipmentSetAsset>($"{nameof(ScriptableObject)}s/{nameof(EquipmentSet)}s/");
    }
}