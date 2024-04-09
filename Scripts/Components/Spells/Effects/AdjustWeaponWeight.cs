using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AdjustWeaponWeight : PhysicalWeaponModifier
    {
        [SerializeField]
        int weightAdjustment = 1;

        protected override void ModifyWeapon(GameObject gameObject)
        {
            gameObject.GetComponent<PhysicalWeapon>().weapon.weightAdjustment += weightAdjustment;
        }

        protected override void ResetWeapon(GameObject gameObject)
        {
            gameObject.GetComponent<PhysicalWeapon>().weapon.weightAdjustment -= weightAdjustment;
        }
    }
}