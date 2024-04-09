using System.Collections.Generic;
using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class SpellPage : BookPage
    {
        [SerializeField]
        CharacterClassBook characterClassBook;
        PrimaryCharacterClass characterClass;

        [SerializeField]
        TextMeshProUGUI archetypeTitle;

        [SerializeField]
        TextMeshProUGUI spellsBody;

        private void Start()
        {
            characterClass = PrimaryCharacterClass.Get(characterClassBook.characterClassIdentifier);

            archetypeTitle.text = $"<u>{characterClass.classAttributes.name} Spells</u>";

            List<Spell> firstLevelSpellObjects = characterClass.spells;
            List<string> piecesOfSpellText = new List<string>();

            spellsBody.text = "";
            for (int i = 0; i < firstLevelSpellObjects.Count; i++)
            {
                piecesOfSpellText.Add($"<i><u>{firstLevelSpellObjects[i].name}</i></u> : {firstLevelSpellObjects[i].effectDescription}");
            }

            spellsBody.text = string.Join("\n\n", piecesOfSpellText);
        }
    }
}
