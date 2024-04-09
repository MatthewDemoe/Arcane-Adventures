using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class Grell : HumanoidMonster
    {
        public override bool isAttacking => animator.GetBool(GrellAnimatorParameters.IsUsingJab) 
            || animator.GetBool(GrellAnimatorParameters.IsUsingOverheadSwing) 
            || animator.GetBool(GrellAnimatorParameters.IsUsingComboAttack);

        protected override System.Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.Grell); } }
    }
}