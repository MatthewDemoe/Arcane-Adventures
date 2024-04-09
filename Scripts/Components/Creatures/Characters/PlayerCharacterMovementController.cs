using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using UnityEngine;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public class PlayerCharacterMovementController : MonoBehaviour
    {
        private const float MaxHorizontalMovementVelocityChange = 0.2f;
        private const float MinHorizontalMovementInputMagnitude = 0.15f;
        private const float HorizontalInputDrag = 0.95f;
        private const float BaseJumpForce = 4000;
        private const float MinimumTimeBetweenJumpsInSeconds = 0.5f;
        private const float MaxGroundDistanceForJump = 0.01f;
        private const float HeadsetBasedMovementForceMultiplier = 20000;
        private const float HeadsetBasedMovementMaxForce = 21;

        [SerializeField] private CapsuleCollider bodyCollider;

        private Rigidbody rigidBody;
        private Camera mainCamera;
        private float maximumRotationSpeed = 2;
        private float rotationAcceleration = 0.1f;

        private Vector3 lastHorizontalMovementInput = new Vector3();
        private LayerMask environmentLayerMask;
        private float strength => 0;//TODO: Get from character.
        private float movementAndJumpReductionInPercentage => 0;//TODO: Get from armor.
        private float horizontalMovementSpeed => 4 * creatureReference.creature.modifiers.effects.Select((effect) => effect.moveSpeed).Product();//TODO: Get from armor.

        private float ClampToMaxHorizontalMovementVelocityChange(float value) => Mathf.Clamp(value, -MaxHorizontalMovementVelocityChange, MaxHorizontalMovementVelocityChange);

        private Vector2 movementInput;


        private bool jumpWasTriggered = false;
        private float rotationInput;
        private float timeUntilNextJumpIsPossibleInSeconds = 0;

        private Vector3 lastHeadsetPosition;

        CreatureReference creatureReference;

        public bool isDashing = false;
        public bool characterIsOnTheGround => Physics.SphereCast(transform.TransformPoint(bodyCollider.center), bodyCollider.radius, Vector3.down, out _, bodyCollider.center.y + MaxGroundDistanceForJump, environmentLayerMask);

        public float rotationSpeed { get; private set; }//TODO: Expose to enable movement vignette experiment - close again after experiment is over.

        private void Start()
        {
            environmentLayerMask = LayerMask.GetMask(nameof(Layers.Environment), nameof(Layers.Default));//TODO: Default is only here because everything that's environment is not yet set to environment.
            rigidBody = GetComponent<Rigidbody>();
            mainCamera = Camera.main;

            creatureReference = GetComponent<CreatureReference>();
        }

        private void FixedUpdate()
        {
            if (isDashing)
                return;

            HandleManualCharacterRotation();
            AddHorizontalMovementForce();
            AddVerticalMovementForce();
        }

        public void SetMovementInput(Vector2 movement, bool forceMove = false) => movementInput = creatureReference.creature.isMovementEnabled ? movement : forceMove ? movement : Vector2.zero;
        public void SetRotationInput(float rotation) => rotationInput = rotation;
        public void Jump() => jumpWasTriggered = true;

        private void HandleManualCharacterRotation()
        {
            var targetRotationSpeed = maximumRotationSpeed * rotationInput;

            if (targetRotationSpeed > 0)
            {
                if (rotationSpeed < targetRotationSpeed)
                {
                    rotationSpeed = Mathf.Min(rotationSpeed + rotationAcceleration, targetRotationSpeed);
                }
                else
                {
                    rotationSpeed = Mathf.Max(rotationSpeed - rotationAcceleration, targetRotationSpeed);
                }
            }
            else
            {
                if (rotationSpeed > targetRotationSpeed)
                {
                    rotationSpeed = Mathf.Max(rotationSpeed - rotationAcceleration, targetRotationSpeed);
                }
                else
                {
                    rotationSpeed = Mathf.Min(rotationSpeed + rotationAcceleration, targetRotationSpeed);
                }
            }

            gameObject.transform.RotateAround(this.transform.position, gameObject.transform.up, rotationSpeed);
        }

        private void AddHorizontalMovementForce()
        {
            var input = new Vector3(movementInput.x, 0, movementInput.y);

            if (input.magnitude < MinHorizontalMovementInputMagnitude)
            {
                input = lastHorizontalMovementInput * HorizontalInputDrag;
                //TODO: Disable until implementation is improved: ApplyHeadsetBasedMovementForce();
            }

            ApplyHorizontalMovementForce(input);
        }

        private void ApplyHeadsetBasedMovementForce()
        {
            var position = XRReferences.Instance.headsetPosition;
            position.y = 0;
            var distance = Vector3.Distance(lastHeadsetPosition, position);
            var targetDirection = (position - lastHeadsetPosition).normalized;
            targetDirection = transform.rotation * targetDirection;
            var force = targetDirection * distance * HeadsetBasedMovementForceMultiplier;
            force = Vector3.ClampMagnitude(force, HeadsetBasedMovementMaxForce);
            rigidBody.AddForce(force, ForceMode.Acceleration);
            lastHeadsetPosition = position;
        }

        private void ApplyHorizontalMovementForce(Vector3 input, bool headsetDirection = true)
        {
            var direction = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0);

            var targetVelocity = (headsetDirection && creatureReference.creature.isMovementEnabled ? direction : Quaternion.identity) * input * horizontalMovementSpeed;

            var velocityChangeX = ClampToMaxHorizontalMovementVelocityChange(targetVelocity.x - rigidBody.velocity.x);
            var velocityChangeZ = ClampToMaxHorizontalMovementVelocityChange(targetVelocity.z - rigidBody.velocity.z);
            var velocityChange = new Vector3(velocityChangeX, 0, velocityChangeZ);
            rigidBody.AddForce(velocityChange, ForceMode.VelocityChange);

            if(headsetDirection)
                lastHorizontalMovementInput = input;
        }

        private void AddVerticalMovementForce()
        {
            if (jumpWasTriggered && timeUntilNextJumpIsPossibleInSeconds == 0 && characterIsOnTheGround)
            {
                var jumpForce = BaseJumpForce * (1 + ((strength - movementAndJumpReductionInPercentage) * 0.01f));
                rigidBody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                timeUntilNextJumpIsPossibleInSeconds = MinimumTimeBetweenJumpsInSeconds;
            }

            if (timeUntilNextJumpIsPossibleInSeconds > 0)
            {
                timeUntilNextJumpIsPossibleInSeconds = Mathf.Max(timeUntilNextJumpIsPossibleInSeconds - Time.fixedDeltaTime, 0);
            }

            jumpWasTriggered = false;
        }
    }
}