using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.UnityWrappers;
using com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper
{
    public class PlayerItemEquipper : ItemEquipper, IInjectable
    {
        private const string Fist = nameof(Fist);
        public const float PinpointGripMaxDistance = 0.4f;

        private readonly Dictionary<HandSide, GameObject> FistByHandSide = new Dictionary<HandSide, GameObject>
        {
            { HandSide.Left, null },
            { HandSide.Right, null }
        };

        private readonly Dictionary<HandSide, SetPhysicsIgnoreCommand> FistAndPreviousWeaponSetPhysicsIgnoreCommandByHandSide = new Dictionary<HandSide, SetPhysicsIgnoreCommand>
        {
            { HandSide.Left, new SetPhysicsIgnoreCommand() },
            { HandSide.Right, new SetPhysicsIgnoreCommand() }
        };

        private readonly Dictionary<HandSide, PhysicalHandHeldItem> closestEquippableItemByHandSide = new Dictionary<HandSide, PhysicalHandHeldItem>
        {
            { HandSide.Left, null },
            { HandSide.Right, null }
        };

        private readonly Dictionary<HandSide, ItemSlot> closestUsableItemSlotByHandSide = new Dictionary<HandSide, ItemSlot>
        {
            { HandSide.Left, null },
            { HandSide.Right, null }
        };

        private readonly List<ItemSlot> usableItemSlotsInRange = new List<ItemSlot>();
        private readonly List<PhysicalHandHeldItem> equippableItemsInRange = new List<PhysicalHandHeldItem>();

        private Animator animator;

        public bool IsFistActive(HandSide handSide) => !(FistByHandSide[handSide] is null) && FistByHandSide[handSide].activeSelf;
        public override bool IsEquipped(HandSide handSide) => base.IsEquipped(handSide) && !IsFistActive(handSide);

        protected override void Awake()
        {
            base.Awake();
            
            InjectorContainer.Injector.Bind(this);
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            
            if (!PlayerCharacterReference.Instance.isGhost)
            {
                if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
                {
                    InstantiateFist(HandSide.Left);
                    InstantiateFist(HandSide.Right);
                }
                else StartCoroutine(EquipFistsRoutine());
            }
        }

        protected void Update()
        {
            if (equippableItemsInRange.Any())
            {
                foreach (var handSide in HandSides)
                {
                    if (IsEquipped(handSide))
                    {
                        if (closestEquippableItemByHandSide[handSide] != null)
                        {
                            closestEquippableItemByHandSide[handSide].itemUI.SetClosestEquippableWeaponForHandSide(handSide, false);
                            closestEquippableItemByHandSide[handSide] = null;
                        }

                        continue;
                    }

                    var closestEquippableItem = equippableItemsInRange
                        .OrderBy(item => GetDistanceFromCameraCenter(item.transform.TransformPoint(item.gripPoint)))
                        .First();

                    var lastClosestEquippableItem = closestEquippableItemByHandSide[handSide];

                    if (closestEquippableItem != lastClosestEquippableItem)
                    {
                        closestEquippableItemByHandSide[handSide] = closestEquippableItem;
                        Controllers.SendHapticImpulse(handSide, 0.25f, 0.1f);
                        lastClosestEquippableItem?.itemUI.SetClosestEquippableWeaponForHandSide(handSide, false);
                        closestEquippableItem.itemUI.SetClosestEquippableWeaponForHandSide(handSide, true);
                    }
                }
            }

            if (usableItemSlotsInRange.Any())
            {
                foreach (var handSide in HandSides)
                {
                    if (itemInHandByHandSide[handSide] == null)
                    {
                        if (closestUsableItemSlotByHandSide[handSide] != null)
                        {
                            closestUsableItemSlotByHandSide[handSide].SetClosestUsableItemSlotForHandSide(handSide, false);
                            closestUsableItemSlotByHandSide[handSide] = null;
                        }

                        continue;
                    }

                    var closestUsableItemSlot = usableItemSlotsInRange
                        .OrderBy(itemSlot => GetDistanceFromCameraCenter(itemSlot.transform.position))
                        .First();

                    var lastClosestUsableItemSlot = closestUsableItemSlotByHandSide[handSide];

                    if (closestUsableItemSlot != lastClosestUsableItemSlot)
                    {
                        closestUsableItemSlotByHandSide[handSide] = closestUsableItemSlot;
                        Controllers.SendHapticImpulse(handSide, 0.25f, 0.1f);
                        lastClosestUsableItemSlot?.SetClosestUsableItemSlotForHandSide(handSide, false);
                        closestUsableItemSlot.SetClosestUsableItemSlotForHandSide(handSide, true);
                    }
                }
            }
        }

        private float GetDistanceFromCameraCenter(Vector3 position)
        {
            var distanceToCamera = Vector3.Distance(position, Camera.main.transform.position);
            return Vector3.Distance(position, Camera.main.transform.position + (Camera.main.transform.forward * distanceToCamera));
        }

        public void RegisterUsableItemSlotInRange(ItemSlot itemSlot)
        {
            usableItemSlotsInRange.Add(itemSlot);
        }

        public void UnregisterUsableItemSlotInRange(ItemSlot itemSlot)
        {
            if (usableItemSlotsInRange.Contains(itemSlot))
            {
                usableItemSlotsInRange.Remove(itemSlot);
            }

            foreach (var handSide in HandSides.Where(handSide => closestUsableItemSlotByHandSide[handSide] == itemSlot))
            {
                closestUsableItemSlotByHandSide[handSide].SetClosestUsableItemSlotForHandSide(handSide, false);
                closestUsableItemSlotByHandSide[handSide] = null;
            }
        }

        public void RegisterEquippableItemInRange(PhysicalHandHeldItem physicalHandHeldItem)
        {
            equippableItemsInRange.Add(physicalHandHeldItem);
        }

        public void UnregisterEquippableItemInRange(PhysicalHandHeldItem physicalHandHeldItem)
        {
            if (equippableItemsInRange.Contains(physicalHandHeldItem))
            {
                equippableItemsInRange.Remove(physicalHandHeldItem);
            }

            foreach (var handSide in HandSides.Where(handSide => closestEquippableItemByHandSide[handSide] == physicalHandHeldItem))
            {
                closestEquippableItemByHandSide[handSide].itemUI.SetClosestEquippableWeaponForHandSide(handSide, false);
                closestEquippableItemByHandSide[handSide] = null;
            }
        }

        public bool TryEquipDetectedItem(HandSide handSide)
        {
            if (!equippableItemsInRange.Any())
            {
                return false;
            }

            var item = closestEquippableItemByHandSide[handSide];
            
            return EquipItem(handSide, item);
        }
        
        public override bool EquipItem(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            if (!CanEquip())
                return false;

            if (IsFistActive(handSide))
                DropItem(handSide);

            base.EquipItem(handSide, physicalHandHeldItem);

            if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
                Grip(handSide, physicalHandHeldItem);
            else
                PlayerCharacterReference.Instance.playerAvatar.Grip(handSide, physicalHandHeldItem);            
            
            var playerCharacter = PlayerCharacterReference.Instance.creature as PlayerCharacter;

            if(handSide == HandSide.Left)
                playerCharacter.leftHandItem = physicalHandHeldItem.item;
            else
                playerCharacter.rightHandItem = physicalHandHeldItem.item;

            var isSecondHandInTwoHandedWielding = itemInHandByHandSide[HandSide.Right] != null && itemInHandByHandSide[HandSide.Right].Equals(itemInHandByHandSide[HandSide.Left]);
            var controllerLink = ControllerLink.Get(handSide);
            controllerLink.SetItem(physicalHandHeldItem, isSecondHandInTwoHandedWielding);
            var gripPoint = GetGripPoint(handSide, physicalHandHeldItem);
            controllerLink.SetGripPoint(gripPoint);

            return true;
        }

        private const float GripAnimationRadiusMultiplier = 15;
 
        private float GetGripPoint(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            if (physicalHandHeldItem.gripRange == 0)
                return 0;

            var worldSpaceGripPosition = physicalHandHeldItem.transform.TransformPoint(physicalHandHeldItem.gripPoint);
            var endOfGrip = physicalHandHeldItem.transform.right * (physicalHandHeldItem.gripRange * 0.5f);
            var segmentStart = worldSpaceGripPosition + endOfGrip;
            var segmentEnd = worldSpaceGripPosition - endOfGrip;
            var point = ControllerLink.Get(handSide).transform.position;
            var closestPointOnGripRange = point.GetClosestPointOnLine(segmentStart, segmentEnd);
            var gripPoint = Vector3.Distance(point, closestPointOnGripRange) > PinpointGripMaxDistance ?
                0 : 
                physicalHandHeldItem.transform.InverseTransformPoint(closestPointOnGripRange).x;

            return gripPoint;
        }

        public override GameObject GetTarget(HandSide handSide) => XRReferences.GetController(handSide);

        protected override void DropItem(HandSide handSide)
        {
            var controllerLink = ControllerLink.Get(handSide);
            var physicalHandHeldItem = controllerLink.connectedItem;
            controllerLink.DisconnectPhysicalHandHeldItem();

            if (!IsFistActive(handSide) && closestUsableItemSlotByHandSide[handSide] != null && !physicalHandHeldItem.handSide.Equals(HandSide.Both))
            {
                physicalHandHeldItem.PutInItemSlot(closestUsableItemSlotByHandSide[handSide]);
            }

            physicalHandHeldItem.RemoveWieldingHand(handSide);

            if (IsFistActive(handSide))
            {
                FistByHandSide[handSide].SetActive(false);
            }
            else
            {
                if (PlayerCharacterReference.Instance.isUsingPuppetMaster)
                    ReleaseGrip(handSide);
                else
                    PlayerCharacterReference.Instance.playerAvatar.ReleaseGrip(handSide);
                
                EquipFist(handSide, physicalHandHeldItem.colliders, controllerLink);
            }
        }

        private void Grip(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            var stateId = Animator.StringToHash(physicalHandHeldItem.item.name);
            var layer = handSide.Equals(HandSide.Left) ? 2 : 1;
            
            if (animator.HasState(layer, stateId))
            {
                animator.Play(stateId, layer, 1);
            }
            else
            {
                var normalizedTime = 1 - physicalHandHeldItem.gripRadius * GripAnimationRadiusMultiplier;
                animator.Play( PlayerCharacterAnimatorParameters.Grip, layer, normalizedTime);
            }
        }

        private void ReleaseGrip(HandSide handSide)
        {
            var layer = handSide.Equals(HandSide.Left) ? 2 : 1;
            animator.Play( PlayerCharacterAnimatorParameters.Grip, layer, 0);
        }

        private void EquipFist(HandSide handSide, Collider[] previousItemColliders, ControllerLink controllerLink)
        {
            var fist = FistByHandSide[handSide].GetComponent<PhysicalWeapon>();

            FistAndPreviousWeaponSetPhysicsIgnoreCommandByHandSide[handSide].Set(fist.colliders, previousItemColliders, ignore: true);
            FistAndPreviousWeaponSetPhysicsIgnoreCommandByHandSide[handSide].TryExecute();
            FistAndPreviousWeaponSetPhysicsIgnoreCommandByHandSide[handSide].Reuse(invert: true, delay: 0.5f);
            FistAndPreviousWeaponSetPhysicsIgnoreCommandByHandSide[handSide].TryExecute();

            FistByHandSide[handSide].SetActive(true);
            itemInHandByHandSide[handSide] = FistByHandSide[handSide];
            var target = GetTarget(handSide);
            var joint = GetEquipmentJoint(handSide);
            fist.AddWieldingHand(wielder, target, joint, handSide);
            controllerLink.SetItem(fist);
        }

        protected override ConfigurableJoint GetEquipmentJoint(HandSide handSide) => ControllerLink.Get(handSide).gameObject.GetComponent<ConfigurableJoint>();

        public void InstantiateFist(HandSide handSide)
        {
            InstantiateEquippedItem(ItemCache.GetItem(Fist), handSide);

            FistByHandSide[handSide] = itemInHandByHandSide[handSide];
            FistByHandSide[handSide].transform.parent = transform;
        }

        private IEnumerator EquipFistsRoutine()
        {
            yield return new WaitUntil(() => PlayerCharacterReference.Instance.playerAvatar != null && PlayerCharacterReference.Instance.playerAvatar.isAvatarReady);

            InstantiateFist(HandSide.Left);
            InstantiateFist(HandSide.Right);
        }
    }
}