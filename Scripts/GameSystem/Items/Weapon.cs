using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System.Collections.ObjectModel;
using System.Linq;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items
{
    public class Weapon : HandHeldItem
    {
        public const int MinimumWeightCategory = 0;
        public const int MaximumWeightCategory = 3;

        private readonly int baseWeightCategory;
        public int weightAdjustment = 0;
        public int penetrationClassAdjustment = 0;
        public readonly float spellDamageMultiplier;

        public int weightCategory => Math.Min(Math.Max(baseWeightCategory + weightAdjustment, MinimumWeightCategory), MaximumWeightCategory);

        public readonly ReadOnlyCollection<WeaponSurface> weaponSurfaces;
        public readonly bool destroyOnImpact;

        public readonly Identifiers.ItemType itemType;
        public readonly bool isArcaneFocus;

        public int GetDamage()
        {
            int damage = 0;

            weaponSurfaces.ToList().ForEach((weaponSurface) => 
            {
                int surfaceDamage = 0;

                surfaceDamage += weaponSurface.damageByStrikeType[StrikeType.Incomplete];
                surfaceDamage += weaponSurface.damageByStrikeType[StrikeType.Imperfect];
                surfaceDamage += weaponSurface.damageByStrikeType[StrikeType.Perfect];

                damage += surfaceDamage / 3;
            });

            damage /= weaponSurfaces.Count;

            return damage;
        }

        public Weapon(ItemAsset itemAsset) : base(itemAsset)
        {
            baseWeightCategory = itemAsset.weightCategory;
            weaponSurfaces = itemAsset.attackableSurfaces.Select(asset => asset.ToWeaponSurface(this)).ToList().AsReadOnly();
            itemType = itemAsset.itemType;

            isArcaneFocus = itemAsset.isArcaneFocus;
            destroyOnImpact = itemAsset.destroyOnImpact;
            spellDamageMultiplier = itemAsset.spellDamageMultiplier;
        }
    }
}