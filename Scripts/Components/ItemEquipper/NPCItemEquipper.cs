using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using System.Linq;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper
{
    public class NPCItemEquipper : ItemEquipper
    {
        private NPCControllerLink rightControllerLink;
        private NPCControllerLink leftControllerLink;

        public override GameObject GetTarget(HandSide handSide) => GetNPCControllerLink(handSide).gameObject;
        protected override ConfigurableJoint GetEquipmentJoint(HandSide handSide) => GetNPCControllerLink(handSide).GetComponent<ConfigurableJoint>();

        protected override void Awake()
        {
            base.Awake();
            var npcControllerLinks = GetComponentsInChildren<NPCControllerLink>();
            rightControllerLink = npcControllerLinks.SingleOrDefault(controllerLink => controllerLink.handSide.Equals(HandSide.Right));
            leftControllerLink = npcControllerLinks.SingleOrDefault(controllerLink => controllerLink.handSide.Equals(HandSide.Left));

        }

        GameSystem.Creatures.Monsters.HumanoidMonster humanoidMonster => GetComponent<CreatureReference>().creature as GameSystem.Creatures.Monsters.HumanoidMonster;

        protected override void DropItem(HandSide handSide)
        {
            var controllerLink = GetNPCControllerLink(handSide);
            var item = controllerLink.connectedItem;
            controllerLink.Disconnect();
            item.RemoveWieldingHand(handSide);
            AssignItem(handSide, null);
        }

        public override bool EquipItem(HandSide handSide, PhysicalHandHeldItem physicalHandHeldItem)
        {
            if (!base.EquipItem(handSide, physicalHandHeldItem))
                return false;

            var controllerLink = GetNPCControllerLink(handSide);
            controllerLink.SetItem(physicalHandHeldItem);
            controllerLink.SetGripPoint(0);
            AssignItem(handSide, physicalHandHeldItem.item);

            return true;

        }

        private void AssignItem(HandSide handSide, HandHeldItem item)
        {
            switch (handSide)
            {
                case HandSide.Left: humanoidMonster.leftHandItem = item; break;
                case HandSide.Right: humanoidMonster.rightHandItem = item; break;
                default: throw new System.Exception("Invalid HandSide");
            }
        }

        public NPCControllerLink GetNPCControllerLink(HandSide handSide) =>
            handSide switch
            {
                HandSide.Right => rightControllerLink,
                HandSide.Left => leftControllerLink,
                _ => null
            };
    }
}