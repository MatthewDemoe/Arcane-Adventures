using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PlayerDash : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        UnityEvent OnDashComplete = new UnityEvent();

        [SerializeField]
        bool isGradual = false;

        private Rigidbody playerRigidbody;

        Vector3 dashDirection => (targetPoint - playerRigidbody.transform.position).normalized;
        Vector3 targetPoint;


        float dashMagnitude = 0.0f;

        float timeout = 3.0f;

        SpellTargeter spellTargeter;
        public PhysicalSpell physicalSpell { get; set; }

        PlayerCharacterMovementController playerMovementController;

        const float AccelerationMultiplier = 1.0f;
        const float MaximumTerminalVelocity = 15.0f;
        const float MinimumTerminalVelocity = 5.0f;


        private void Awake()
        {
            playerRigidbody = PlayerCharacterReference.Instance.GetComponent<Rigidbody>();
            playerMovementController = PlayerCharacterReference.Instance.GetComponent<PlayerCharacterMovementController>();

            spellTargeter = GetComponentInParent<SpellTargeter>();
        }

        public void Dash()
        {
            if (!physicalSpell.correspondingSpell.GetCaster().isMovementEnabled)
                return;

            GetTargetDirection();
            StartCoroutine(DashRoutine());
        }

        private void GetTargetDirection()
        {
            if(spellTargeter == null)
                spellTargeter = GetComponentInParent<SpellTargeter>();

            targetPoint = (spellTargeter as SpellPointTargeter).targetPoint;
            dashMagnitude = (spellTargeter as SpellPointTargeter).fromSelfToTargetPoint.magnitude;
        }

        IEnumerator DashRoutine()
        {
            float startTime = Time.time;

            yield return new WaitUntil(() => playerMovementController.characterIsOnTheGround || (Time.time - startTime) > timeout);

            if ((Time.time - startTime) > timeout)
            {
                OnDashComplete.Invoke();
                StopCoroutine(DashRoutine());
            }

            startTime = Time.time;

            playerMovementController.isDashing = true;

            Vector3 startPosition = playerRigidbody.position;
            Vector3 dashAcceleration = dashDirection * AccelerationMultiplier;

            float remainingTime;
            float displacementAtCurrentVelocity;

            do
            {
                remainingTime = Time.time - startTime;
                displacementAtCurrentVelocity = (playerRigidbody.velocity.magnitude * remainingTime) + (dashAcceleration.magnitude * (remainingTime * remainingTime) / 2.0f);

                AddAppropriateForce(startPosition, dashAcceleration);

                yield return null;
            } while ((displacementAtCurrentVelocity <= dashMagnitude) && remainingTime < timeout);

            yield return new WaitUntil(() => (Vector3.Distance(startPosition, playerRigidbody.position) >= dashMagnitude) || (Time.time - startTime) > timeout);

            playerMovementController.isDashing = false;

            OnDashComplete.Invoke();
        }

        private void AddAppropriateForce(Vector3 startPosition, Vector3 dashAcceleration)
        {
            float normalizedTerminalVelocity = UtilMath.Lmap(Vector3.Distance(startPosition, transform.position), 0.0f, dashMagnitude, MinimumTerminalVelocity, MaximumTerminalVelocity * (isGradual ? 2.0f : 1.0f));
           
            float terminalVelocity = isGradual ? normalizedTerminalVelocity : MaximumTerminalVelocity;

            if (playerRigidbody.velocity.magnitude <= terminalVelocity)
            {
                playerRigidbody.AddForce(dashAcceleration, ForceMode.VelocityChange);
            }
        }
    }
}
