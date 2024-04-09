using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    public class AttackableSurfaceCoordinator : MonoBehaviour
    {
        public CreatureReference creatureReference { get; private set; }

        private const float strikeImmunityTime = 0.5f;

        private static readonly Dictionary<string, string> AttackableSurfaceNameExceptionsByBoneNames = new Dictionary<string, string>
        {
            { "Spine", "Lower Torso" },
            { "Spine1", "Upper Torso" },
        };

        private DynamicCharacterAvatar dynamicCharacterAvatar;

        private readonly Dictionary<Guid, float> collisionTimeByStrikeGuid = new Dictionary<Guid, float>();

        private Collider[] cachedColliders;
        private bool attackableSurfaceComponentsAdded;

        public Collider[] colliders => GetColliders();

        private void Awake()
        {
            creatureReference = GetComponent<CreatureReference>();

            if (creatureReference == null)
                creatureReference = transform.parent.GetComponentInChildren<CreatureReference>();


            dynamicCharacterAvatar = GetComponent<DynamicCharacterAvatar>();

            if (dynamicCharacterAvatar is DynamicCharacterAvatar)
            {
                attackableSurfaceComponentsAdded = false;
                dynamicCharacterAvatar.CharacterUpdated.AddListener(OnCharacterBegunCallback);
            }
            else
            {
                attackableSurfaceComponentsAdded = true;
            }
        }

        public bool RegisterStrikeGuid(Guid strikeGuid, float immunityTime = strikeImmunityTime)
        {
            if (collisionTimeByStrikeGuid.TryGetValue(strikeGuid, out var collisionTime))
            {
                var immunityExpirationTime = collisionTime + immunityTime;

                if (immunityExpirationTime > Time.timeSinceLevelLoad)
                {
                    return false;
                }
                else
                {
                    collisionTimeByStrikeGuid[strikeGuid] = Time.timeSinceLevelLoad;
                    return true;
                }
            }

            collisionTimeByStrikeGuid.Add(strikeGuid, Time.timeSinceLevelLoad);
            return true;
        }

        private Collider[] GetColliders()
        {
            if (attackableSurfaceComponentsAdded && cachedColliders is null)
            {
                cachedColliders = GetComponentsInChildren<AttackableSurface>()
                .SelectMany(attackableSurface => attackableSurface.colliders)
                .ToArray();
            }

            return cachedColliders;
        }

        private void OnCharacterBegunCallback(UMAData umaData)
        {
            if (!attackableSurfaceComponentsAdded)
            {
                AddAttackableSurfaceComponents();
                attackableSurfaceComponentsAdded = true;
                dynamicCharacterAvatar.CharacterUpdated.RemoveListener(OnCharacterBegunCallback);
            }
        }

        private void AddAttackableSurfaceComponents()
        {
            var colliders = dynamicCharacterAvatar.GetComponentsInChildren<Collider>();
            var attackableSurfaceNames = new List<string>();

            foreach (var collider in colliders)
            {
                if (collider.gameObject == this.gameObject || collider.GetComponent<AttackableSurface>() != null)
                {
                    continue;
                }

                var attackableSurface = collider.gameObject.AddComponent<AttackableSurface>();
                var attackableSurfaceName = collider.gameObject.name.AddSpacesToPascalCase();

                if (AttackableSurfaceNameExceptionsByBoneNames.ContainsKey(attackableSurfaceName))
                {
                    attackableSurfaceName = AttackableSurfaceNameExceptionsByBoneNames[attackableSurfaceName];
                }

                attackableSurface.SetName(attackableSurfaceName);
                attackableSurfaceNames.Add(attackableSurfaceName);
            }
        }
    }
}