using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using com.AlteredRealityLabs.ArcaneAdventures.SystemExtensions;
using UnityEngine;
using System.Linq;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class PhysicalSpellKnockBack : MonoBehaviour, ISpellReferencer
    {
        private PhysicalSpell _physicalSpell;
        public PhysicalSpell physicalSpell
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;

                _knockBackDistance = _physicalSpell.correspondingSpell.knockbackDistance;

                SpellArrowTargeter spellArrowTargeter = GetComponentInParent<SpellArrowTargeter>();

                if (spellArrowTargeter is null)
                    return;

                spellArrowTargeter.gameObject.AddComponent<KnockBackIndicatorCreator>().Initialize(_physicalSpell.correspondingSpell);
            }
        }

        private float _knockBackDistance;
        Collider spellCollider;
        PhysicalWeapon physicalWeapon;

        [SerializeField]
        bool isWeaponHit = false;

        private void Start()
        {
            spellCollider = GetComponent<Collider>();
            physicalWeapon = GetComponentInParent<PhysicalWeapon>();
        }

        private void KnockBack(GameObject target, float knockBackMultiplier = 1.0f)
        {
            CreatureReference parent = target.GetComponentInParent<CreatureReference>();
            
            if(!parent.TryGetComponent(out Rigidbody rigidBody))
                rigidBody = parent.GetComponent<Rigidbody>();

            Vector3 direction = isWeaponHit ? physicalWeapon.strikeDirection : (target.transform.position - transform.position).normalized;

            Creature targetCreature = parent.creature;
            
            targetCreature.statusConditionTracker.AddStatusCondition(AllStatusConditions.ConvertEnumToStatusCondition(AllStatusConditions.StatusConditionName.KnockedBack, ref targetCreature, 
                new KnockedBackStatusSettings(source: _physicalSpell.correspondingSpell.GetCaster()), _physicalSpell.correspondingSpell.name, true));

            float targetKnockBackTakenMultiplier = targetCreature.modifiers.effects.Select(effect => effect.knockBackTaken).Product();
            float casterKnockBackDealtMultiplier = physicalSpell.correspondingSpell.GetCaster().modifiers.effects.Select(effect => effect.knockBackDealt).Product();

            rigidBody.AddForce(direction * _knockBackDistance * knockBackMultiplier * targetKnockBackTakenMultiplier * casterKnockBackDealtMultiplier * CombatSettings.Spells.KnockbackForceMultiplier, ForceMode.VelocityChange);

            if (spellCollider == null)
                return;

            DisableCollisionsWithHitCreature(target);            
        }

        public void KnockBackWhole(GameObject target) => KnockBack(target, 1.0f);
        public void KnockBackHalf(GameObject target) => KnockBack(target, 0.5f);

        public void KnockBackWhole(CreatureReference target) => KnockBack(target.gameObject, 1.0f);
        public void KnockBackHalf(CreatureReference target) => KnockBack(target.gameObject, 0.5f);

        private void DisableCollisionsWithHitCreature(GameObject hitCreature)
        {          
            CreatureReference parentCreature = hitCreature.GetComponentInParent<CreatureReference>();

            parentCreature.GetComponentsInChildren<Collider>().ToList().ForEach((collider) => 
            { 
                Physics.IgnoreCollision(collider, spellCollider);
            });
        }

        private void OnDestroy()
        {
            KnockBackIndicatorCreator knockBackIndicatorCreator = GetComponentInParent<KnockBackIndicatorCreator>();

            if (knockBackIndicatorCreator is null)
                return;

            Destroy(knockBackIndicatorCreator);
        }
    }
}