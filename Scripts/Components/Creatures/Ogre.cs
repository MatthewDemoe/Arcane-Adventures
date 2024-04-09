using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class Ogre : HumanoidMonster
    {
        protected override System.Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.Ogre); } }

        public override bool isAttacking => animator.GetBool(OrcRaiderAnimatorParameters.IsUsingSingleAttack) || animator.GetBool(OrcRaiderAnimatorParameters.IsUsingComboAttack);

        public override void ProcessDamage()
        {
            base.ProcessDamage();
        }
    }
}