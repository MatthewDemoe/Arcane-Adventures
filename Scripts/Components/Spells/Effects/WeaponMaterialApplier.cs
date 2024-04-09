using com.AlteredRealityLabs.ArcaneAdventures.Components.Materials;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class WeaponMaterialApplier : PhysicalWeaponModifier
    {
        [SerializeField]
        public Material materialToApply;

        [SerializeField]
        bool takeTexturesFromPreviousMaterial = false;

        [SerializeField]
        bool overlayMaterials = true;

        protected override void ModifyWeapon(GameObject gameObject)
        {
            if (!gameObject.TryGetComponent(out MaterialApplier materialApplier))
                materialApplier = gameObject.AddComponent<MaterialApplier>();

            materialApplier.Initialize(materialToApply, takeTexturesFromPreviousMaterial, overlayMaterials);
        }

        protected override void ResetWeapon(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out MaterialApplier materialApplier))
                materialApplier.ResetMaterial();
        }
    }
}