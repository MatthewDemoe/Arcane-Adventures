using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using System.Collections.Generic;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class WeaponSurface : AttackSurface
    {
        public const int MinimumPenetrationClass = 0;
        public const int MaximumPenetrationClass = 3;

        public int basePenetrationClass;
        public Dictionary<StrikeType, int> damageByStrikeType;
        public Weapon parentWeapon;

        public int penetrationClass => Math.Min(Math.Max(basePenetrationClass + parentWeapon.penetrationClassAdjustment, MinimumPenetrationClass), MaximumPenetrationClass);
    }
}