using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Races;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation.BookPages
{
    public class RaceButton : MonoBehaviour
    {
        [SerializeField]
        Identifiers.Race myRace;
        Race realRace;

        Button myButton;

        TextMeshProUGUI buttonText;

        public UnityEvent<Race> OnClick = new UnityEvent<Race>();

        void Start()
        {
            myButton = GetComponent<Button>();
            buttonText = GetComponentInChildren<TextMeshProUGUI>();

            realRace = Race.Get(myRace);
            buttonText.text = realRace.name;

            myButton.onClick.AddListener(() => OnClick.Invoke(realRace));

            myButton.onClick.AddListener(SetPlayerRace);
        }

        private void SetPlayerRace()
        {
            CharacterCreator.Instance.UpdateRace(realRace);
        }
    }
}
