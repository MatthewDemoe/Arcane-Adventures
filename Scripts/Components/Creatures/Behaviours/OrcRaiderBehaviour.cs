using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using UnityEngine;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(OrcRaider))]
    public class OrcRaiderBehaviour : EnemyBehaviour
    {
        OrcRaider orcRaiderReference => creatureReference as OrcRaider;

        public override bool isAttacking => GetIsAttacking(OrcRaiderAnimatorParameters.attacks);
        protected override bool shouldUpdateMovementParameters => false;

        public override List<Weapon> useableWeapons => new List<Weapon>()
        {
            ItemCache.GetItem(ItemCache.ItemNames.Shortsword) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Dagger) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Fist) as Weapon,
        };

        public override List<Role> alternativeRoles => new List<Role>()
        {
            Role.SupportAttacker,
        };

        public override Role preferredRole => Role.PrimaryAttacker;

        public override void FinishAttack()
        {
            base.FinishAttack();

            PhysicalWeapon rightHandWeapon = orcRaiderReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);
            PhysicalWeapon leftHandWeapon = orcRaiderReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Left);

            if(rightHandWeapon != null)
                rightHandWeapon.SetManualStrikeType(StrikeType.NotStrike);

            if(leftHandWeapon != null)
                leftHandWeapon.SetManualStrikeType(StrikeType.NotStrike);
        }

        public override bool Attack()
        {
            PhysicalWeapon rightHandWeapon = orcRaiderReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);
            PhysicalWeapon leftHandWeapon = orcRaiderReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Left);

            if ((Time.time - timeOfLastAttack) < timeBetweenAttacks || rightHandWeapon == null)
                return false;

            //TODO: Enable when the Orc Raider can equip a left hand weapon
            //if ((Random.value > 0.5f))
            //{
            //    animator.SetBool(OrcRaiderAnimatorParameters.IsUsingComboAttack, true);
            //}

            else
            {
                animator.SetBool(OrcRaiderAnimatorParameters.IsUsingSingleAttack, true);
            }

            rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);       

            return true;
        }
    }
}