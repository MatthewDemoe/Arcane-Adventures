using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class SpellAdjustmentPage : DebugMenuPage
    {
        public static SpellAdjustmentPage Instance { get; private set; } = null;

        private PlayerSpellCaster _playerSpellCaster = null;

        private Creature _playerCreature = null;

        [SerializeField] GameObject preparedAssaultSpellsParent;
        [SerializeField] GameObject preparedControlSpellsParent;
        [SerializeField] GameObject preparedEnhancementSpellsParent;

        [SerializeField] GameObject assaultSpellsParent;
        [SerializeField] GameObject controlSpellsParent;
        [SerializeField] GameObject enhancementSpellsParent;

        [SerializeField] GameObject spellButtonPrefab;

        delegate void OnButtonPress(Spell spell);

        private void Awake()
        {
            if (Instance != null)
            {
                throw new Exception("Multiple Spell Debug Page instances.");
            }

            Instance = this;
        }

        protected override bool TryInitialize()
        {
            if (PlayerCharacterReference.Instance is null) { return false; }

            _playerSpellCaster = PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>();
            _playerCreature = PlayerCharacterReference.Instance.creature;

            EmptyAllLists();

            BuildPreparedSpells();
            BuildSpellsToPrepare();

            return true;
        }

        private void BuildPreparedSpells()
        {
            if (!(_playerCreature is Character))
                return;

            List<Spell> preparedSpells = (_playerCreature as Character).preparedSpells;

            List<Spell> assaultSpells = preparedSpells.Where((spell) => spell.spellType == SpellType.Assault).ToList();
            List<Spell> controlSpells = preparedSpells.Where((spell) => spell.spellType == SpellType.Control).ToList();
            List<Spell> enhancementSpells = preparedSpells.Where((spell) => spell.spellType == SpellType.Enhancement).ToList();

            CreateSpellButtons(preparedAssaultSpellsParent.transform, assaultSpells, UnprepareSpell);
            CreateSpellButtons(preparedControlSpellsParent.transform, controlSpells, UnprepareSpell);
            CreateSpellButtons(preparedEnhancementSpellsParent.transform, enhancementSpells, UnprepareSpell);
        }

        private void BuildSpellsToPrepare()
        {
            if (!(_playerCreature is PlayerCharacter))
                return;

            List<Spell> playerSpells = (_playerCreature as PlayerCharacter).characterClass.spells;

            List<Spell> assaultSpells = playerSpells.Where((spell) => spell.spellType == SpellType.Assault).ToList();
            List<Spell> controlSpells = playerSpells.Where((spell) => spell.spellType == SpellType.Control).ToList();
            List<Spell> enhancementSpells = playerSpells.Where((spell) => spell.spellType == SpellType.Enhancement).ToList();

            CreateSpellButtons(assaultSpellsParent.transform, assaultSpells, PrepareSpell);
            CreateSpellButtons(controlSpellsParent.transform, controlSpells, PrepareSpell);
            CreateSpellButtons(enhancementSpellsParent.transform, enhancementSpells, PrepareSpell);
        }

        private void PrepareSpell(Spell spell)
        {
            _playerSpellCaster.PrepareSpell(spell);
            Initialize();
        }

        private void UnprepareSpell(Spell spell)
        {
            _playerSpellCaster.UnPrepareSpell(spell);
            Initialize();
        }

        private void CreateSpellButtons(Transform parent, List<Spell> spells, OnButtonPress onButtonPress)
        {
            for (int i = 0; i < spells.Count; i++)
            {
                GameObject newButton = Instantiate(spellButtonPrefab, parent);

                Spell theSpell = spells[i];

                newButton.GetComponent<Button>().onClick.AddListener(() =>
                {
                    onButtonPress.Invoke(theSpell);
                });

                newButton.GetComponentInChildren<TextMeshProUGUI>().text = theSpell.name;
            }
        }

        private void EmptyAllLists()
        {
            EmptyList(assaultSpellsParent.transform);
            EmptyList(controlSpellsParent.transform);
            EmptyList(enhancementSpellsParent.transform);

            EmptyList(preparedAssaultSpellsParent.transform);
            EmptyList(preparedControlSpellsParent.transform);
            EmptyList(preparedEnhancementSpellsParent.transform);
        }

        private void EmptyList(Transform parent)
        {
            foreach (Transform child in parent)
            {
                Destroy(child.gameObject);
            }
        }

        public void ToggleHoverSelect()
        {
            SpellMenu.Instance.ToggleHoverSelection();
        }
    }
}
