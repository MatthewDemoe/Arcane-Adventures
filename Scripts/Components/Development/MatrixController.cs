using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class MatrixController : MonoBehaviour
    {
        private static readonly Dictionary<string, Color> EnvironmentColorsByName = new Dictionary<string, Color>
        {
            { nameof(Color.white), Color.white },
            { nameof(Color.black), Color.black },
            { nameof(Color.red), Color.red },
            { nameof(Color.blue), Color.blue },
        };
        private static readonly int ShaderPropertyIDForTint = Shader.PropertyToID("_Tint");

        [SerializeField] private float objectInstantiationXLimit;
        [SerializeField] private int valueDisplayDecimalPlaces;
        
        [SerializeField] private int instantiatedObjectSpacing;
        [SerializeField] private Transform instantiationPoint;
        [SerializeField] private GameObject[] prefabs;

        [Header("UI References")]
        [SerializeField] private Canvas menu;
        
        [SerializeField] private TMP_Dropdown prefabDropdown;
        
        [SerializeField] private Slider instancesSlider;
        [SerializeField] private TextMeshProUGUI instancesValueDisplay;
        
        [SerializeField] private Button instantiateButton;
        
        [SerializeField] private TMP_Dropdown colorDropdown;

        [SerializeField] private Slider yPositionSlider;
        [SerializeField] private TextMeshProUGUI yPositionValueDisplay;
        
        [SerializeField] private Slider scaleSlider;
        [SerializeField] private TextMeshProUGUI scaleValueDisplay;
        
        [SerializeField] private Slider xRotationSlider;
        [SerializeField] private TextMeshProUGUI xRotationValueDisplay;
        
        [SerializeField] private Slider yRotationSlider;
        [SerializeField] private TextMeshProUGUI yRotationValueDisplay;
        
        [SerializeField] private Slider zRotationSlider;
        [SerializeField] private TextMeshProUGUI zRotationValueDisplay;

        private bool menuIsFlipped = false;

        private void Start()
        {
            PopulatePrefabDropdown();
            PopulateEnvironmentColorDropdown();
            RenderSettings.skybox.SetColor(ShaderPropertyIDForTint, EnvironmentColorsByName.First().Value);
            AddListeners();
        }

        private void Update()
        {
            if (TryFlipMenu(Camera.main.transform.position.z))
                menuIsFlipped = !menuIsFlipped;
        }

        private bool TryFlipMenu(float cameraZ)
        {
            var menuShouldBeFlipped = cameraZ > menu.transform.position.z;

            if (menuShouldBeFlipped == menuIsFlipped)
                return false;

            var newYRotation = menuShouldBeFlipped ? 180 : 0;
            menu.transform.localRotation = Quaternion.Euler(0, newYRotation, 0);

            return true;
        }

        private void AddListeners()
        {
            instantiateButton.onClick.AddListener(InstantiateSelectedPrefab);
            colorDropdown.onValueChanged.AddListener(UpdateEnvironmentColor);
            yPositionSlider.onValueChanged.AddListener(UpdatePositionY);
            scaleSlider.onValueChanged.AddListener(UpdateScale);
            xRotationSlider.onValueChanged.AddListener(UpdateRotation);
            yRotationSlider.onValueChanged.AddListener(UpdateRotation);
            zRotationSlider.onValueChanged.AddListener(UpdateRotation);
            AddUpdateTextOnValueChangedListeners();
        }

        private void PopulatePrefabDropdown()
        {
            var options = prefabs.Select(prefab => prefab.name).ToList();
            
            prefabDropdown.AddOptions(options);   
        }

        private void PopulateEnvironmentColorDropdown()
        {
            var options = EnvironmentColorsByName.Select(keyValuePair => keyValuePair.Key.CapitalizeFirstLetter()).ToList();
            
            colorDropdown.AddOptions(options);
        }
        
        private void UpdateEnvironmentColor(int index)
        {
            var selectedOption = colorDropdown.options[index];
            var selectedColor = EnvironmentColorsByName[selectedOption.text.ToLowerInvariant()];
            
            RenderSettings.skybox.SetColor(ShaderPropertyIDForTint, selectedColor);
        }
        
        private void UpdatePositionY(float value)
        {
            var localPosition = instantiationPoint.localPosition;
            localPosition.y = value;
            instantiationPoint.localPosition = localPosition;
        }

        private void UpdateScale(float value)
        {
            var scale = Vector3.one * value;

            foreach (Transform child in instantiationPoint.transform)
                child.localScale = scale;
        }

        private void UpdateRotation(float value = 0)
        {
            var rotation = Quaternion.Euler(xRotationSlider.value, yRotationSlider.value, zRotationSlider.value);
            
            foreach (Transform child in instantiationPoint.transform)
                child.localRotation = rotation;
        }

        private void AddUpdateTextOnValueChangedListeners()
        {
            AddUpdateTextOnValueChangedListener(instancesSlider, instancesValueDisplay);
            AddUpdateTextOnValueChangedListener(yPositionSlider, yPositionValueDisplay);
            AddUpdateTextOnValueChangedListener(scaleSlider, scaleValueDisplay);
            AddUpdateTextOnValueChangedListener(xRotationSlider, xRotationValueDisplay);
            AddUpdateTextOnValueChangedListener(yRotationSlider, yRotationValueDisplay);
            AddUpdateTextOnValueChangedListener(zRotationSlider, zRotationValueDisplay);
        }

        private void AddUpdateTextOnValueChangedListener(Slider slider, TextMeshProUGUI valueDisplay)
        {
            slider.onValueChanged.AddListener(delegate (float value) { valueDisplay.text = value.Round(valueDisplayDecimalPlaces).ToString(CultureInfo.InvariantCulture); });
            
        }

        private void InstantiateSelectedPrefab()
        {
            RemoveAllInstantiatedPrefabs();
            var selectedPrefab = GetSelectedPrefab();
            var instances = (int)instancesSlider.value;
            InstantiatePrefabs(selectedPrefab, instances);

            UpdatePositionY(yPositionSlider.value);
            UpdateScale(scaleSlider.value);
            UpdateRotation();
        }

        private void RemoveAllInstantiatedPrefabs()
        {
            foreach(Transform child in instantiationPoint.transform)
                Destroy(child.gameObject);
        }

        private GameObject GetSelectedPrefab()
        {
            var selectedPrefabName = prefabDropdown.options[prefabDropdown.value].text;
            
            return prefabs.Single(prefab => prefab.name.Equals(selectedPrefabName));
        }
        
        private void InstantiatePrefabs(GameObject prefab, int instances)
        {
            var instancePosition = Vector3.zero;
            
            for (var i = 0; i < instances; i++)
            {
                if (i > 0)
                    instancePosition = GetNextInstancePosition(instancePosition);

                var container = new GameObject($"Instance container {i+1}")
                {
                    transform =
                    {
                        parent = instantiationPoint,
                        localPosition = instancePosition
                    }
                };
                
                Instantiate(prefab, container.transform);
            }
        }

        private Vector3 GetNextInstancePosition(Vector3 instancePosition)
        {
            instancePosition.x = -instancePosition.x;
            
            if (instancePosition.x >= 0)
            {
                instancePosition.x += instantiatedObjectSpacing;
            }
            
            if (instancePosition.x >= objectInstantiationXLimit)
            {
                instancePosition.x = 0;
                instancePosition.z -= instantiatedObjectSpacing;
            }

            return instancePosition;
        }
    }
}