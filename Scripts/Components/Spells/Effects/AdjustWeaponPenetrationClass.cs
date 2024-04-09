using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AdjustWeaponPenetrationClass : PhysicalWeaponModifier
    {
        [SerializeField]
        int penetrationClassAdjustment = 1;

        protected override void ModifyWeapon(GameObject gameObject)
        {
            gameObject.GetComponent<PhysicalWeapon>().weapon.penetrationClassAdjustment += penetrationClassAdjustment;
        }

        protected override void ResetWeapon(GameObject gameObject)
        {
            gameObject.GetComponent<PhysicalWeapon>().weapon.penetrationClassAdjustment -= penetrationClassAdjustment;
        }
    }
}