using System.Collections.Generic;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using Injection;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items
{
    public class PhysicalWeaponTracker : IInjectable
    {
        List<PhysicalWeapon> physicalWeaponsInScene = new List<PhysicalWeapon>();

        public void TrackWeapon(PhysicalWeapon physicalWeapon)
        {
            if (!physicalWeaponsInScene.Contains(physicalWeapon))
                physicalWeaponsInScene.Add(physicalWeapon);
        }

        public void StopTrackingWeapon(PhysicalWeapon physicalWeapon)
        {
            if (physicalWeaponsInScene.Contains(physicalWeapon))
                physicalWeaponsInScene.Remove(physicalWeapon);
        }

        public List<PhysicalWeapon> GetAllWeaponsInScene()
        {
            return physicalWeaponsInScene;
        }

        public List<PhysicalWeapon> GetUnequippedWeaponsInScene()
        {
            return physicalWeaponsInScene.FindAll(physicalWeapon => !physicalWeapon.isWielded).ToList();
        }

        public PhysicalWeapon GetClosestWeapon(Vector3 point)
        {       
            return GetUnequippedWeaponsInScene().OrderBy(physicalWeapon => Vector3.Distance(point, physicalWeapon.transform.position)).First();
        }

        public PhysicalWeapon GetClosestWeapon(Vector3 point, List<Weapon> useableWeapons)
        {
            return GetUnequippedWeaponsInScene().OrderBy(physicalWeapon => Vector3.Distance(point, physicalWeapon.transform.position)).FirstOrDefault(physicalWeapon => useableWeapons.Contains(physicalWeapon.weapon));
        }
    }
}