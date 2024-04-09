using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class CheckIfEquipped : ActionNode
    {
        ItemEquipper.ItemEquipper itemEquipper;

        protected override void OnStart()
        {
            base.OnStart();

            itemEquipper = context.creatureBehaviour.GetComponent<ItemEquipper.ItemEquipper>();
        }

        protected override State OnUpdate()
        {
            if (!itemEquipper.IsAnyEquipped())
                return State.Failure;

            return State.Success;
        }
    }
}