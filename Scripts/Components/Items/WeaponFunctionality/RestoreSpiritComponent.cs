using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponFunctionality
{
    [RequireComponent(typeof(PhysicalHandHeldItem))]
    public class RestoreSpiritComponent : MonoBehaviour
    {
        [SerializeField]
        float restorationPercent;

        public void RestoreSpirit()
        {
            if (restorationPercent <= 0.0f)
                throw new Exception($"{nameof(restorationPercent)} is less than 0");

            Creature wielder = GetComponent<PhysicalHandHeldItem>().wielder.creature;
            wielder.stats.TryAdjustSpirit(-wielder.stats.maxSpirit * restorationPercent, out _);
        }
    }
}