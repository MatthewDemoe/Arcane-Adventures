using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.UnityWrappers;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper
{
    public abstract class ItemEquipper : MonoBehaviour
    {
        public UnityEvent<GameObject> OnEquipItem = new UnityEvent<GameObject>();
        public UnityEvent<GameObject> OnUnequipItem = new UnityEvent<GameObject>();

        private const float UnequipPhysicsIgnoreRevokeDelay = 1;

        protected static readonly HandSide[] HandSides = new HandSide[] { HandSide.Left, HandSide.Right };

        protected CreatureReference wielder;
        private SetPhysicsIgnoreCommand setPhysicsIgnoreCommand;

        protected readonly Dictionary<HandSide, GameObject> itemInHandByHandSide = HandSides.ToDictionary(handSide => handSide, handSide => (GameObject) null);

        public virtual bool IsEquipped(HandSide handSide) => itemInHandByHandSide[handSide] != null;
        public virtual bool IsAnyEquipped() => itemInHandByHandSide[HandSide.Left] != null || itemInHandByHandSide[HandSide.Right] != null;

        public GameObject GetItemInHand(HandSide handSide) => itemInHandByHandSide[handSide];
        public PhysicalWeapon GetWeaponInHand(HandSide handSide) => itemInHandByHandSide[handSide] == null ? null : itemInHandByHandSide[handSide].GetComponent<PhysicalWeapon>();

        protected virtual void Awake()
        {
            wielder = GetComponent<CreatureReference>();
            setPhysicsIgnoreCommand = new SetPhysicsIgnoreCommand();
        }

        protected virtual void FixedUpdate()
        {
            if (setPhysicsIgnoreCommand.isSet)
            {
                setPhysicsIgnoreCommand.TryExecute(Time.deltaTime);
            }
        }

        public void InstantiateEquippedItem(Item item, HandSide handSide)
        {
            if (IsEquipped(handSide))
            {
                UnequipItem(handSide);
            }

            var target = GetTarget(handSide);
            var prefab = ItemCache.GetPrefab(item.name);
            var instance = Instantiate(prefab, target.transform.position, target.transform.rotation);
            var physicalWeapon = instance.GetComponent<PhysicalWeapon>();

            EquipItem(handSide, physicalWeapon);
        }

        public void UnequipItem(HandSide handSide)
        {
            if (!IsEquipped(handSide))
            {
                return;
            }

            var itemInHand = itemInHandByHandSide[handSide];
            var isWieldingWithTwoHands = itemInHandByHandSide[GetOppositeHandSide(handSide)] == itemInHand;
            itemInHandByHandSide[handSide] = null;

            if (!isWieldingWithTwoHands)
            {
                SetIgnoreCollisionForItem(itemInHand, ignore: false, delay: UnequipPhysicsIgnoreRevokeDelay);
            }

            itemInHand.GetComponent<PhysicalHandHeldItem>().OnUnequipped.Invoke();
            OnUnequipItem.Invoke(itemInHand);
            DropItem(handSide);
        }

        private HandSide GetOppositeHandSide(HandSide handSide)
        {
            switch (handSide)
            {
                case HandSide.Left: return HandSide.Right;
                case HandSide.Right: return HandSide.Left;
                default: throw new System.Exception("Cannot get opposite handside of non-singular abstract handside");
            }
        }

        public bool CanEquip() => !(setPhysicsIgnoreCommand.isSet || wielder.HasStatusCondition(AllStatusConditions.StatusConditionName.Disarmed));

        public virtual bool EquipItem(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            if (!CanEquip())
            {
                return false;
            }

            SetIgnoreCollisionForItem(physicalHandHeldItem.gameObject, ignore: true);
            setPhysicsIgnoreCommand.TryExecute();
            itemInHandByHandSide[handSide] = physicalHandHeldItem.gameObject;
            var target = GetTarget(handSide);
            var joint = GetEquipmentJoint(handSide);
            physicalHandHeldItem.AddWieldingHand(wielder, target, joint, handSide);

            OnEquipItem.Invoke(physicalHandHeldItem.gameObject);
            physicalHandHeldItem.OnEquipped.Invoke();
            return true;
        }

        private static readonly List<int> CreatureColliderLayersToIgnore = new List<int>()
        {
            (int)Layers.Creature,
            (int)Layers.CreatureBase,
            (int)Layers.CreatureRagdoll,
            (int)Layers.CreatureControl
        };

        private void SetIgnoreCollisionForItem(GameObject item, bool ignore, float delay = 0)
        {
            var weaponColliders = item.GetComponentsInChildren<Collider>()
                .Where(collider => collider.gameObject.layer == (int)Layers.Weapon)
                .ToArray();
            var creatureColliders = this.gameObject.transform.parent.GetComponentsInChildren<Collider>()
                .Where(collider => CreatureColliderLayersToIgnore.Contains(collider.gameObject.layer))
                .ToArray();

            setPhysicsIgnoreCommand.Set(weaponColliders, creatureColliders, ignore, delay);
        }

        public abstract GameObject GetTarget(HandSide handSide);
        protected abstract void DropItem(HandSide handSide);
        protected abstract ConfigurableJoint GetEquipmentJoint(HandSide handSide);
    }
}