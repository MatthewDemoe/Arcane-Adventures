using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Debug
{
    public class ControlsAdjustmentsPage : DebugMenuPage
    {
        private const float UpdateDelayInSeconds = 1f;
        private const int DecimalPlaces = 2;

        [SerializeField] private TextMeshProUGUI values;
        [SerializeField] private Slider maximumVelocitySlider;
        [SerializeField] private Slider maximumAngularVelocitySlider;
        [SerializeField] private Slider characterStrengthToMassRatioSlider;
        [SerializeField] private Slider leftWeaponMassSlider;
        [SerializeField] private Slider rightWeaponMassSlider;

        private float lastUpdateTime;
        private PhysicalHandHeldItem cachedLeftWeapon;
        private PhysicalHandHeldItem cachedRightWeapon;

        private void Update()
        {
            if (Time.time > lastUpdateTime + UpdateDelayInSeconds)
            {
                var leftWeapon = ControllerLink.Get(HandSide.Left).connectedItem;

                if (!ReferenceEquals(leftWeapon, cachedLeftWeapon))
                {
                    cachedLeftWeapon = leftWeapon;
                    leftWeaponMassSlider.value = GetWeaponMass(HandSide.Left);
                    UpdateValues();
                }

                var rightWeapon = ControllerLink.Get(HandSide.Right).connectedItem;

                if (!ReferenceEquals(rightWeapon, cachedRightWeapon))
                {
                    cachedRightWeapon = rightWeapon;
                    rightWeaponMassSlider.value = GetWeaponMass(HandSide.Right);
                    UpdateValues();
                }

                lastUpdateTime = Time.time;
            }
        }

        private void SetInitialValues()
        {
            maximumVelocitySlider.value = CombatSettings.Controller.MaximumVelocity;
            maximumAngularVelocitySlider.value = CombatSettings.Controller.MaximumAngularVelocity;
            characterStrengthToMassRatioSlider.value = CombatSettings.Controller.CharacterStrengthToMassRatio;
            leftWeaponMassSlider.value = 0.1f;
            rightWeaponMassSlider.value = 0.1f;
        }

        public override void ResetValues()
        {
            maximumVelocitySlider.value = CombatSettings.Controller.DefaultMaximumVelocity;
            maximumAngularVelocitySlider.value = CombatSettings.Controller.DefaultMaximumAngularVelocity;
            characterStrengthToMassRatioSlider.value = CombatSettings.Controller.DefaultCharacterStrengthToMassRatio;
            //TODO: Reset weapon mass?
        }

        private void UpdateValues()
        {
            values.text =
                "\n" +
                "\n" +
                $"{CombatSettings.Controller.MaximumVelocity}\n" +
                $"\n" +
                $"{CombatSettings.Controller.MaximumAngularVelocity}\n" +
                "\n" +
                $"{CombatSettings.Controller.CharacterStrengthToMassRatio}\n" +
                "\n" +
                $"{GetWeaponMass(HandSide.Left)}\n" + 
                "\n" +
                $"{GetWeaponMass(HandSide.Right)}";
        }

        private float GetWeaponMass(HandSide handSide)
        {
            var controllerLink = ControllerLink.Get(handSide);

            if (controllerLink == null || !controllerLink.isHoldingItem)
            {
                return 0;
            }

            return controllerLink.connectedItem.GetComponent<Rigidbody>().mass;
        }

        private void SetWeaponMass(HandSide handSide, float value)
        {
            var controllerLink = ControllerLink.Get(handSide);

            if (controllerLink == null || !controllerLink.isHoldingItem)
            {
                return;
            }

            var rigidBody = controllerLink.connectedItem.GetComponent<Rigidbody>();
            rigidBody.mass = value.Round(2);
            UpdateValues();
        }

        protected override bool TryInitialize()
        {
            SetInitialValues();

            maximumVelocitySlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Controller.MaximumVelocity = value.Round(DecimalPlaces); UpdateValues(); });
            maximumAngularVelocitySlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Controller.MaximumAngularVelocity = value.Round(DecimalPlaces); UpdateValues(); });
            characterStrengthToMassRatioSlider.onValueChanged.AddListener(delegate (float value) { CombatSettings.Controller.CharacterStrengthToMassRatio = value.Round(DecimalPlaces); UpdateValues(); });
            leftWeaponMassSlider.onValueChanged.AddListener(delegate (float value) { SetWeaponMass(HandSide.Left, value); });
            rightWeaponMassSlider.onValueChanged.AddListener(delegate (float value) { SetWeaponMass(HandSide.Right, value); });

            UpdateValues();

            return true;
        }
    }
}