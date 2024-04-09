using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(PlayerCharacterReference))]
    public class PlayerCharacterBehaviour : CreatureBehaviour
    {
        public override bool isAttacking => false;
        public override Team team => Team.Ally;

        public override List<Role> alternativeRoles => new List<Role>()
        {
            Role.SupportAttacker,
        };

        public override Role preferredRole => Role.PrimaryAttacker;

        protected override void Initialize(CreatureReference creatureReference)
        {
            base.Initialize(creatureReference);
        }

        protected override void Update()
        {
            if (!creatureReference.creature.statusConditionTracker.HasStatusCondition(AllStatusConditions.StatusConditionName.Maddened))
                return;

            base.Update();
        }

        public override bool Attack()
        {
            if (animator == null)
                animator = PlayerCharacterReference.Instance.avatarAnimator;

            if ((Random.value > 0.7f))
            {
                animator.SetBool(CharacterAnimatorParameters.IsUsingComboAttack, true);
            }
            else
            {
                animator.SetFloat(CharacterAnimatorParameters.SwingHeight, Random.value);
                animator.SetBool(CharacterAnimatorParameters.IsUsingSingleAttack, true);
            }

            PhysicalWeapon rightHandWeapon = creatureReference.GetComponent<PlayerItemEquipper>().GetItemInHand(HandSide.Right).GetComponent<PhysicalWeapon>();

            if (rightHandWeapon != null)
            {
                rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);
            }

            return true;
        }

        public void SetBehaviourTreeActive(bool isActive)
        {
            navMeshAgent.enabled = isActive;
            behaviourTreeRunner.enabled = isActive;
        }
    }
}