using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellButtonContainer : MonoBehaviour
    {
        [SerializeField]
        SpellType spellType;

        [SerializeField]
        float buttonSpacing = 0.25f;

        [SerializeField]
        SpellMenu spellMenu;

        [SerializeField]
        GameObject spellButtonPrefab;

        [SerializeField]
        UIAudioSourcePlayer audioSourcePlayer;

        public void InitializeButtonType(List<Spell> spells)
        {
            List<Spell> spellsOfThisType = spells.Where((spell) => (spell.spellType == spellType)).ToList();
            
            EmptyList();
            CreateSpellButtons(spellsOfThisType);            
        }

        private void EmptyList()
        {
            foreach (Transform child in transform)
                Destroy(child.gameObject);
        }

        private void CreateSpellButtons(List<Spell> spells)
        {
            float numSpells = spells.Count;
            float halfSpells = numSpells / 2.0f;

            for (int i = 0; i < spells.Count; i++)
            {
                float buttonPosition = (halfSpells - i) * buttonSpacing;

                GameObject newButton = Instantiate(spellButtonPrefab, transform);
                newButton.transform.Translate(new Vector3(0.0f, buttonPosition, 0.0f));

                Spell theSpell = spells[i];

                newButton.GetComponent<SpellButton>().Initialize(theSpell);

                newButton.GetComponent<Button>().onClick.AddListener(() => spellMenu.ToggleSpellMenu());
                newButton.GetComponentInChildren<TextMeshProUGUI>().text = theSpell.name;
                newButton.GetComponent<SpellButtonCooldownTimer>().InitializeWithSpell(theSpell);

                Entry hoverEntryTrigger = new Entry();
                TriggerEvent hoverTriggerEvent = new TriggerEvent();
                hoverTriggerEvent.AddListener((baseEventData) => audioSourcePlayer.PlayHoverClip());

                hoverEntryTrigger.eventID = EventTriggerType.PointerEnter;
                hoverEntryTrigger.callback = hoverTriggerEvent;

                Entry selectEntryTrigger = new Entry();
                TriggerEvent selectTriggerEvent = new TriggerEvent();
                selectTriggerEvent.AddListener((baseEventData) => audioSourcePlayer.PlaySelectedAudioClip());

                selectEntryTrigger.eventID = EventTriggerType.Select;
                selectEntryTrigger.callback = selectTriggerEvent;

                newButton.GetComponent<SpellButton>().triggers.Add(hoverEntryTrigger);
                newButton.GetComponent<SpellButton>().triggers.Add(selectEntryTrigger);
            }
        }
    }
}
