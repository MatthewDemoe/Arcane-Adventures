using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using Injection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class InGameMaterialCollisionProcessor : IInjectable
    {
        private const int InitialContactEffectPlayerPoolSize = 1;

        private static readonly List<DamageType> LodgableDamageTypes = new List<DamageType>
        {
            DamageType.Piercing,
            DamageType.Slashing
        };

        private Dictionary<Identifiers.InGameMaterial, InGameMaterial> inGameMaterialsByIdentifier = new Dictionary<Identifiers.InGameMaterial, InGameMaterial>
        {
            { Identifiers.InGameMaterial.Flesh, new Flesh() },
            { Identifiers.InGameMaterial.Wood, new Wood() },
            { Identifiers.InGameMaterial.Metal, new Metal() },
            { Identifiers.InGameMaterial.Stone, new Stone() },
            { Identifiers.InGameMaterial.Glass, new Glass() },
            { Identifiers.InGameMaterial.Water, new Water() },
            { Identifiers.InGameMaterial.Dirt, new Dirt() },
            { Identifiers.InGameMaterial.Sand, new Sand() },
        };

        private GameObject ContactEffectPlayerPrefab;
        private List<ContactEffectPlayer> contactEffectPlayers = new List<ContactEffectPlayer>();

        public InGameMaterialCollisionProcessor()
        {
            PopulateContactEffectPlayerPool();
        }

        private void PopulateContactEffectPlayerPool()
        {
            ContactEffectPlayerPrefab = Prefabs.Load(Prefabs.PrefabNames.ContactEffectPlayer);

            for (var i = 0; i < InitialContactEffectPlayerPoolSize; i++)
            {
                contactEffectPlayers.Add(Object.Instantiate(ContactEffectPlayerPrefab).GetComponent<ContactEffectPlayer>());
            }
        }

        public InGameMaterial Resolve(Identifiers.InGameMaterial identifier) => inGameMaterialsByIdentifier[identifier];

        public bool TryLodge(Collision collision, WeaponLodgeSurface weaponLodgeSurface, out LodgedWeaponPoint newLodgedWeaponPoint)
        {
            newLodgedWeaponPoint = null;

            if (!collision.gameObject.TryGetComponent<PhysicalWeapon>(out var physicalWeapon)) { return false; }

            var existingLodgedWeaponPoint = weaponLodgeSurface.GetLodgedWeaponPoint(physicalWeapon);

            if (HasLodgableContactPoints(collision, physicalWeapon, out var lodgableContactPoints))
            {
                if (existingLodgedWeaponPoint == null && physicalWeapon.isStriking)
                {
                    newLodgedWeaponPoint = weaponLodgeSurface.gameObject.AddComponent<LodgedWeaponPoint>();
                    newLodgedWeaponPoint.Activate(physicalWeapon, weaponLodgeSurface, lodgableContactPoints.First());

                    return true;
                }
            }
            else
            {
                if (existingLodgedWeaponPoint != null)
                {
                    return existingLodgedWeaponPoint.TryLodge(ignoreStriking: true);
                }
            }

            return false;
        }

        public void PlayContactEffects(Identifiers.InGameMaterial inGameMaterialA, Identifiers.InGameMaterial inGameMaterialB, Vector3 position, float scale)
        {
            PlayEffect(inGameMaterialA, inGameMaterialB, position, scale);
            PlayEffect(inGameMaterialB, inGameMaterialA, position, scale);
        }

        private void PlayEffect(Identifiers.InGameMaterial targetInGameMaterial, Identifiers.InGameMaterial contactInGameMaterial, Vector3 position, float scale)
        {
            var availableContactEffectPlayer = contactEffectPlayers.FirstOrDefault(contactEffectPlayer => contactEffectPlayer.isAvailable);

            if (availableContactEffectPlayer is null)
            {
                availableContactEffectPlayer = Object.Instantiate(ContactEffectPlayerPrefab).GetComponent<ContactEffectPlayer>();
                contactEffectPlayers.Add(availableContactEffectPlayer);
            }

            var contactEffect = Resolve(targetInGameMaterial).GetContactEffect(contactInGameMaterial);

            if (contactEffect != ContactEffect.Nothing)
            {
                availableContactEffectPlayer.PlayEffect(contactEffect, position, scale);
            }
        }

        private static bool HasLodgableContactPoints(Collision collision, PhysicalWeapon physicalWeapon, out IEnumerable<ContactPoint> lodgableContactPoints)
        {
            lodgableContactPoints = collision.contacts
                .Where(contact => LodgableDamageTypes.Contains(physicalWeapon.GetColliderDamageType(contact.otherCollider)));

            return lodgableContactPoints.Any();
        }
    }
}