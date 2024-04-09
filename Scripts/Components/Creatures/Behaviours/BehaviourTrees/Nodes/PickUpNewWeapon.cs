using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class PickUpNewWeapon : ActionNode
    {
        float MaxDistance => context.creatureBehaviour.closeRangeDistance;
        float MinDistance => context.creatureBehaviour.maximumCloseRangeDistance;

        ItemEquipper.ItemEquipper itemEquipper;
        PhysicalWeapon closestWeapon;

        protected override void OnStart()
        {
            base.OnStart();

            if (context.agent.enabled == false)
                context.agent.enabled = true;

            context.agent.stoppingDistance = context.creatureBehaviour.closeRangeDistance;

            itemEquipper = context.creatureBehaviour.GetComponent<ItemEquipper.ItemEquipper>();

            closestWeapon = InjectorContainer.Injector.GetInstance<PhysicalWeaponTracker>().GetClosestWeapon(context.creatureBehaviour.transform.position, context.creatureBehaviour.useableWeapons);
        }

        protected override State OnUpdate()
        {
            if (itemEquipper.IsAnyEquipped() || closestWeapon is null)
                return State.Success;

            if (!context.creatureReference.creature.isMovementEnabled)
                return State.Failure;

            UpdateTargetPosition();

            if (blackboard.distanceFromTarget < MaxDistance)
            {
                context.creatureBehaviour.PickUpItem(closestWeapon);
                return State.Success;
            }

            if (context.agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathInvalid)
                return State.Failure;            

            return State.Running;
        }

        private void UpdateTargetPosition()
        {
            if (context.creatureBehaviour.target == null)
                return;

            blackboard.distanceFromTarget = Vector3.Distance(closestWeapon.transform.position, context.agent.transform.position);

            blackboard.fromTargetToAgent = context.agent.transform.position - closestWeapon.transform.position;
            blackboard.fromTargetToAgent.y = 0.0f;

            Vector3 targetToSelf = context.transform.position - closestWeapon.transform.position;

            if (targetToSelf.magnitude > MaxDistance)
                targetToSelf = -targetToSelf;

            blackboard.moveToPosition = context.transform.position + (targetToSelf.normalized * context.agent.stoppingDistance * 1.5f);
            context.agent.destination = blackboard.moveToPosition;
        }
    }
}