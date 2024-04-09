using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class MeasuringPage : DebugMenuPage
    {
        [Header("Measuring object")]
        [SerializeField] private TMP_Dropdown typeDropdown;
        [SerializeField] private TextMeshProUGUI distanceValueDisplay;
        [SerializeField] private Slider distanceSlider;
        [SerializeField] private Button createMeasuringObjectButton;
        [SerializeField] private GameObject measuringDiscPrefab;
        [SerializeField] private GameObject measuringRayPrefab;

        [Header("Measure from point")]
        [SerializeField] private TextMeshProUGUI distanceFromMeasuredPointValuesDisplay;
        [SerializeField] private Button startMeasureFromPointButton;
        [SerializeField] private GameObject measuringPointIndicatorPrefab;

        private bool isMeasuringFromPoint;
        private Vector3 pointToMeasureFrom;
        private GameObject measuringPointIndicator;
        private Camera mainCamera;
        private GameObject currentMeasuringObject;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (isMeasuringFromPoint)
            {
                UpdateMeasureFromPointValues();
            }
        }

        private void UpdateMeasureFromPointValues()
        {
            var pointToMeasureTo = mainCamera.transform.position;
            pointToMeasureTo.y = PlayerCharacterReference.Instance.transform.position.y;

            var distance = Vector3.Distance(pointToMeasureFrom, pointToMeasureTo);
            var diff = pointToMeasureFrom - pointToMeasureTo;

            distanceFromMeasuredPointValuesDisplay.text =
                $"{distance.Round(2)}\n" +
                $"{Mathf.Abs(diff.x).Round(2)}\n" +
                $"{Mathf.Abs(diff.y).Round(2)}\n" +
                $"{Mathf.Abs(diff.z).Round(2)}";
        }

        private void TriggerMeasuringObjectCreation()
        {
            var measuringObject = CreateMeasuringObject((TypeDropdownOption)typeDropdown.value);

            if (currentMeasuringObject != null)
            {
                Destroy(currentMeasuringObject);
            }

            currentMeasuringObject = measuringObject;
        }

        private GameObject CreateMeasuringObject(TypeDropdownOption type)
        {
            return type switch
            {
                TypeDropdownOption.Line => CreateMeasuringLine(),
                TypeDropdownOption.Circle => CreateMeasuringDisc(),
                _ => throw new NotImplementedException(),
            };
        }

        private GameObject CreateMeasuringLine()
        {
            var distance = distanceSlider.value;
            var position = mainCamera.transform.position + (mainCamera.transform.forward * (distance * 0.5f));
            var instance = Instantiate(measuringRayPrefab, position, Quaternion.LookRotation(mainCamera.transform.forward));
            instance.transform.localScale = new Vector3(instance.transform.localScale.x, instance.transform.localScale.y, distance);

            return instance;
        }

        private GameObject CreateMeasuringDisc()
        {
            var distance = distanceSlider.value;
            var position = mainCamera.transform.position;
            position.y = PlayerCharacterReference.Instance.transform.position.y;
            var instance = Instantiate(measuringDiscPrefab, position, new Quaternion());
            var xzScale = distance * 2f;
            instance.transform.localScale = new Vector3(xzScale, instance.transform.localScale.y, xzScale);

            return instance;
        }

        private void StartMeasuringFromPoint()
        {
            isMeasuringFromPoint = true;
            pointToMeasureFrom = mainCamera.transform.position;
            pointToMeasureFrom.y = PlayerCharacterReference.Instance.transform.position.y;

            if (measuringPointIndicator == null)
            {
                measuringPointIndicator = Instantiate(measuringPointIndicatorPrefab, pointToMeasureFrom, new Quaternion());
            }
            else
            {
                measuringPointIndicator.transform.position = pointToMeasureFrom;
            }
        }

        private void UpdateDistanceValueDisplay(float value) => distanceValueDisplay.text = $"{value.Round(2)}";

        protected override bool TryInitialize()
        {
            UpdateDistanceValueDisplay(distanceSlider.value);
            createMeasuringObjectButton.onClick.AddListener(TriggerMeasuringObjectCreation);
            startMeasureFromPointButton.onClick.AddListener(StartMeasuringFromPoint);
            distanceSlider.onValueChanged.AddListener(UpdateDistanceValueDisplay);

            return true;
        }

        private enum TypeDropdownOption
        {
            Line = 0, 
            Circle = 1
        }
    }
}