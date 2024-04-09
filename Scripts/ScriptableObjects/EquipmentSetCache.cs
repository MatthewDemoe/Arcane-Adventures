using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System.Collections.Generic;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    public static class EquipmentSetCache
    {
        private static List<EquipmentSet> CachedEquipmentSets;
        public static List<EquipmentSet> EquipmentSets
        {
            get
            {
                if (CachedEquipmentSets == null)
                {
                    CachedEquipmentSets = EquipmentSetAsset.LoadAll().Select(asset => asset.ToEquipmentSet()).ToList();
                }

                return CachedEquipmentSets;
            }
        }
    }
}