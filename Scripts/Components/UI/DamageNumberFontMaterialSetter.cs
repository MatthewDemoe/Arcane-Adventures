using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class DamageNumberFontMaterialSetter : MonoBehaviour
    {
        [SerializeField] private Material perfectStrikeMaterial;
        [SerializeField] private Material imperfectStrikeMaterial;
        [SerializeField] private Material incompleteStrikeMaterial;

        public void UpdateMaterial(StrikeType strikeType)
        {
            var textMeshProUGui = GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGui.fontSharedMaterial = GetMaterial(strikeType);
        }

        private Material GetMaterial(StrikeType strikeType)
        {
            switch (strikeType)
            {
                case StrikeType.Perfect: return perfectStrikeMaterial;
                case StrikeType.Imperfect: return imperfectStrikeMaterial;
                case StrikeType.Incomplete: return incompleteStrikeMaterial;
                default: throw new Exception("Invalid strike type");
            }
        }
    }
}