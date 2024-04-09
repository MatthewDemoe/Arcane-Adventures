using UnityEngine;
using TMPro;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class ConfirmPage : BookPage
    {
        [SerializeField] TextMeshProUGUI overviewBody;

        public string characterName = null;

        CharacterCreator cc;

        private void Start()
        {
            cc = CharacterCreator.Instance;
        }

        private void Update()
        {
            if (cc.creatureReference.creature != null)
            {
                var raceName = Race.Get(cc.creatureReference.creature.race).name;

                overviewBody.text = $"Name: {(cc.creatureReference.creature as PlayerCharacter).playerName}\n" +
                    $"Gender: {(cc.creatureReference.creature as PlayerCharacter).gender.ToString()}\n"+
                    $"Race: {raceName}\n" +
                    $"Class: {(cc.creatureReference.creature as PlayerCharacter).characterClass.classAttributes.name}\n" +
                    $"Strength: {cc.creatureReference.creature.stats.subtotalStrength}\n" +
                    $"Vitality: {cc.creatureReference.creature.stats.subtotalVitality}\n" +
                    $"Spirit: {cc.creatureReference.creature.stats.subtotalSpirit}";
            }
        }

        public void SetCharacterName(string name)
        {
            characterName = name;
            (CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).playerName = characterName;
        }

        public void ConfirmCharacter()
        {
            if (characterName == string.Empty)            
                return;

            InteractorHeldItemDropper.DropHeldItems();
            (CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).playerName = characterName;
            CharacterCreator.Instance.Save();

            SceneLoader.LoadWithFadeEffect(GameScene.Hub);            
        }
    }
}
