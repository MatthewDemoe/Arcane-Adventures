using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public class ItemSlot : MonoBehaviour
    {
        private const float CanUseDistance = 2f;
        private const float CanUseDistanceFromCameraCenter = 0.5f;

        [Header("Settings")]
        [SerializeField] private bool _disableWeaponPhysicsWhenInSlot = true;
        [SerializeField] private bool isRemoveOnly;
        [SerializeField] private PhysicalHandHeldItem initialItemInSlot;

        [Header("UI object references")]
        [SerializeField] private GameObject canPickUpEffect;
        [SerializeField] private GameObject isClosestLeftIndicator;
        [SerializeField] private GameObject isClosestRightIndicator;
        [SerializeField] private GameObject isClosestBothIndicator;
        [SerializeField] private GameObject isClosestIndicator;

        public bool disableWeaponPhysicsWhenInSlot => _disableWeaponPhysicsWhenInSlot;

        private Joint joint;
        private PlayerItemEquipper playerItemEquipper;
        private Camera mainCamera;

        private bool isClosestUsableItemSlotForLeftHand;
        private bool isClosestUsableItemSlotForRightHand;
        private bool canUse;
        private bool isInRange;
        private bool registeredAsUsable;
        private Vector3 uiPosition;

        public GameObject item => joint.connectedBody == null ? null : joint.connectedBody.gameObject;
        public bool isLeftInRange { get; private set; }
        public bool isRightInRange { get; private set; }
        public bool isInUse => joint.connectedBody != null;

        private void Start()
        {
            playerItemEquipper = PlayerCharacterReference.Instance.GetComponent<PlayerItemEquipper>();
            mainCamera = Camera.main;
            joint = GetComponent<Joint>();

            if (initialItemInSlot != null)
            {
                initialItemInSlot.PutInItemSlot(this);
            }
        }

        public void SetClosestUsableItemSlotForHandSide(HandSide handSide, bool value)
        {
            if (handSide.Equals(HandSide.Left))
            {
                isClosestUsableItemSlotForLeftHand = value;
            }
            else
            {
                isClosestUsableItemSlotForRightHand = value;
            }
        }

        private void Update()
        {
            if (isRemoveOnly)
            {
                return;
            }

            EvaluateWhatToShow();
            UpdateUsableRegistration();
            UpdateObjects();
        }

        private void EvaluateWhatToShow()
        {
            canUse = false;
            isInRange = false;

            if (isInUse)
            {
                return;
            }

            var distanceToCamera = Vector3.Distance(this.transform.position, mainCamera.transform.position);

            if (distanceToCamera > CanUseDistance)
            {
                return;
            }

            isInRange = true;

            var direction = (mainCamera.transform.position - this.transform.position).normalized;
            uiPosition = this.transform.position + (direction * (distanceToCamera * 0.5f));

            var distanceFromCenter = Vector3.Distance(this.transform.position, mainCamera.transform.position + (mainCamera.transform.forward * distanceToCamera));
            canUse = distanceFromCenter < CanUseDistanceFromCameraCenter;
        }

        private void UpdateUsableRegistration()
        {
            if (canUse)
            {
                if (!registeredAsUsable)
                {
                    playerItemEquipper.RegisterUsableItemSlotInRange(this);
                    registeredAsUsable = true;
                }
            }
            else if (registeredAsUsable)
            {
                playerItemEquipper.UnregisterUsableItemSlotInRange(this);
                registeredAsUsable = false;
            }
        }

        private void UpdateObjects()
        {
            UpdateObject(canPickUpEffect, active: isInRange, updatePosition: true);
            UpdateObject(isClosestLeftIndicator, active: !isClosestUsableItemSlotForRightHand && isClosestUsableItemSlotForLeftHand);
            UpdateObject(isClosestRightIndicator, active: !isClosestUsableItemSlotForLeftHand && isClosestUsableItemSlotForRightHand);
            UpdateObject(isClosestBothIndicator, active: isClosestUsableItemSlotForRightHand && isClosestUsableItemSlotForLeftHand);
            UpdateObject(isClosestIndicator, active: isClosestUsableItemSlotForRightHand || isClosestUsableItemSlotForLeftHand);
        }

        private void UpdateObject(GameObject gameObject, bool active, bool updatePosition = false)
        {
            if (active && updatePosition)
            {
                gameObject.transform.position = uiPosition;
            }

            if (gameObject.activeSelf != active)
            {
                gameObject.SetActive(active);
            }
        }
    }
}