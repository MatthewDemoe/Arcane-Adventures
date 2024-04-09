using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class SwordTestNPCItemEquipper : ItemEquipper.ItemEquipper
    {
        [SerializeField] private ConfigurableJoint rightJoint;
        [SerializeField] private ConfigurableJoint leftJoint;
        [SerializeField] private GameObject rightTarget;
        [SerializeField] private GameObject leftTarget;

        private PhysicalHandHeldItem defaultItem;
        
        private void Start()
        {
            rightJoint.anchor = rightTarget.transform.localPosition;
            var shortsword = ItemCache.GetItem(ItemCache.ItemNames.Shortsword);
            InstantiateEquippedItem(shortsword, HandSide.Right);
            defaultItem = itemInHandByHandSide[HandSide.Right].GetComponent<PhysicalHandHeldItem>();
        }

        public override GameObject GetTarget(HandSide handSide)
            => handSide.Equals(HandSide.Right) ? rightTarget : leftTarget;

        protected override void DropItem(HandSide handSide)
        {
            var joint = GetEquipmentJoint(handSide);
            joint.connectedBody = null;
            joint.xMotion = joint.yMotion = joint.zMotion = ConfigurableJointMotion.Free;
            joint.xDrive = joint.yDrive = joint.zDrive = joint.slerpDrive = PhysicalHandHeldItem.PowerlessJointDrive;
            defaultItem.RemoveWieldingHand(handSide);
        }

        protected override ConfigurableJoint GetEquipmentJoint(HandSide handSide)
            => handSide.Equals(HandSide.Right) ? rightJoint : leftJoint;
    }
}