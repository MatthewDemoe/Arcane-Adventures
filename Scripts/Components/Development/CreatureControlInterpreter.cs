using UnityEngine;
using System.Collections;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using RootMotion;
using RootMotion.Demos;
using RootMotion.Dynamics;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development {
	public class CreatureControlInterpreter : MonoBehaviour
	{
		private const float airborneThreshold = 0.5f;
        private const float spherecastRadius = 0.1f;
        private const bool smoothPhysics = true;
        private const float smoothAccelerationTime = 0.2f;
        private const float linearAccelerationSpeed = 2f;
        private const float platformFriction = 7f;
        private const float groundStickyEffect = 7f;
        private const float maxVerticalVelocityOnGround = 3f;
        private const float velocityToGroundTangentWeight = 0f;
        private const bool lookInCameraDirection = false;
        private const bool smoothJump = true;
        private const float airSpeed = 6f;
        private const float airControl = 3f;
        private const float jumpPower = 6f;
        private const float jumpRepeatDelayTime = 0f;
        private const float doubleJumpPowerMlp = 1f;
        private const float crouchCapsuleScaleMlp = 0.6f;
        
        private LayerMask groundLayers;
        private PhysicMaterial zeroFrictionMaterial;
		private PhysicMaterial highFrictionMaterial;
		private float originalHeight;
		private Vector3 originalCenter;
		private CapsuleCollider capsule;
		private Vector3 moveDirection;
		private Vector3 normal;
		private Vector3 platformVelocity;
		private RaycastHit hit;
		private float jumpLeg;
		private float jumpEndTime;
		private float forwardMlp;
		private float groundDistance;
		private float lastAirTime;
		private float stickyForce;
		private Vector3 moveDirectionVelocity;
		private float wallRunWeight;
		private float lastWallRunWeight;
		private float fixedDeltaTime;
		private Vector3 fixedDeltaPosition;
		private Quaternion fixedDeltaRotation = Quaternion.identity;
		private bool fixedFrame;
		private float wallRunEndTime;
		private Vector3 gravity;
		private Vector3 verticalVelocity;
		private float velocityY;
		private bool doubleJumped;
		private bool jumpReleased;

		protected Rigidbody r;
		protected Animator animator;

		

		public ICreatureController creatureController;
		public CharacterAnimationBase characterAnimation { get; private set; }
		public bool doubleJumpEnabled { get; private set; }
		
		public struct AnimState {
			public Vector3 moveDirection;
			public bool jump;
			public bool crouch;
			public bool onGround;
			public float yVelocity;
			public bool doubleJump;
			public float moveSpeed;
		}
		
		public BehaviourPuppet puppet { get; private set; }
		
		private bool IsPuppetInControl => puppet.state == BehaviourPuppet.State.Puppet;

        public bool fullRootMotion { get; set; }
        public bool onGround { get; private set; }
        
        public AnimState animState;

		private Vector3 jumpDirection => r.velocity.normalized;
		
		protected virtual void Start() {
			groundLayers = LayerMask.GetMask(nameof(Layers.Walkable), nameof(Layers.Environment), nameof(Layers.Default));
			capsule = GetComponent<Collider>() as CapsuleCollider;
			r = GetComponent<Rigidbody>();

			originalHeight = capsule.height;
			originalCenter = capsule.center;

			zeroFrictionMaterial = new PhysicMaterial();
			zeroFrictionMaterial.dynamicFriction = 0f;
			zeroFrictionMaterial.staticFriction = 0f;
			zeroFrictionMaterial.frictionCombine = PhysicMaterialCombine.Minimum;
			zeroFrictionMaterial.bounciness = 0f;
			zeroFrictionMaterial.bounceCombine = PhysicMaterialCombine.Minimum;

			highFrictionMaterial = new PhysicMaterial();

			r.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			characterAnimation = GetComponentInChildren<CharacterAnimationBase>();
			animator = GetComponent<Animator>();
			if (animator == null) animator = characterAnimation.GetComponent<Animator>();

			onGround = true;
			animState.onGround = true;
			puppet = transform.parent.GetComponentInChildren<BehaviourPuppet>();
		}

		protected RaycastHit GetSpherecastHit() {
			Vector3 up = transform.up;
			Ray ray = new Ray (r.position + up * airborneThreshold, -up);
			RaycastHit h = new RaycastHit();
			h.point = transform.position - transform.transform.up * airborneThreshold;
			h.normal = transform.up;

			Physics.SphereCast(ray, spherecastRadius, out h, airborneThreshold * 2f, groundLayers);
			return h;
		}

		protected void ScaleCapsule (float mlp) {
			if (capsule.height != originalHeight * mlp) {
				capsule.height = Mathf.MoveTowards (capsule.height, originalHeight * mlp, Time.deltaTime * 4);
				capsule.center = Vector3.MoveTowards (capsule.center, originalCenter * mlp, Time.deltaTime * 2);
			}
		}

		protected void HighFriction() {
			capsule.material = highFrictionMaterial;
		}

		protected void ZeroFriction() {
			capsule.material = zeroFrictionMaterial;
		}

		void OnAnimatorMove() {
			Move (animator.deltaPosition, animator.deltaRotation);
		}

		// When the Animator moves
		public void Move(Vector3 deltaPosition, Quaternion deltaRotation)
		{
			if (!IsPuppetInControl)
				//targetPosition = Vector3.zero;
				return;
			
            // Accumulate delta position, update in FixedUpdate to maintain consitency
            fixedDeltaTime += Time.deltaTime;
			fixedDeltaPosition += deltaPosition;
			fixedDeltaRotation *= deltaRotation;
		}

        void FixedUpdate() {
            gravity = fullRootMotion? Vector3.zero: Physics.gravity;

			verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
			velocityY = verticalVelocity.magnitude;
			if (Vector3.Dot(verticalVelocity, gravity) > 0f) velocityY = -velocityY;

			// Smoothing out the fixed time step
			r.interpolation = smoothPhysics? RigidbodyInterpolation.Interpolate: RigidbodyInterpolation.None;
			characterAnimation.smoothFollow = smoothPhysics;

            // Move
			MoveFixed(fixedDeltaPosition);

            fixedDeltaTime = 0f;
			fixedDeltaPosition = Vector3.zero;

			r.MoveRotation(transform.rotation * fixedDeltaRotation);
			fixedDeltaRotation = Quaternion.identity;

			Rotate();

			GroundCheck (); // detect and stick to ground

			// Friction
			if (creatureController.targetPosition == Vector3.zero && groundDistance < airborneThreshold * 0.5f) HighFriction();
			else ZeroFriction();

			var stopSlide = !fullRootMotion && onGround && creatureController.targetPosition == Vector3.zero && r.velocity.magnitude < 0.5f && groundDistance < airborneThreshold * 0.5f;

			r.useGravity = !stopSlide;
			
			if (stopSlide)
				r.velocity = Vector3.zero;

			if (onGround) {
				// Jumping
				animState.jump = Jump();
				jumpReleased = false;
				doubleJumped = false;
			} else {
				if (!creatureController.jump) jumpReleased = true;

				//r.AddForce(gravity * gravityMultiplier);
				if (jumpReleased && creatureController.jump && !doubleJumped && doubleJumpEnabled) {
					jumpEndTime = Time.time + 0.1f;
					animState.doubleJump = true;

					Vector3 jumpVelocity = jumpDirection * airSpeed;
					r.velocity = jumpVelocity;
					r.velocity += transform.up * jumpPower * doubleJumpPowerMlp;
					doubleJumped = true;
				}
			}

			// Scale the capsule colllider while crouching
			ScaleCapsule(creatureController.crouch? crouchCapsuleScaleMlp: 1f);

			fixedFrame = true;
        }

        protected virtual void Update() {
            // Fill in animState
			animState.onGround = onGround;
			animState.moveDirection = GetMoveDirection();
			animState.moveSpeed = creatureController.targetSpeed;
			animState.yVelocity = Mathf.Lerp(animState.yVelocity, velocityY, Time.deltaTime * 10f);
			animState.crouch = creatureController.crouch;
		}

		private void MoveFixed(Vector3 deltaPosition) {
            Vector3 velocity = fixedDeltaTime > 0f? deltaPosition / fixedDeltaTime: Vector3.zero;

            // Add velocity of the rigidbody the character is standing on
            if (!fullRootMotion)
            {
                velocity += V3Tools.ExtractHorizontal(platformVelocity, gravity, 1f);

                if (onGround)
                {
                    // Rotate velocity to ground tangent
                    if (velocityToGroundTangentWeight > 0f)
                    {
                        Quaternion rotation = Quaternion.FromToRotation(transform.up, normal);
                        velocity = Quaternion.Lerp(Quaternion.identity, rotation, velocityToGroundTangentWeight) * velocity;
                    }
                }
                else
                {
                    // Air move
                    //Vector3 airMove = new Vector3 (userControl.state.move.x * airSpeed, 0f, userControl.state.move.z * airSpeed);
                    Vector3 airMove = V3Tools.ExtractHorizontal(jumpDirection * airSpeed, gravity, 1f);
                    velocity = Vector3.Lerp(r.velocity, airMove, Time.deltaTime * airControl);
                }

                if (onGround && Time.time > jumpEndTime)
                {
                    r.velocity = r.velocity - transform.up * stickyForce * Time.deltaTime;
                }

                // Vertical velocity
                Vector3 verticalVelocity = V3Tools.ExtractVertical(r.velocity, gravity, 1f);
                Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(velocity, gravity, 1f);

                if (onGround)
                {
                    if (Vector3.Dot(verticalVelocity, gravity) < 0f)
                    {
                        verticalVelocity = Vector3.ClampMagnitude(verticalVelocity, maxVerticalVelocityOnGround);
                    }
                }

                r.velocity = horizontalVelocity + verticalVelocity;
            } else
            {
                r.velocity = velocity;
            }

            // Dampering forward speed on the slopes (Not working since Unity 2017.2)
            //float slopeDamper = !onGround? 1f: GetSlopeDamper(-deltaPosition / Time.deltaTime, normal);
            //forwardMlp = Mathf.Lerp(forwardMlp, slopeDamper, Time.deltaTime * 5f);
            forwardMlp = 1f;
		}


		// Get the move direction of the character relative to the character rotation
		private Vector3 GetMoveDirection()
		{
				moveDirection = Vector3.SmoothDamp(moveDirection, creatureController.targetPosition, ref moveDirectionVelocity, smoothAccelerationTime);
				moveDirection = Vector3.MoveTowards(moveDirection, creatureController.targetPosition, Time.deltaTime * linearAccelerationSpeed);
				return transform.InverseTransformDirection(moveDirection);
		}

		protected virtual void Rotate()
		{
			if (!IsPuppetInControl)
				return;

			//TODO: Consider adopting what makes sense in the commented out code below with the de facto rotation function (or remote it).
			/*if (gravityTarget != null) r.MoveRotation (Quaternion.FromToRotation(transform.up, transform.position - gravityTarget.position) * transform.rotation);
			if (platformAngularVelocity != Vector3.zero) r.MoveRotation (Quaternion.Euler(platformAngularVelocity) * transform.rotation);

			float angle = GetAngleFromForward(GetForwardDirection());
			
			if (userControl.state.move == Vector3.zero) angle *= (1.01f - (Mathf.Abs(angle) / 180f)) * stationaryTurnSpeedMlp;

			// Rotating the character
			//RigidbodyRotateAround(characterAnimation.GetPivotPoint(), transform.up, angle * Time.deltaTime * turnSpeed);
			r.MoveRotation(Quaternion.AngleAxis(angle * Time.deltaTime * turnSpeed, transform.up) * r.rotation);*/
		}

		// Which way to look at?
		private Vector3 GetForwardDirection() {
			bool isMoving = creatureController.targetPosition != Vector3.zero;

				if (isMoving) return creatureController.lookPosition - r.position;
				return lookInCameraDirection? creatureController.lookPosition - r.position: transform.forward;
		}

		protected bool Jump() {
			if (!IsPuppetInControl)
				return false;
			
			// check whether conditions are right to allow a jump:
			if (!creatureController.jump) return false;
			if (creatureController.crouch) return false;
			if (!characterAnimation.animationGrounded) return false;
			if (Time.time < lastAirTime + jumpRepeatDelayTime) return false;

			// Jump
			onGround = false;
			jumpEndTime = Time.time + 0.1f;

            Vector3 jumpVelocity = jumpDirection * airSpeed;
            jumpVelocity += transform.up * jumpPower;

            if (smoothJump)
            {
                StopAllCoroutines();
                StartCoroutine(JumpSmooth(jumpVelocity - r.velocity));
            } else
            {
                r.velocity = jumpVelocity;
            }

            return true;
		}

        // Add jump velocity smoothly to avoid puppets launching to space when unpinned during jump acceleration
        private IEnumerator JumpSmooth(Vector3 jumpVelocity)
        {
            int steps = 0;
            int stepsToTake = 3;
            while (steps < stepsToTake)
            {
                r.AddForce((jumpVelocity) / stepsToTake, ForceMode.VelocityChange);
                steps++;
                yield return new WaitForFixedUpdate();
            }
        }

		// Is the character grounded?
		private void GroundCheck () {
			Vector3 platformVelocityTarget = Vector3.zero;
			float stickyForceTarget = 0f;

			// Spherecasting
			hit = GetSpherecastHit();

			//normal = hit.normal;
			normal = transform.up;
			//groundDistance = r.position.y - hit.point.y;
			groundDistance = Vector3.Project(r.position - hit.point, transform.up).magnitude;

			// if not jumping...
			bool findGround = Time.time > jumpEndTime && velocityY < jumpPower * 0.5f;

			if (findGround) {
				bool g = onGround;
				onGround = false;

				// The distance of considering the character grounded
				float groundHeight = !g? airborneThreshold * 0.5f: airborneThreshold;

				//Vector3 horizontalVelocity = r.velocity;
				Vector3 horizontalVelocity = V3Tools.ExtractHorizontal(r.velocity, gravity, 1f);

				float velocityF = horizontalVelocity.magnitude;

				if (groundDistance < groundHeight) {
					// Force the character on the ground
					stickyForceTarget = groundStickyEffect * velocityF * groundHeight;

					// On moving platforms
					if (hit.rigidbody != null) {
						platformVelocityTarget = hit.rigidbody.GetPointVelocity(hit.point);
					}

					// Flag the character grounded
					onGround = true;
				}
			}

			// Interpolate the additive velocity of the platform the character might be standing on
			platformVelocity = Vector3.Lerp(platformVelocity, platformVelocityTarget, Time.deltaTime * platformFriction);
            if (fullRootMotion) stickyForce = 0f;

            stickyForce = stickyForceTarget;//Mathf.Lerp(stickyForce, stickyForceTarget, Time.deltaTime * 5f);

			// remember when we were last in air, for jump delay
			if (!onGround) lastAirTime = Time.time;
		}
	}
}
