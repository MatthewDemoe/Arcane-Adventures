using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Player.Hands;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    public class GhostHand : MonoBehaviour
    {
        private static readonly Vector3 StandardAngleAdjustment = new Vector3(90, 90, 180);
        private static readonly Vector3 UpsideDownGripAngleAdjustment = new Vector3(270, 180, 90);
        private static bool SetupComplete = false;

        [SerializeField] private Transform offsetTransform;
        private static GhostHand Left;
        private static GhostHand Right;

        private PlayerHandAnimationController playerHandAnimationController;
        private PhysicalHandHeldItem grippedItem;
        private ControllerLink controllerLink;
        private GameObject controller;

        private Quaternion AngleAdjustment => Quaternion.Euler(controllerLink.isUsingStandardGrip ? StandardAngleAdjustment : UpsideDownGripAngleAdjustment);

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        private void Update()
        {
            if (grippedItem == null)
            {
                this.transform.position = controller.transform.position;
                this.transform.rotation = controller.transform.rotation;
            }

            playerHandAnimationController.Update();
        }

        private void Initialize(HandSide handSide)
        {
            controllerLink = ControllerLink.Get(handSide);
            controller = XRReferences.GetController(handSide);

            var animator = GetComponentInChildren<Animator>();
            playerHandAnimationController = new PlayerHandAnimationController(animator, handSide);

            if (handSide.Equals(HandSide.Right))
            {
                SetOffsetTransformToRight();
            }
        }

        private void SetOffsetTransformToRight()
        {
            var offsetScale = offsetTransform.localScale;
            offsetScale.x = -offsetScale.x;
            offsetTransform.localScale = offsetScale;

            var offsetRotationInEuler = offsetTransform.localRotation.eulerAngles;
            offsetRotationInEuler.z = -offsetRotationInEuler.z;
            offsetTransform.localRotation = Quaternion.Euler(offsetRotationInEuler);

            var offsetPosition = offsetTransform.localPosition;
            offsetPosition.x = -offsetPosition.x;
            offsetTransform.localPosition = offsetPosition;
        }

        public void SetStaticGrip(float stopPoint) => playerHandAnimationController.SetStaticGrip(stopPoint);

        public void Grip(PhysicalHandHeldItem physicalHandHeldItem)
        {
            grippedItem = physicalHandHeldItem;
            this.transform.parent = grippedItem.transform;
            UpdateGrip();
            playerHandAnimationController.Grip(0/*physicalHandHeldItem.gripAnimationStopPoint*/);
        }

        public void ReleaseGrip()
        {
            grippedItem = null;
            this.transform.parent = null;
            this.transform.localPosition = Vector3.zero;
            playerHandAnimationController.ReleaseGrip();
        }

        public void UpdateGrip()
        {
            if (grippedItem == null) { return; }

            this.transform.localPosition = new Vector3(controllerLink.gripHeight, 0, 0);
            this.transform.localRotation = AngleAdjustment;
        }

        public static void Set(bool on)
        {
            if (!SetupComplete)
            {
                SetupHands();
            }

            Left.gameObject.SetActive(on);
            Right.gameObject.SetActive(on);
        }

        private static void SetupHands()
        {
            var ghostHandPrefab = Prefabs.Load(Prefabs.PrefabNames.GhostHand);

            Left = Instantiate(ghostHandPrefab).GetComponent<GhostHand>();
            Left.Initialize(HandSide.Left);

            Right = Instantiate(ghostHandPrefab).GetComponent<GhostHand>();
            Right.Initialize(HandSide.Right);

            SetupComplete = true;
        }
    }
}