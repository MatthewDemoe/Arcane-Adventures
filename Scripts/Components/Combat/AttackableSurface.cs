using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using Injection;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    [RequireComponent(typeof(Collider))]
    public class AttackableSurface : MonoBehaviour
    {
        [SerializeField] private string bodyPartName;

        [Inject] protected ICombatSystem combatSystem;

        public AttackableSurfaceCoordinator attackableSurfaceCoordinator { get; private set; }
        public CreatureReference creatureReference { get; private set; }
        public Collider[] colliders { get; private set; }
        public string surfaceName => bodyPartName;//TODO: Poor naming.

        public UnityEvent OnSurfaceAttacked = new UnityEvent();

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);

            colliders = GetComponents<Collider>();
            attackableSurfaceCoordinator = TryGetComponent<AttackableSurfaceCoordinator>(out var newAttackableSurfaceCoordinator) ?
                newAttackableSurfaceCoordinator : 
                GetComponentInParent<AttackableSurfaceCoordinator>();

            creatureReference = attackableSurfaceCoordinator.creatureReference;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var physicalSpellSurfaces = GetPhysicalSpellSurfaces(collision);

            if (physicalSpellSurfaces.Any())
            {
                HandleSpellAttack(physicalSpellSurfaces.First());//TODO: If multiple, sort out which attack surface is the most valid one.
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var physicalSpellSurface = other.GetComponent<PhysicalSpellSurface>();

            if (physicalSpellSurface != null)
            {
                HandleSpellAttack(physicalSpellSurface);
            }
        }

        private IEnumerable<PhysicalSpellSurface> GetPhysicalSpellSurfaces(Collision collision)
        {
            var validPhysicalAttackSurfaces = new List<PhysicalSpellSurface>();

            for (var i = 0; i < collision.contactCount; i++)
            {
                var validAttackSurface = collision.GetContact(i).otherCollider.GetComponent<PhysicalSpellSurface>();

                if (validAttackSurface != null)
                {
                    validPhysicalAttackSurfaces.Add(validAttackSurface);
                }
            }

            return validPhysicalAttackSurfaces;
        }

        private void HandleSpellAttack(PhysicalSpellSurface physicalSpellSurface)
        {
            if (creatureReference.creature.isDead || !attackableSurfaceCoordinator.RegisterStrikeGuid(physicalSpellSurface.guid))
            {
                return;
            }

            var caster = physicalSpellSurface.physicalSpell.correspondingSpell.GetCaster();
            var spellSurface = physicalSpellSurface.attackSurface as SpellSurface;
            var magicHit = new MagicHit(caster, creatureReference.creature, spellSurface.damage);

            creatureReference.GetComponentsInChildren<ComboCastEffectApplier>().ToList().ForEach((combo) =>
            {
                combo.CheckForCombos(physicalSpellSurface.physicalSpell.correspondingSpell);
            });

            combatSystem.ReportHit(magicHit);
            physicalSpellSurface.OnHit.Invoke(gameObject);
            OnSurfaceAttacked.Invoke();
        }

        public void SetName(string name)
        {
            if (bodyPartName != null && bodyPartName != string.Empty) { throw new Exception($"Attackable surface name already set: {bodyPartName}"); }
            bodyPartName = name;
        }
    }
}