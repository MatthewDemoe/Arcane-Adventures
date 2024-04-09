using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellMenu : MonoBehaviour
    {
        public static SpellMenu Instance { get; private set; } = null;

        private bool _menuEnabled = false;

        PlayerSpellCaster playerSpellCaster = null;

        [SerializeField]
        GameObject schoolButtons;

        SpellButtonContainer assaultButtonContainer;
        SpellButtonContainer controlButtonContainer;
        SpellButtonContainer enhancementButtonContainer;

        public bool hoverSelectionEnabled = true;

        private void Awake()
        {
            if (!(Instance is null))
            {
                Destroy(Instance);
            }

            Instance = this;
            gameObject.SetActive(false);

            List<SpellButtonContainer> spellButtonContainers = GetComponentsInChildren<SpellButtonContainer>().ToList();

            assaultButtonContainer = spellButtonContainers[0];
            assaultButtonContainer.gameObject.SetActive(false);

            controlButtonContainer = spellButtonContainers[1];
            controlButtonContainer.gameObject.SetActive(false);

            enhancementButtonContainer = spellButtonContainers[2];
            enhancementButtonContainer.gameObject.SetActive(false);
        }

        public void ToggleSpellMenu()
        {
            if(playerSpellCaster == null)
                playerSpellCaster = PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>();

            if (_menuEnabled)
                DisableSpellMenu();

            else if (!playerSpellCaster.isCasting)
                EnableSpellMenu();
        }

        private void EnableSpellMenu()
        {
            _menuEnabled = true;
            gameObject.SetActive(true);
        }

        private void DisableSpellMenu()
        {
            _menuEnabled = false;

            schoolButtons.SetActive(true);

            assaultButtonContainer.gameObject.SetActive(false);
            controlButtonContainer.gameObject.SetActive(false);
            enhancementButtonContainer.gameObject.SetActive(false);

            gameObject.SetActive(false);
        }

        public void EnableAssaultButtons()
        {
            EnableSchoolButtons(SpellType.Assault);
        }

        public void EnableControlButtons()
        {
            EnableSchoolButtons(SpellType.Control);
        }
        public void EnableEnhancementButtons()
        {
            EnableSchoolButtons(SpellType.Enhancement);
        }

        private void EnableSchoolButtons(SpellType spellType)
        {
            if (!hoverSelectionEnabled)
                return;

            schoolButtons.SetActive(false);

            switch (spellType)
            {
                case (SpellType.Assault):
                    assaultButtonContainer.gameObject.SetActive(true);
                    break;

                case (SpellType.Control):
                    controlButtonContainer.gameObject.SetActive(true);
                    break;

                case (SpellType.Enhancement):
                    enhancementButtonContainer.gameObject.SetActive(true);
                    break;

                default:
                    break;
            }
        }

        public void InitializeSpellButtons(List<Spell> spells)
        {
            assaultButtonContainer.InitializeButtonType(spells);
            controlButtonContainer.InitializeButtonType(spells);
            enhancementButtonContainer.InitializeButtonType(spells);
        }

        public void ToggleHoverSelection()
        {
            hoverSelectionEnabled = !hoverSelectionEnabled;
        }
    }
}
