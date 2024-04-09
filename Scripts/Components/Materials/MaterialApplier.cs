using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using UnityEngine;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Materials
{
    public class MaterialApplier : MonoBehaviour
    {
        [SerializeField]
        MeshRenderer meshRenderer;

        [SerializeField]
        bool applyOnStart = true;

        [SerializeField]
        bool overlayMaterials = true;

        [SerializeField]
        bool takeTexturesFromPreviousMaterial = false;

        List<Material> materialsToInclude = new List<Material>();
        public Material materialToApply;
        private Material originalMaterial;

        public void Initialize(Material newMaterial, bool takeTexturesFromPreviousMaterial, bool overlayMaterials)
        {
            if (meshRenderer is null)
                meshRenderer = GetComponentInChildren<MeshRenderer>();

            materialToApply = new Material(newMaterial);

            this.overlayMaterials = overlayMaterials;
            this.takeTexturesFromPreviousMaterial = takeTexturesFromPreviousMaterial;

            if (applyOnStart)
                ReplaceMaterial();
        }

        public void ReplaceMaterial()
        {
            if (takeTexturesFromPreviousMaterial)
            {
                materialToApply.SetTexture(ShaderProperties.BaseMap, originalMaterial.GetTexture(ShaderProperties.BaseMap));
                materialToApply.SetTexture(ShaderProperties.NormalMap, originalMaterial.GetTexture(ShaderProperties.NormalMap));
            }

            materialsToInclude.Add(materialToApply);

            if (overlayMaterials)
            {
                originalMaterial = meshRenderer.material;
                materialsToInclude.Add(originalMaterial);
            }

            meshRenderer.materials = materialsToInclude.ToArray();            
        }

        public void ResetMaterial()
        {
            meshRenderer.materials = new Material[] { originalMaterial };
            Destroy(this);
        }
    }
}