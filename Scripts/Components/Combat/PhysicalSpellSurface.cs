using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    [RequireComponent(typeof(Collider))]
    public class PhysicalSpellSurface : MonoBehaviour, ISpellReferencer
    {
        private List<Damage> _damage = new List<Damage>();
        public Guid guid { get; private set; }

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

                UpdateSpellInformation();
                guid = Guid.NewGuid();
            } 
        }

        public AttackSurface attackSurface => ToAttackSurface();

        public UnityEvent<GameObject> OnHit = new UnityEvent<GameObject>();

        private void Start()
        {
            PhysicalWeapon physicalWeapon;

            if (GetComponent<PhysicalSpell>().isAttachedToCaster)
                physicalWeapon = GetComponent<PhysicalSpell>().spellCaster.itemEquipper.GetWeaponInHand(HandSide.Right);

            else
                physicalWeapon = GetComponentInParent<PhysicalWeapon>();

            DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(gameObject, physicalWeapon.wielder.gameObject);
        }

        private AttackSurface ToAttackSurface()
        {
            UpdateSpellInformation();

            return new SpellSurface
            {
                caster = PlayerCharacterReference.Instance.creature,
                damage = _damage
            };
        }

        public void UpdateSpellInformation()
        {
            _damage = physicalSpell.correspondingSpell.modifiedDamageSources;
        }
    }
}
