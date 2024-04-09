using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.Creatures.UMA;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Movement;
using com.AlteredRealityLabs.ArcaneAdventures.Player.Hands;
using System.Collections.Generic;
using UMA;
using UMA.CharacterSystem;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    public class PlayerAvatar : MonoBehaviour
    {
        private const float GripAnimationRadiusMultiplier = 15;

        private static readonly Dictionary<Identifiers.Race, float> HeightMultiplierByRace = new Dictionary<Identifiers.Race, float>()
        {
            { Identifiers.Race.Human, 1.61f },
            { Identifiers.Race.Elf, 1.45f },
            { Identifiers.Race.Ghost, 1.61f }
        };

        private const float MinimumHeight = 0.85f;

        private Rigidbody rigidBody;
        private DynamicCharacterAvatar dynamicCharacterAvatar;
        private PlayerHandAnimationController playerLeftHandAnimationController;
        private PlayerHandAnimationController playerRightHandAnimationController;
        private Dictionary<string, DnaSetter> dna;
        private Dictionary<string, Transform> transformsByBoneName = new Dictionary<string, Transform>();
        private CameraOffsetUpdater cameraOffsetUpdater;

        public bool showHead = false;
        public bool autoRotateToCamera = true;

        public bool isAvatarReady { get; private set; } = false;

        public UnityEvent OnCharacterUpdated = new UnityEvent();

        private PlayerHandAnimationController GetPlayerHandAnimationController(HandSide handSide)
            => handSide.Equals(HandSide.Left) ? playerLeftHandAnimationController : playerRightHandAnimationController;

        public Transform headTransform => GetBoneTransform(UmaBones.Head);
        public Transform rightArmTransform => GetBoneTransform(UmaBones.RightArm);
        public Transform leftArmTransform => GetBoneTransform(UmaBones.LeftArm);
        public Transform leftFootTransform => GetBoneTransform(UmaBones.LeftFoot);

        public Vector3 midPointPosition => headTransform.position - (Vector3.up * GetHeadToFootDistance() / 2.0f);

        public float GetHeadToFootDistance()
        {
            return isAvatarReady ?
                Mathf.Abs(headTransform.position.y - leftFootTransform.position.y) :
                0;
        }

        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            dynamicCharacterAvatar = GetComponent<DynamicCharacterAvatar>();
            cameraOffsetUpdater = XRReferences.Instance.GetComponentInChildren<CameraOffsetUpdater>();

            var animator = GetComponent<Animator>();
            playerLeftHandAnimationController = new PlayerHandAnimationController(animator, HandSide.Left);
            playerRightHandAnimationController = new PlayerHandAnimationController(animator, HandSide.Right);

            if (PlayerCharacterReference.Instance != null && !PlayerCharacterReference.Instance.isUsingPuppetMaster)
                dynamicCharacterAvatar.CharacterUpdated.AddListener(OnCharacterBegunCallback);
        }

        private void OnCharacterBegunCallback(UMAData umaData)
        {
            dna = dynamicCharacterAvatar.GetDNA();
            isAvatarReady = true;
            OnCharacterUpdated.Invoke();
        }

        private Transform GetBoneTransform(string boneName)
        {
            if (transformsByBoneName.TryGetValue(boneName, out var transform))
            {
                return transform;
            }

            transform = dynamicCharacterAvatar?.umaData?.GetBoneGameObject(boneName)?.transform;

            if (transform is Transform)
            {
                transformsByBoneName.Add(boneName, transform);
            }

            return transform;
        }

        public void SetStaticGrip(HandSide handSide, float stopPoint)
            => GetPlayerHandAnimationController(handSide).SetStaticGrip(stopPoint);
        public void ReleaseGrip(HandSide handSide)
            => GetPlayerHandAnimationController(handSide).ReleaseGrip();
        public void Grip(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            var stopAtNormalizedTime = 1 - (physicalHandHeldItem.gripRadius * GripAnimationRadiusMultiplier);
            GetPlayerHandAnimationController(handSide).Grip(stopAtNormalizedTime);
        }

        public float GetNormalizedHeight()
        {
            return isAvatarReady && dna.TryGetValue(DnaName.height.ToString(), out var dnaSetter) ? 
                dnaSetter.Value : 0;
        }

        public void SetNormalizedHeight(float value)
        {
            if (dna.TryGetValue(DnaName.height.ToString(), out var dnaSetter))
            {
                dnaSetter.Set(value);
                dynamicCharacterAvatar.ForceUpdate(true);
                dna = null;
            }
        }

        public void SetHandSize(float value)
        {
            if (dna.TryGetValue(DnaName.handsSize.ToString(), out var dnaSetter))
            {
                dnaSetter.Set(value);
                dynamicCharacterAvatar.ForceUpdate(true);
                dna = null;
            }
        }

        public float GetHeight()
        {
            var normalizedHeight = GetNormalizedHeight();
            var heightMultiplier = HeightMultiplierByRace[PlayerCharacterReference.Instance.creature.race];
            var height = MinimumHeight + (heightMultiplier * normalizedHeight);

            return height;
        }

        private void Update()
        {
            if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
                return;
            
            if (headTransform != null)
            {
                EnsureHeadScale();
            }

            if (autoRotateToCamera)
            {
                AutoRotateToCamera();
            }

            playerLeftHandAnimationController.Update();
            playerRightHandAnimationController.Update();
        }

        private void EnsureHeadScale()
        {
            var targetHeadScale = showHead ? Vector3.one : Vector3.zero;

            if (isAvatarReady && headTransform.localScale != targetHeadScale)
            {
                headTransform.localScale = targetHeadScale;
            }
        }

        private void AutoRotateToCamera()
        {
            var mainCameraY = Camera.main.transform.rotation.eulerAngles.y;
            var targetRotation = Quaternion.Euler(0, mainCameraY, 0);
            var amountLookedDown = Vector3.Dot(Camera.main.transform.forward, Vector3.down);
            var oldYRotation = transform.rotation.eulerAngles.y;
            var rotationSpeed = amountLookedDown > MovementSettings.FacedownStart ?
                GetRotationSpeedBasedOnMovementAndAngleDifference(mainCameraY) :
                MovementSettings.FaceupAutoRotateSpeed;

            if (rotationSpeed == 0) { return; }

            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
            var yChange = Mathf.DeltaAngle(transform.rotation.eulerAngles.y, oldYRotation);
            cameraOffsetUpdater.ReportAutoRotation(yChange);
        }

        private float GetRotationSpeedBasedOnMovementAndAngleDifference(float mainCameraY)
        {
            float yAngleDifference = Mathf.Abs(Mathf.DeltaAngle(transform.rotation.eulerAngles.y, mainCameraY));
            var rotationSpeed = 0f;
            var playerVelocity = rigidBody.velocity;
            playerVelocity.y = 0;

            if (playerVelocity.magnitude > 0.1f)
            {
                rotationSpeed = playerVelocity.magnitude * MovementSettings.FacedownMovementRotationMultiplier * Time.deltaTime;
            }

            if (yAngleDifference > MovementSettings.AngleDifferenceForFacedownAutoRotate)
            {
                var angleModifier = (yAngleDifference - MovementSettings.AngleDifferenceForFacedownAutoRotate) * 2;
                var overShoulderRotationSpeed = angleModifier * MovementSettings.FacedownOverShoulderRotationMultiplier * Time.deltaTime;
                rotationSpeed = Mathf.Max(overShoulderRotationSpeed, rotationSpeed);
            }

            return rotationSpeed;
        }
    }
}