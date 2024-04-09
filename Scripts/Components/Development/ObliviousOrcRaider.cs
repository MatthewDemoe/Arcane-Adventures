using UnityEngine;
using UMA.CharacterSystem;
using UMA;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class ObliviousOrcRaider : HumanoidMonster
    {
        protected override System.Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.OrcRaider); } }

        private static readonly Dictionary<string, string> AttackableSurfaceNameExceptionsByBoneNames = new Dictionary<string, string>
        {
            { "Spine", "Lower Torso" },
            { "Spine1", "Upper Torso" },
        };

        private DynamicCharacterAvatar dynamicCharacterAvatar;
        private bool attackableSurfaceComponentsAdded = false;

        protected override void Awake()
        {
            dynamicCharacterAvatar = GetComponent<DynamicCharacterAvatar>();
            dynamicCharacterAvatar.CharacterUpdated.AddListener(OnCharacterBegunCallback);
        }

        protected override void Start()
        {
            base.Start();
        }

        public void OnCharacterBegunCallback(UMAData umaData)
        {
            if (attackableSurfaceComponentsAdded) { return; }

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

                collider.gameObject.AddComponent<WeaponLodgeSurface>();
            }

            attackableSurfaceComponentsAdded = true;
        }
    }
}