using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Effects;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{
    public class ChangeCharacterClass : MonoBehaviour//TODO: Update class name to match scope.
    {
        [SerializeField] private PedestalController classPedestals;
        [SerializeField] private PedestalController bookPedestal;

        private CharacterCreationPhase phase = CharacterCreationPhase.PickingClass;

        public void CheckBookCollisions(Collider other)
        {
            switch (phase)
            {
                case CharacterCreationPhase.PickingClass:
                    if (TrySubmitClassBook(other))
                    {
                        phase = CharacterCreationPhase.FillingOutDetails;
                    }
                    break;
                case CharacterCreationPhase.FillingOutDetails:
                    if (TrySubmitCharacterBook(other))
                    {
                        phase = CharacterCreationPhase.Finished;
                    }
                    break;
            }
        }

        private bool TrySubmitClassBook(Collider other)
        {
            var characterClassBook = other.GetComponentInParent<CharacterClassBook>();

            if (characterClassBook == null)
            {
                return false;
            }

            CharacterCreator.Instance.UpdateClass(characterClassBook.characterClass.classAttributes);
            Destroy(characterClassBook.gameObject);
            classPedestals.Reverse();
            bookPedestal.Forward();

            return true;
        }

        private bool TrySubmitCharacterBook(Collider other)
        {
            var statAssignPage = other.GetComponentInChildren<StatAssignPage>();

            if (statAssignPage == null || !IsEverythingFilledOut)
            {
                return false;
            }

            statAssignPage.GetComponentInParent<VirtualKeyboardCreator>().DestroyKeyboard();//TODO: May not be needed since scene is destroyed anyway?
            EndScene();

            return true;
        }

        private bool IsEverythingFilledOut => CharacterCreator.Instance.creatureReference.creature.stats.remainingStatPoints == 0
            && !string.IsNullOrWhiteSpace((CharacterCreator.Instance.creatureReference.creature as PlayerCharacter).playerName);

        private void EndScene()
        {
            InteractorHeldItemDropper.DropHeldItems();
            //TODO: "Forget" or "forgive" trigger button presses to prevent dropping of equipped weapon(s).
            CharacterCreator.Instance.Save();
            SceneLoader.LoadWithFadeEffect(GameScene.Hub);
        }

        private enum CharacterCreationPhase
        {
            PickingClass,
            FillingOutDetails,
            Finished
        }
    }
}
