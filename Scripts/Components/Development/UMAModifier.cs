using System.Collections.Generic;
using TMPro;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class UMAModifier : MonoBehaviour
    {
        [SerializeField] private GameObject linePrefab;

        [SerializeField] private DynamicCharacterAvatar orcAvatar;
        [SerializeField] private DynamicCharacterAvatar humanAvatar;

        [SerializeField] private Button orcButton;
        [SerializeField] private Button humanButton;

        [SerializeField] private Button wrappedModeButton;
        [SerializeField] private Button umaDnaModeButton;

        [SerializeField] private GameObject wrappedModeScreen;
        [SerializeField] private GameObject umaDnaModeScreen;

        [SerializeField] private TextMeshProUGUI balancedHeightValueDisplay;
        [SerializeField] private Slider balancedHeightSlider;

        [SerializeField] private Slider groupOneBigSlider;
        [SerializeField] private Slider groupOneSmallSlider;
        [SerializeField] private Slider groupTwoBigSlider;
        [SerializeField] private Slider groupTwoSmallSlider;
        [SerializeField] private Slider groupThreeBigSlider;
        [SerializeField] private Slider groupThreeSmallSlider;

        [SerializeField] private TextMeshProUGUI groupOneBigValueDisplay;
        [SerializeField] private TextMeshProUGUI groupOneSmallValueDisplay;
        [SerializeField] private TextMeshProUGUI groupTwoBigValueDisplay;
        [SerializeField] private TextMeshProUGUI groupTwoSmallValueDisplay;
        [SerializeField] private TextMeshProUGUI groupThreeBigValueDisplay;
        [SerializeField] private TextMeshProUGUI groupThreeSmallValueDisplay;

        private DynamicCharacterAvatar selectedAvatar;
        private Identifiers.Race selectedRace;
        private GameObject layoutGroup;
        private Dictionary<Identifiers.Race, AvatarHeightSetter> avatarHeightSettersByRace = new Dictionary<Identifiers.Race, AvatarHeightSetter>();

        private AvatarHeightSetter avatarHeightSetter => avatarHeightSettersByRace[selectedRace];

        private void Start()
        {
            layoutGroup = umaDnaModeScreen.GetComponentInChildren<LayoutGroup>(includeInactive: true).gameObject;

            orcButton.onClick.AddListener(delegate () { SelectAvatar(orcAvatar, Identifiers.Race.Elf); });
            humanButton.onClick.AddListener(delegate () { SelectAvatar(humanAvatar, Identifiers.Race.Human); });

            wrappedModeButton.onClick.AddListener(delegate ()
                {
                    wrappedModeScreen.SetActive(true);
                    umaDnaModeScreen.SetActive(false);
                }
            );

            umaDnaModeButton.onClick.AddListener(delegate ()
                {
                    umaDnaModeScreen.SetActive(true);
                    wrappedModeScreen.SetActive(false);
                    ClearDnaList();
                    PopulateDnaList(selectedAvatar);
                }
            );

            balancedHeightSlider.onValueChanged.AddListener(delegate (float value) { SetBalancedHeight(value); });
            avatarHeightSettersByRace.Add(Identifiers.Race.Human, new AvatarHeightSetter(humanAvatar, Identifiers.Race.Human));
            avatarHeightSettersByRace.Add(Identifiers.Race.Elf, new AvatarHeightSetter(orcAvatar, Identifiers.Race.Elf));

            foreach (var avatarHeightSetter in avatarHeightSettersByRace.Values)
            {
                StartCoroutine(avatarHeightSetter.Initialize());
            }

            groupOneBigSlider.value = AvatarHeightSetter.GroupOneBigModifier;
            groupOneSmallSlider.value = AvatarHeightSetter.GroupOneSmallModifier;
            groupTwoBigSlider.value = AvatarHeightSetter.GroupTwoBigModifier;
            groupTwoSmallSlider.value = AvatarHeightSetter.GroupTwoSmallModifier;
            groupThreeBigSlider.value = AvatarHeightSetter.GroupThreeBigModifier;
            groupThreeSmallSlider.value = AvatarHeightSetter.GroupThreeSmallModifier;

            groupOneBigValueDisplay.text = AvatarHeightSetter.GroupOneBigModifier.ToString();
            groupOneSmallValueDisplay.text = AvatarHeightSetter.GroupOneSmallModifier.ToString();
            groupTwoBigValueDisplay.text = AvatarHeightSetter.GroupTwoBigModifier.ToString();
            groupTwoSmallValueDisplay.text = AvatarHeightSetter.GroupTwoSmallModifier.ToString();
            groupThreeBigValueDisplay.text = AvatarHeightSetter.GroupThreeBigModifier.ToString();
            groupThreeSmallValueDisplay.text = AvatarHeightSetter.GroupThreeSmallModifier.ToString();

            groupOneBigSlider.onValueChanged.AddListener(delegate (float value) { groupOneBigValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupOneBigModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
            groupOneSmallSlider.onValueChanged.AddListener(delegate (float value) { groupOneSmallValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupOneSmallModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
            groupTwoBigSlider.onValueChanged.AddListener(delegate (float value) { groupTwoBigValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupTwoBigModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
            groupTwoSmallSlider.onValueChanged.AddListener(delegate (float value) { groupTwoSmallValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupTwoSmallModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
            groupThreeBigSlider.onValueChanged.AddListener(delegate (float value) { groupThreeBigValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupThreeBigModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
            groupThreeSmallSlider.onValueChanged.AddListener(delegate (float value) { groupThreeSmallValueDisplay.text = value.ToString(); AvatarHeightSetter.GroupThreeSmallModifier = value; SetBalancedHeight(balancedHeightSlider.value); });
        }

        private void SelectAvatar(DynamicCharacterAvatar avatar, Identifiers.Race race)
        {
            selectedRace = race;
            balancedHeightSlider.minValue = avatarHeightSetter.enforcedMinimumHeightInDecimal;
            balancedHeightSlider.maxValue = avatarHeightSetter.enforcedMaximumHeightInDecimal;
            balancedHeightSlider.value = avatarHeightSetter.defaultHeightInDecimal;

            if (selectedAvatar != null)
            {
                selectedAvatar.gameObject.SetActive(false);
            }

            selectedAvatar = avatar;
            selectedAvatar.gameObject.SetActive(true);

            if (umaDnaModeScreen.activeSelf)
            {
                ClearDnaList();
                PopulateDnaList(selectedAvatar);
            }
         }

        private void ClearDnaList()
        {
             foreach (Transform child in layoutGroup.transform)
            {
                Destroy(child.gameObject);
            }
        }

        private void PopulateDnaList(DynamicCharacterAvatar avatar)
        {
            var dna = avatar.GetDNA();

            foreach (var keyValuePair in dna)
            {
                var name = keyValuePair.Key;
                var dnaSetter = keyValuePair.Value;

                var line = Instantiate(linePrefab, layoutGroup.transform);
                var textMeshPros = line.GetComponentsInChildren<TextMeshProUGUI>();
                textMeshPros[0].text = name;
                textMeshPros[1].text = dnaSetter.Value.ToString();
                var slider = line.GetComponentInChildren<Slider>();
                slider.value = dnaSetter.Value;
                slider.onValueChanged.AddListener(delegate (float value)
                    {
                        textMeshPros[1].text = value.ToString();
                        dnaSetter.Set(value);
                        avatar.ForceUpdate(true);
                    }
                );
            }
        }

        private void SetBalancedHeight(float newHeight)
        {
            avatarHeightSetter.SetHeight(newHeight);

            var heightInCm = avatarHeightSetter.GetHeightInMeters(newHeight) * 100;

            balancedHeightValueDisplay.text = $"{Mathf.Round(heightInCm)} cm / {GetFeetAndInches(heightInCm)}";
        }

        private string GetFeetAndInches(float cm)
        {
            var totalInches = Mathf.RoundToInt(cm / 2.54f);
            var feet = (totalInches - totalInches % 12) / 12;
            var inches = totalInches % 12;

            return $"{feet}′{inches}″";
        }
    }
}