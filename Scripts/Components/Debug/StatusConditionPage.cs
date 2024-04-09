using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class StatusConditionPage : DebugMenuPage
    {
        [SerializeField] private Button equipSpellButton;
        [SerializeField] private Button addToPlayerButton;
        [SerializeField] private TMP_Dropdown conditionDropdown;
        [SerializeField] private Toggle showGhostHandToggle;

        private const string unimplementedSuffix = " - X";

        List<string> statusConditionsByName = new List<string>();
        List<string> implementedStatusConditions = new List<string>();

        string statusConditionSource => "Status Condition Debug Page";

        protected override bool TryInitialize()
        {
            PopulateImplementedStatusConditions();

            PopulateStatusConditionsByName();

            conditionDropdown.AddOptions(statusConditionsByName);

            (transform as RectTransform).localScale = Vector3.one;
            equipSpellButton.onClick.AddListener(EquipSpell);
            addToPlayerButton.onClick.AddListener(AddConditionToPlayer);

            showGhostHandToggle.isOn = CombatSettings.Controller.showGhostHandsWhenStunned;

            showGhostHandToggle.onValueChanged.AddListener(delegate (bool value)
            {
                CombatSettings.Controller.showGhostHandsWhenStunned = showGhostHandToggle.isOn;
            });

            return true;
        }

        private void EquipSpell()
        {
            AllStatusConditions.StatusConditionName? statusConditionToUse = GetSelectedStatusCondition();

            if (statusConditionToUse == null)
                return;

            PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().CreateStatusSpell((AllStatusConditions.StatusConditionName)statusConditionToUse, HandSide.Right);
        }

        private void AddConditionToPlayer()
        {
            AllStatusConditions.StatusConditionName? statusConditionToUse = GetSelectedStatusCondition();

            if (statusConditionToUse == null)
                return;

            Creature player = PlayerCharacterReference.Instance.creature;

            if (!player.statusConditionTracker.HasStatusCondition((AllStatusConditions.StatusConditionName)statusConditionToUse))
            {
                StatusCondition statusCondition = AllStatusConditions.ConvertEnumToStatusCondition((AllStatusConditions.StatusConditionName)statusConditionToUse, ref player, 
                    new StatusConditionSettings(durationInMilliseconds: 5000), statusConditionSource);
                player.statusConditionTracker.AddStatusCondition(statusCondition);
            }
        }

        private AllStatusConditions.StatusConditionName? GetSelectedStatusCondition()
        {
            var name = conditionDropdown.options[conditionDropdown.value].text;

            if (name.Contains(unimplementedSuffix))
                return null;

            return (AllStatusConditions.StatusConditionName)statusConditionsByName.IndexOf(name);
        }

        private void PopulateStatusConditionsByName()
        {
            statusConditionsByName = Enum.GetNames(typeof(AllStatusConditions.StatusConditionName)).ToList();

            for (int i = 0; i < statusConditionsByName.Count; i++)
            {
                if (!implementedStatusConditions.Contains(statusConditionsByName[i]))
                    statusConditionsByName[i] += unimplementedSuffix;  
            }
        }

        private void PopulateImplementedStatusConditions()
        {
            List<Type> statusConditionTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsSubclassOf(typeof(StatusCondition)) && !type.IsAbstract)
            .ToList();

            implementedStatusConditions = statusConditionTypes.Select((type) => type.Name).ToList(); 
        }
    }
}