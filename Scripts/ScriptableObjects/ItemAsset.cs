using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Item", order = 2)]
    public class ItemAsset : ScriptableObject
    {
        public Identifiers.ItemType itemType;

        public string itemName;

        [Required]
        [ShowAssetPreview(128, 128)]
        public GameObject prefab;

        [SerializeField][ShowIf(nameof(isArcaneFocus))]
        public BasicSpellType basicSpellType;

        [ShowIf(nameof(isWeapon))]
        [Range(0, 8)]
        public int weightCategory;


        public bool isArcaneFocus = false;
        public bool canWieldWithTwoHands = false;

        [ShowIf(nameof(isArcaneFocus))]
        public float spellDamageMultiplier = 1.0f;

        [ShowIf(nameof(isWeapon))]
        public bool destroyOnImpact;

        [ShowIf(nameof(isWeapon))]
        [ValidateInput(nameof(ValidateAttackableSurfaces), "Does not match attackable surfaces of prefab.")]
        public AttackableSurfaceAsset[] attackableSurfaces;

        [Button("Syncronize attackable surfaces with prefab")]
        private void SyncronizeAttackableSurfacesWithPrefab()
        {
            if (!IsAttackableSurfacesSameAsPrefab(this.attackableSurfaces, out var actualAttackableSurfaceAssets))
            {
                attackableSurfaces = actualAttackableSurfaceAssets;
            }
        }

        private bool isWeapon => itemType.Equals(Identifiers.ItemType.Weapon);

        public static ItemAsset[] LoadAll() => Resources.LoadAll<ItemAsset>($"{nameof(ScriptableObject)}s/{nameof(Item)}s/");

        private bool ValidateAttackableSurfaces(AttackableSurfaceAsset[] value)
        {
            return IsAttackableSurfacesSameAsPrefab(value);
        }

        private bool IsAttackableSurfacesSameAsPrefab(AttackableSurfaceAsset[] currentAttackableSurfaceAssets) => IsAttackableSurfacesSameAsPrefab(currentAttackableSurfaceAssets, out var _);

        private bool IsAttackableSurfacesSameAsPrefab(AttackableSurfaceAsset[] currentAttackableSurfaceAssets, out AttackableSurfaceAsset[] actualAttackableSurfaceAssets)
        {
            var parts = GetPhysicalWeaponParts();

            if (!parts.Any())
            {
                actualAttackableSurfaceAssets = null;
                return false;
            }

            var currentAttackableSurfaces = currentAttackableSurfaceAssets.ToList();
            var actualAttackableSurfaces = new List<AttackableSurfaceAsset>();
            var wasModified = false;

            foreach (var currentAttackableSurface in currentAttackableSurfaces)
            {
                if (parts.Any(part => part.name.Equals(currentAttackableSurface.name)) &&
                    !actualAttackableSurfaces.Any(attackableSurface => attackableSurface.name.Equals(currentAttackableSurface.name)))
                {
                    actualAttackableSurfaces.Add(currentAttackableSurface);
                }
                else wasModified = true;
            }

            foreach (var part in parts)
            {
                if (!currentAttackableSurfaces.Any(attackableSurface => attackableSurface.name.Equals(part.name)))
                {
                    actualAttackableSurfaces.Add(new AttackableSurfaceAsset { name = part.name });
                    wasModified = true;
                }
            }

            actualAttackableSurfaceAssets = wasModified ? actualAttackableSurfaces.ToArray() : attackableSurfaces;
            return !wasModified;
        }

        private List<PhysicalWeapon.PhysicalWeaponPart> GetPhysicalWeaponParts()
        {
            if (!itemType.Equals(Identifiers.ItemType.Weapon) || prefab == null)
            {
                return new List<PhysicalWeapon.PhysicalWeaponPart>();
            }

            var physicalWeapon = prefab.GetComponent<PhysicalWeapon>();

            if (physicalWeapon == null || physicalWeapon.parts == null)
            {
                return new List<PhysicalWeapon.PhysicalWeaponPart>();
            }

            return physicalWeapon.parts.ToList();
        }

        [Serializable]
        public struct AttackableSurfaceAsset
        {
            [ReadOnly] [AllowNesting] public string name;
            public DamageType damageType;
            [Range(0, 7)] public int penetrationClass;
            [MinValue(0)] public int incompleteStrikeDamage;
            [MinValue(0)] public int imperfectStrikeDamage;
            [MinValue(0)] public int perfectStrikeDamage;

            public WeaponSurface ToWeaponSurface(Weapon parentWeapon)
            {
                return new WeaponSurface
                {
                    name = this.name,
                    damageType = this.damageType,
                    basePenetrationClass = this.penetrationClass,
                    damageByStrikeType = new Dictionary<StrikeType, int>
                    {
                        { StrikeType.Incomplete, incompleteStrikeDamage },
                        { StrikeType.Imperfect, imperfectStrikeDamage },
                        { StrikeType.Perfect, perfectStrikeDamage },
                    },
                    parentWeapon = parentWeapon
                };
            }
        }
    }
}