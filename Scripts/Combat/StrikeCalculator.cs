using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Combat
{
    public class StrikeCalculator
    {
        private Vector3 lastControllerPosition;
        private bool handIsThrusting;
        private bool weaponIsThrusting;
        private float timeBeforeThrustExpires;

        private Quaternion lastControllerRotation;
        private bool handIsFlicking;
        private bool weaponIsFlicking;
        private float timeBeforeFlickExpires;

        private bool weaponIsMoving = false;
        bool weaponWasMoving = false;

        public delegate void WeaponMoveEvent(bool weaponIsMoving);
        public WeaponMoveEvent OnWeaponMoveStateChanged = (weaponIsMoving) => { };

        public Strike currentStrike { get; private set; }

        private WeaponSoundPlayer weaponSoundPlayer;
        private GameObject target;

        public Strike hitStrike => currentStrike is Strike && currentStrike.hit ? currentStrike : null;
        public StrikeType currentStrikeType => currentStrike is Strike && !currentStrike.isFinished ?
            currentStrike.strikeType : StrikeType.NotStrike;
        public Guid? currentStrikeGuid => currentStrike?.strikeGuid;
        public void SetTarget(GameObject newTarget) => target = newTarget;
        public void ReportHit() => currentStrike.ReportHit();

        public StrikeCalculator(WeaponSoundPlayer weaponSoundPlayer)
        {
            this.weaponSoundPlayer = weaponSoundPlayer;
        }

        public void Calculate()
        {
            if (target == null)
                return;

            weaponWasMoving = weaponIsMoving;

            UpdateThrust();
            UpdateFlick();
            currentStrike?.Update(weaponIsThrusting, weaponIsFlicking);

            if (weaponIsMoving != weaponWasMoving)
                OnWeaponMoveStateChanged.Invoke(weaponIsMoving);
        }

        private void UpdateThrust()
        {
            var controllerPositionDiff = target.transform.localPosition - lastControllerPosition;
            var targetVelocity = Vector3.Scale(controllerPositionDiff, CombatSettings.Strikes.AxisScale).magnitude;
            lastControllerPosition = target.transform.localPosition;
            var wasHandThrustingLastUpdate = handIsThrusting;
            handIsThrusting = targetVelocity > CombatSettings.Strikes.ThrustThreshold;

            weaponIsMoving = targetVelocity > CombatSettings.Strikes.ThrustThreshold / CombatSettings.Strikes.DefaultMoveThresholdDenominator;

            if (handIsThrusting)
            {
                timeBeforeThrustExpires = CombatSettings.Strikes.ThrustExpirationTime;

                if (!wasHandThrustingLastUpdate)
                {
                    weaponIsThrusting = true;
                    StartNewStrike();
                    currentStrike.SetAsThrusting();
                }
            }

            else if (weaponIsThrusting)
            {
                timeBeforeThrustExpires -= Time.deltaTime;

                if (timeBeforeThrustExpires <= 0)
                    weaponIsThrusting = false;
            }
        }

        private void UpdateFlick()
        {
            var rotationDifference = Quaternion.Angle(target.transform.localRotation, lastControllerRotation);
            lastControllerRotation = target.transform.localRotation;
            var wasHandFlickingLastUpdate = handIsFlicking;
            handIsFlicking = rotationDifference > CombatSettings.Strikes.FlickThreshold;

            if (!weaponIsMoving)
                weaponIsMoving = rotationDifference > CombatSettings.Strikes.FlickThreshold / CombatSettings.Strikes.DefaultMoveThresholdDenominator;

            if (handIsFlicking)
            {
                timeBeforeFlickExpires = CombatSettings.Strikes.FlickExpirationTime;

                if (!wasHandFlickingLastUpdate)
                {
                    weaponIsFlicking = true;
                    StartNewStrike();
                }
            }
            else if (weaponIsFlicking)
            {
                timeBeforeFlickExpires -= Time.deltaTime;

                if (timeBeforeFlickExpires <= 0)
                    weaponIsFlicking = false;
            }
        }

        private void StartNewStrike()
        {
            if (currentStrikeType.Equals(StrikeType.NotStrike))
            {
                currentStrike = new Strike(target.transform);

                if (weaponSoundPlayer != null)
                    weaponSoundPlayer.PlayStrikeSound(currentStrikeType);                
            }
        }

        public enum StrikeMovement
        {
            None,
            Stab,
            Slash
        }
    }
}