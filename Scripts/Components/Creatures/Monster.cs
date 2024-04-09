using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public abstract class Monster : CreatureReference
    {
        [SerializeField] private GameObject _overHeadAnchor;
        public GameObject overHeadAnchor => _overHeadAnchor;

        protected CreatureBehaviour creatureBehaviour;

        protected override void Awake()
        {
            base.Awake();

            creatureBehaviour = GetComponent<CreatureBehaviour>();
        }

        public override void ProcessDamage()
        {
            base.ProcessDamage();

            if (creature.isDead)
            {
                overHeadAnchor.SetActive(false);
            }
        }

        protected override void ProcessWeaponClash(PhysicalWeapon physicalWeapon)
        {
            base.ProcessWeaponClash(physicalWeapon);

            if (isAttacking)
            {
                animator.SetFloat(CharacterAnimatorParameters.AttackSpeed, 0.25f);
                animator.SetBool(CharacterAnimatorParameters.AttackWasParried, true);
            }
        }
    }
}