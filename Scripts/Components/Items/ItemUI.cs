using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items
{
    public class ItemUI : MonoBehaviour
    {
        private const float CanPickUpDistance = 2f;
        private const float CanUseDistanceFromCameraCenter = 0.5f;

        [SerializeField] private GameObject isInRangeIndicator;
        [SerializeField] private GameObject isClosestLeftIndicator;
        [SerializeField] private GameObject isClosestRightIndicator;
        [SerializeField] private GameObject isClosestBothIndicator;
        [SerializeField] private GameObject isClosestIndicator;

        private Camera mainCamera;
        private bool registeredAsEquippable;
        private Vector3 uiPosition;
        private bool isInRange;
        private bool canGrab;
        private bool isClosestEquippableWeaponForLeftHand;
        private bool isClosestEquippableWeaponForRightHand;
        private PhysicalHandHeldItem physicalHandHeldItem;

        private bool CanAddHand => !physicalHandHeldItem.isWielded || 
            (physicalHandHeldItem.item.canWieldWithTwoHands && !physicalHandHeldItem.handSide.Equals(HandSide.Both));


        private void Awake()
        {
            mainCamera = Camera.main;

            physicalHandHeldItem = GetComponentInParent<PhysicalHandHeldItem>();
        }

        private void Update()
        {
            if (physicalHandHeldItem == null) { return; }

            EvaluateWhatToShow();
            UpdateEquippableRegistration();
            UpdateObjects();
        }

        private void OnDestroy()
        {
            if (registeredAsEquippable)
            {
                PlayerCharacterReference.Instance.playerItemEquipper.UnregisterEquippableItemInRange(physicalHandHeldItem);
                registeredAsEquippable = false;
            }
        }

        private void EvaluateWhatToShow()
        {
            isInRange = false;
            canGrab = false;

            if (!CanAddHand) { return; }

            var worldSpaceGripPosition = physicalHandHeldItem.transform.TransformPoint(physicalHandHeldItem.gripPoint);
            var distanceToCamera = Vector3.Distance(worldSpaceGripPosition, mainCamera.transform.position);

            if (distanceToCamera > CanPickUpDistance) { return; }

            isInRange = true;
            uiPosition = worldSpaceGripPosition;
            var distanceFromCenter = Vector3.Distance(worldSpaceGripPosition, mainCamera.transform.position + (mainCamera.transform.forward * distanceToCamera));
            canGrab = distanceFromCenter < CanUseDistanceFromCameraCenter;
        }

        private void UpdateEquippableRegistration()
        {
            if (canGrab)
            {
                if (!registeredAsEquippable)
                {
                    PlayerCharacterReference.Instance.playerItemEquipper.RegisterEquippableItemInRange(physicalHandHeldItem);
                    registeredAsEquippable = true;
                }
            }
            else if (registeredAsEquippable)
            {
                PlayerCharacterReference.Instance.playerItemEquipper.UnregisterEquippableItemInRange(physicalHandHeldItem);
                registeredAsEquippable = false;
            }
        }

        private void UpdateObjects()
        {
            var isClosestForAnyHand = isClosestEquippableWeaponForRightHand || isClosestEquippableWeaponForLeftHand;

            UpdateObject(isClosestLeftIndicator, active: !isClosestEquippableWeaponForRightHand && isClosestEquippableWeaponForLeftHand);
            UpdateObject(isClosestRightIndicator, active: !isClosestEquippableWeaponForLeftHand && isClosestEquippableWeaponForRightHand);
            UpdateObject(isClosestBothIndicator, active: isClosestEquippableWeaponForRightHand && isClosestEquippableWeaponForLeftHand);
            UpdateObject(isClosestIndicator, active: isClosestForAnyHand, updatePosition: true);
            UpdateObject(isInRangeIndicator, active: isInRange && !isClosestForAnyHand, updatePosition: true);
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

        public void SetClosestEquippableWeaponForHandSide(HandSide handSide, bool value)
        {
            if (handSide.Equals(HandSide.Left))
            {
                isClosestEquippableWeaponForLeftHand = value;
            }
            else
            {
                isClosestEquippableWeaponForRightHand = value;
            }
        }
    }
}