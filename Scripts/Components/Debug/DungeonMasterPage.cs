using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class DungeonMasterPage : DebugMenuPage
    {
        [SerializeField] private Button spawnButton;
        [SerializeField] private TMP_Dropdown monsterDropdown;

        [SerializeField] private Slider distanceSlider;
        [SerializeField] private TextMeshProUGUI distanceValueDisplay;

        [SerializeField] private Toggle isArmedToggle;

        private Dictionary<string, Type> monsterTypesByName;

        MobCoordinator mobCoordinator;

        private bool spawnArmedCreatures = true;

        private void Start()
        {
            mobCoordinator = Instantiate(new GameObject()).AddComponent<MobCoordinator>();
            mobCoordinator.gameObject.AddComponent<DoNotDestroyOnLoad>();
        }

        protected override bool TryInitialize()
        {
            PopulateMonsterTypesByName();
            var options = monsterTypesByName.Select(x => x.Key).ToList();
            monsterDropdown.AddOptions(options);

            (transform as RectTransform).localScale = Vector3.one;
            spawnButton.onClick.AddListener(Spawn);

            isArmedToggle.isOn = spawnArmedCreatures;

            isArmedToggle.onValueChanged.AddListener(delegate { spawnArmedCreatures = isArmedToggle.isOn; });

            distanceSlider.onValueChanged.AddListener(delegate (float value) { distanceValueDisplay.text = value.ToString(); });

            return true;
        }

        private void Spawn()
        {
            var name = monsterDropdown.options[monsterDropdown.value].text;
            var monster = DefaultCreatureResolver.GetDefaultCreature(monsterTypesByName[name], spawnArmedCreatures);
            var spawnLocation = GetSpawnLocation();

            GameObject newCreature = Creatures.CreatureBuilder.Build(monster, spawnLocation);
            newCreature.transform.parent = mobCoordinator.transform;
            mobCoordinator.AddCreature(newCreature.GetComponent<CreatureBehaviour>());
            mobCoordinator.AssignNonCombatRoles();
        }

        private Vector3 GetSpawnLocation()
        {
            var distance = (int)distanceSlider.value;
            var position = PlayerCharacterReference.Instance.transform.position + (PlayerCharacterReference.Instance.transform.forward * distance);

            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 100, NavMesh.AllAreas))
                position = hit.position;

            else
                position.y += 25;

            return position;
        }

        private void PopulateMonsterTypesByName()
        {
            monsterTypesByName = monsterTypesByName = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(Monster)) && !type.IsAbstract)
            .ToDictionary(type => type.Name.AddSpacesToPascalCase(), type => type);
        }
    }
}