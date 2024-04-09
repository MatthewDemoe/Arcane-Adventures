using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    public static class ItemCache
    {
        private static Dictionary<Item, GameObject> CachedPrefabsByItem;
        private static Dictionary<Item, GameObject> PrefabsByItem
        {
            get
            {
                if (CachedPrefabsByItem == null)
                {
                    CachePrefabsByItem();
                }

                return CachedPrefabsByItem;
            }
        }

        public static Item GetItem(string name)
        {
            return PrefabsByItem.Keys.Single(item => item.name.Equals(name));
        }

        public static GameObject GetPrefab(string name)
        {
            var item = GetItem(name);

            return PrefabsByItem[item];
        }

        private static void CachePrefabsByItem()
        {
            var itemAssets = ItemAsset.LoadAll();
            var prefabsByItem = new Dictionary<Item, GameObject>();

            foreach (var itemAsset in itemAssets)
            {
                switch (itemAsset.itemType)
                {
                    case Identifiers.ItemType.NotSet:
                        Debug.LogWarning($"Item type of item \"itemAsset.name\" not set - not loading.");
                        continue;
                    case Identifiers.ItemType.Generic:
                        prefabsByItem.Add(new HandHeldItem(itemAsset), itemAsset.prefab);
                        continue;
                    case Identifiers.ItemType.Weapon:
                        prefabsByItem.Add(new Weapon(itemAsset), itemAsset.prefab);
                        continue;
                    default: throw new NotImplementedException();
                }
            }

            CachedPrefabsByItem = prefabsByItem;
        }

        public static class ItemNames
        {
            public const string Dagger = nameof(Dagger);
            public const string Fist = nameof(Fist);
            public const string OgreFist = nameof(OgreFist);
            public const string Shield = nameof(Shield);
            public const string Shortsword = nameof(Shortsword);
            public const string Spear = nameof(Spear);
            public const string Staff = nameof(Staff);
            public const string Tree = nameof(Tree);
            public const string Wand = nameof(Wand);
            public const string Warhammer = nameof(Warhammer);
        }
    }
}