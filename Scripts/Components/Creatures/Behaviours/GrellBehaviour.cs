using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using UnityEngine;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(Grell))]
    public class GrellBehaviour : EnemyBehaviour
    {
        Grell grellReference => creatureReference as Grell;
        public override List<Weapon> useableWeapons => new List<Weapon>() 
        {
            ItemCache.GetItem(ItemCache.ItemNames.Shortsword) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Dagger) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Fist) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Spear) as Weapon,
        };

        public override bool isAttacking => GetIsAttacking(GrellAnimatorParameters.attacks);

        public override List<Role> alternativeRoles => new List<Role>() 
        {
            Role.SupportAttacker,
        };
        public override Role preferredRole => Role.PrimaryAttacker;


        public override void FinishAttack()
        {
            base.FinishAttack();

            PhysicalWeapon rightHandWeapon = grellReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);

            if (rightHandWeapon == null)
                return;

            rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);
        }

        public override bool Attack()
        {
            PhysicalWeapon rightHandWeapon = grellReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);

            if ((Time.time - timeOfLastAttack) < timeBetweenAttacks || rightHandWeapon == null)
                return false;

            if (role == Role.PrimaryAttacker)
                animator.SetBool(GetPrimaryAttackAnimatorString(Random.value), true);

            else
                animator.SetBool(GetSupportAttackAnimatorString(Random.value), true);

            rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);

            return true;
        }

        private string GetPrimaryAttackAnimatorString(float input)
        {
            if (input < 0.33f)
                return GrellAnimatorParameters.IsUsingJab;

            if (input < 0.66f)
                return GrellAnimatorParameters.IsUsingComboAttack;

            return GrellAnimatorParameters.IsUsingOverheadSwing;
        }

        private string GetSupportAttackAnimatorString(float input)
        {
            if (input < 0.5f)
                return GrellAnimatorParameters.IsUsingJab;

            return GrellAnimatorParameters.IsUsingOverheadSwing;
        }
    }
}