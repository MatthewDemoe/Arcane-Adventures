using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public static class DisableCasterSpellCollision
    {
        public static void SetIgnoreCasterCollisionForSpell(GameObject spell, GameObject caster)
        {
            List<Collider> spellColliders = spell.GetComponentsInChildren<Collider>().ToList();

            if(spell.GetComponent<Collider>() != null)
                spellColliders.Add(spell.GetComponent<Collider>());

            GameObject creatureColliderParent = caster.TryGetComponent(out AttackableSurfaceCoordinator coordinator) ? 
                coordinator.gameObject : caster.transform.parent.GetComponentInChildren<AttackableSurfaceCoordinator>().gameObject;

            List<Collider> creatureColliders = creatureColliderParent.GetComponentsInChildren<Collider>().ToList();

            InjectorContainer.Injector.GetInstance<CreatureTracker>().GetCreaturesOnTeam(caster.GetComponent<CreatureBehaviour>().team).ForEach((creatureBehaviour) => creatureColliders.AddRange(creatureBehaviour.GetComponentsInChildren<Collider>().ToList()));

            creatureColliders.AddRange(GetWeaponCollidersOnHandSide(HandSide.Left, caster));

            creatureColliders.AddRange(GetWeaponCollidersOnHandSide(HandSide.Right, caster));

            foreach (var spellCollider in spellColliders)
            {
                foreach (var creatureCollider in creatureColliders)
                {
                    Physics.IgnoreCollision(spellCollider, creatureCollider, true);
                }
            }
        }

        private static List<Collider> GetWeaponCollidersOnHandSide(HandSide handSide, GameObject caster)
        {
            ItemEquipper.ItemEquipper itemEquipper = caster.GetComponent<ItemEquipper.ItemEquipper>();

            if (itemEquipper.GetItemInHand(handSide) == null)
                return new List<Collider>();

            return itemEquipper.GetItemInHand(handSide).GetComponentsInChildren<Collider>()
                    .Select(collider => collider)
                    .ToList();
        }
    }
}