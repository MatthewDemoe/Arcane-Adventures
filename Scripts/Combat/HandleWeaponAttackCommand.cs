using com.AlteredRealityLabs.ArcaneAdventures.Components;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using Injection;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Combat
{
    public class HandleWeaponAttackCommand
    {
        private GameObject combatUIDamageNumberPrefab;

        private static Dictionary<DamageType, float> AdditionalForceMultiplierByDamageType = new Dictionary<DamageType, float>
        {
            { DamageType.Magical, 1f },
            { DamageType.Bludgeoning, 1f },
            { DamageType.Slashing, 0.5f },
            { DamageType.Piercing, 0 }
        };

        private static Dictionary<StrikeType, float> AdditionalForceMultiplierByStrikeType = new Dictionary<StrikeType, float>
        {
            { StrikeType.Perfect, 1f },
            { StrikeType.Imperfect, 0.66f },
            { StrikeType.Incomplete, 0.33f }
        };

        private PhysicalWeapon physicalWeapon;
        private WeaponSurface weaponSurface;
        private AttackableSurface attackableSurface;

        [Inject] protected ICombatSystem combatSystem;

        public HandleWeaponAttackCommand(PhysicalWeapon physicalWeapon, WeaponSurface weaponSurface, AttackableSurface attackableSurface)
        {
            this.physicalWeapon = physicalWeapon;
            this.weaponSurface = weaponSurface;
            this.attackableSurface = attackableSurface;

            InjectorContainer.Injector.Inject(this);
        }

        public void Execute()
        {
            if (!IsValidAttack)
            {
                if (IsPlayerInvolved)
                {
                    CombatHaptics.SendHapticImpulse(StrikeType.NotStrike, physicalWeapon.handSide);
                }

                return;
            }

            combatUIDamageNumberPrefab = Prefabs.UI.Load(Prefabs.UI.combatUIDamageNumber);//TODO: Cache.
            var strikeType = physicalWeapon.strikeType;

            if (attackableSurface.creatureReference is PlayerCharacterReference)
            {
                //TODO: All of this needs to be reworked with more proper feedback and adhere to the actual combat system.
                CombatHaptics.SendHapticImpulse(StrikeType.Perfect, HandSide.Left);
                CombatHaptics.SendHapticImpulse(StrikeType.Perfect, HandSide.Right);
                strikeType = StrikeType.Incomplete;
            }

            var wieldingCreature = physicalWeapon.wielder == null ? null : physicalWeapon.wielder.creature;
            var weaponHit = new WeaponHit(wieldingCreature, attackableSurface.creatureReference.creature, strikeType, weaponSurface);

            if(wieldingCreature != null)
                wieldingCreature.OnWeaponHitBeforeReport.Invoke(weaponHit.target);

            combatSystem.ReportHit(weaponHit);
            attackableSurface.OnSurfaceAttacked.Invoke();

            if (attackableSurface.creatureReference is Monster)
            {
                TakeMonsterSpecificActions(attackableSurface.creatureReference as Monster, weaponHit);
            }

            physicalWeapon.OnAttack.Invoke(attackableSurface.creatureReference, strikeType);
            physicalWeapon.ReportHit();
            RegisterSecondaryStrikeGuid();
        }

        private bool IsPlayerInvolved => physicalWeapon.wielder is PlayerCharacterReference || 
            attackableSurface.creatureReference is PlayerCharacterReference;

        private bool IsValidAttack => !physicalWeapon.strikeType.Equals(StrikeType.NotStrike) &&
            !attackableSurface.creatureReference.creature.isDead &&
            attackableSurface.attackableSurfaceCoordinator.RegisterStrikeGuid(physicalWeapon.currentStrikeGuid.Value);

        private Rigidbody GetRigidbody()
        {
            //TODO: Questionable code required because the current monsters are implemented in different ways.
            if (attackableSurface.creatureReference is PossessedDummyReference)
            {
                return attackableSurface.GetComponent<Rigidbody>();
            }
            else if (attackableSurface.creatureReference is OrcRaider)
            {
                return attackableSurface.GetComponentInParent<Monster>().GetComponent<Rigidbody>();
            }

            return null;
        }

        private void TakeMonsterSpecificActions(Monster monster, WeaponHit weaponHit)
        {
            DisplayDamageNumber(weaponHit.healthChange.Value, monster.overHeadAnchor.transform.position, weaponHit.strikeType);

            if (physicalWeapon.isWieldedByPlayer)
            {
                AddAdditionalForce(physicalWeapon, physicalWeapon.strikeType, weaponSurface.damageType);
            }
        }

        private void AddAdditionalForce(PhysicalWeapon physicalWeapon, StrikeType strikeType, DamageType damageType)
        {
            var rigidbody = GetRigidbody();
            var isWieldingWithTwoHands = ControllerLink.Get(physicalWeapon.handSide).isWieldingWithTwoHands;
            var handStrengthPoints = PlayerCharacterReference.Instance.creature.stats.subtotalStrength * (isWieldingWithTwoHands ? 2 : 1);
            var weaponWeightCategoryPoints = physicalWeapon.weapon.weightCategory * 6;
            var damageTypeMultiplier = AdditionalForceMultiplierByDamageType[damageType];
            var strikeMultiplier = AdditionalForceMultiplierByStrikeType[strikeType];
            var additionalForceScore = (handStrengthPoints + weaponWeightCategoryPoints) * damageTypeMultiplier * strikeMultiplier;
            var additionalForceMultiplier = additionalForceScore * CombatSettings.Strikes.KnockbackForceMultiplier * physicalWeapon.knockbackMultiplier;
            var direction = physicalWeapon.strikeDirection;
            var force = direction * additionalForceMultiplier;

            if (rigidbody is Rigidbody)
            {
                rigidbody.AddForce(force);
            }
        }

        private void RegisterSecondaryStrikeGuid()
        {
            var secondaryStrikeGuid = physicalWeapon.GetSecondaryStrikeGuid();

            if (secondaryStrikeGuid != null)
            {
                attackableSurface.attackableSurfaceCoordinator.RegisterStrikeGuid(secondaryStrikeGuid.Value);
            }
        }

        private void DisplayDamageNumber(int damage, Vector3 position, StrikeType strikeType)
        {
            var instance = Object.Instantiate(combatUIDamageNumberPrefab, position, Camera.main.transform.rotation);
            var canvas = instance.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;
            canvas.worldCamera = Camera.main;
            var canvasRectTransform = canvas.transform as RectTransform;
            var sizeDelta = canvasRectTransform.sizeDelta;
            sizeDelta.x = 35;
            sizeDelta.y = 35;
            canvasRectTransform.sizeDelta = sizeDelta;
            canvas.GetComponent<LookAt>().target = Camera.main.gameObject;
            instance.GetComponent<DamageNumberFontMaterialSetter>().UpdateMaterial(strikeType);
            var textMeshProUGui = instance.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGui.text = $"{damage} ({attackableSurface.surfaceName})";
        }
    }
}