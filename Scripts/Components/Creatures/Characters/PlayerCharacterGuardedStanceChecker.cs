using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using Injection;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public class PlayerCharacterGuardedStanceChecker : MonoBehaviour
    {
        private float timeOfLastFailedCheckInSeconds;
        private Vector3 positionAtLastFailedCheck;
        private CreatureEffect currentGuardedStance;

        [Inject] protected ICombatSystem combatSystem;
        [Inject] protected PlayerCharacterReference playerCharacterReference;

        private bool isInGuardedStance => currentGuardedStance is CreatureEffect;

        private void Awake()
        {
            InjectorContainer.Injector.Inject(this);
        }

        private void Update()
        {
            if (HasMovedSignificantly() || IsStriking())
                HandleFailedCheck();
            else if (!isInGuardedStance && HasBeenStillForTheRequiredTime())
                AddGuardedStance();
        }

        private void HandleFailedCheck()
        {
            timeOfLastFailedCheckInSeconds = Time.time;
            positionAtLastFailedCheck = transform.position;

            if (isInGuardedStance)
                RemoveGuardedStance();
        }

        private bool IsStriking()
        {
            var leftHandWeapon = playerCharacterReference.playerItemEquipper.GetWeaponInHand(HandSide.Left);
            var rightHandWeapon = playerCharacterReference.playerItemEquipper.GetWeaponInHand(HandSide.Right);

            return (leftHandWeapon != null && leftHandWeapon.isStriking) ||
                (rightHandWeapon != null && rightHandWeapon.isStriking);
        }

        private bool HasMovedSignificantly() => Vector3.Distance(transform.position, positionAtLastFailedCheck) > combatSystem.settings.guardedStanceMovementThresholdInMeters;
        private bool HasBeenStillForTheRequiredTime() => (Time.time - timeOfLastFailedCheckInSeconds) > combatSystem.settings.guardedStanceTimeToStandStillInSeconds;

        private void RemoveGuardedStance()
        {
            playerCharacterReference.creature.modifiers.RemoveEffect(currentGuardedStance);
            currentGuardedStance = null;
        }

        private void AddGuardedStance()
        {
            currentGuardedStance = combatSystem.creatureEffectFactory.BuildGuardedStance();
            playerCharacterReference.creature.modifiers.AddEffect(currentGuardedStance);
        }
    }
}