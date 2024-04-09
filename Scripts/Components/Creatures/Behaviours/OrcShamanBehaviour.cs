using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.WizardSpells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.ScriptableObjects;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(OrcShaman))]
    public class OrcShamanBehaviour : SpellCastingEnemyBehaviour
    {
        OrcShaman orcShamanReference => creatureReference as OrcShaman;

        PhysicalWeapon rightHandWeapon;

        public override List<Weapon> useableWeapons => new List<Weapon>() 
        {
            ItemCache.GetItem(ItemCache.ItemNames.Staff) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Wand) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Shortsword) as Weapon,
            ItemCache.GetItem(ItemCache.ItemNames.Fist) as Weapon,
        };

        public override bool isAttacking => GetIsAttacking(OrcShamanAnimatorParameters.attacks);

        public override List<Role> alternativeRoles => new List<Role>()
        {
            Role.RangedSupportAttacker,
            Role.MidrangeSupport,
            Role.SupportAttacker,
        };

        public override Role preferredRole => Role.PrimaryRangedAttacker;

        public override void FinishAttack()
        {
            base.FinishAttack();

            PhysicalWeapon rightHandWeapon = orcShamanReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);
            PhysicalWeapon leftHandWeapon = orcShamanReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Left);

            if (rightHandWeapon != null)
                rightHandWeapon.SetManualStrikeType(StrikeType.NotStrike);

            if (leftHandWeapon != null)
                leftHandWeapon.SetManualStrikeType(StrikeType.NotStrike);
        }

        public override bool Attack()
        {
            rightHandWeapon = orcShamanReference.itemEquipper.GetWeaponInHand(Characters.HandSide.Right);

            if ((Time.time - timeOfLastAttack) < timeBetweenAttacks || rightHandWeapon == null)
                return false;

            if (role == Role.PrimaryRangedAttacker || role == Role.RangedSupportAttacker)
                CastSpell();

            else
                OverheadSwing();

            return true;
        }

        private void OverheadSwing()
        {
            animator.SetBool(OrcShamanAnimatorParameters.IsUsingOverheadSwing, true);
            rightHandWeapon.SetManualStrikeType(StrikeType.Imperfect);
        }

        private void CastSpell()
        {
            float action = Random.value;

            if (action < 0.25f && !enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(ShadowBind.Instance))
                TryCastSpell(ShadowBind.Instance);

            else if (action < 0.50f && !enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(CursedBlood.Instance))
                TryCastSpell(CursedBlood.Instance);

            else if (action < 0.75f && !enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(BasicHealSpell.Instance))
                CastBasicHealSpell();

            else if (!enemySpellCaster.spellCooldownTracker.IsOnCooldownBySpell(BasicStaffSpell.Instance))
                CastBasicSpell();
        }

        private void CastBasicSpell()
        {
            StartCoroutine(nameof(CastBasicSpellRoutine));
        }

        private void CastBasicHealSpell()
        {
            if (!TryCastSpell(BasicHealSpell.Instance))
                return;

            SetTarget(InjectorContainer.Injector.GetInstance<CreatureTracker>().GetLowestHealthCreatureOnTeam(team).gameObject.transform);            

            animator.SetBool(OrcShamanAnimatorParameters.IsCastingSpell, true);

            animationEventInvoker.OnAnimationEnd.AddListener(StartCast);
            animationEventInvoker.OnAnimationEnd.AddListener(FinishCast);
            animationEventInvoker.OnAnimationEnd.AddListener(ResetTarget);
        }

        public override void ResetTarget()
        {
            base.ResetTarget();

            animationEventInvoker.OnAnimationEnd.RemoveListener(ResetTarget);
        }

        IEnumerator CastBasicSpellRoutine()
        {
            //TODO: Check if there is an appropriate weapon in either hand... use that hand 
            enemySpellCaster.StartCast(HandSide.Right);

            while (!enemySpellCaster.equippedGameSpellByHandSide[HandSide.Right].correspondingSpell.isChanneledFully)
            {
                enemySpellCaster.ChannelSpell(HandSide.Right);
                yield return null;
            }

            animator.SetBool(OrcShamanAnimatorParameters.IsCastingBasicSpell, true);
            animationEventInvoker.OnAnimationEnd.AddListener(FinishCast);
        }
    }
}