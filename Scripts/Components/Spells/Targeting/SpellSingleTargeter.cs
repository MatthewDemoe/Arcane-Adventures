using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public enum Directions { X, Y, Z };

    public class SpellSingleTargeter : SpellTargeter
    {
        CapsuleCollider attachedCollider;

        public virtual CreatureReference targetedCreatureReference { get; private set; } = null;

        public List<AttackableSurface> targetedSurfaces { get; private set; } = new List<AttackableSurface>();

        private GameObject targetDecal;

        protected int environmentLayerMask = 1 << (int) Layers.Environment;

        //TODO: Potentially find the target in a more comprehensive way
        public virtual GameObject targetGameObject => targetedSurfaces[0].gameObject;

        private const float RaycastDistance = 10.0f;

        //TODO: Scale to enemy size
        private const float DecalScale = 2.0f;

        private void Awake()
        {
            if (!TryGetComponent(out attachedCollider))
                attachedCollider = gameObject.AddComponent<CapsuleCollider>();

            attachedCollider.isTrigger = true;

            PhysicalWeapon physicalWeapon = GetComponentInParent<PhysicalWeapon>();
            DisableCasterSpellCollision.SetIgnoreCasterCollisionForSpell(gameObject, physicalWeapon.wielder.gameObject);
        }

        public override void InitializeWithSpellInformation(Spell spell, SpellCaster spellCaster, HandSide handSide)
        {
            base.InitializeWithSpellInformation(spell, spellCaster, handSide);

            attachedCollider.direction = (int)Directions.Z;
            attachedCollider.height = spell.range;
            attachedCollider.radius = 1.0f;

            attachedCollider.center =  new Vector3(0.0f, 0.0f, spell.range / 2.0f);

            attachedCollider.transform.forward = GetTargetDirection();
        }

        private void Update()
        {
            PositionTargetDecal();
        }

        private void PositionTargetDecal()
        {
            if ((targetDecal == null) || (targetedCreatureReference == null))
                return;

            if (Physics.Raycast(targetedCreatureReference.transform.position + Vector3.up, Vector3.down, out RaycastHit raycastHit, RaycastDistance, environmentLayerMask))
            {
                targetDecal.transform.position = raycastHit.point;
                targetDecal.transform.up = raycastHit.normal;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out AttackableSurface attackableSurface))
                return;

            CreatureReference nonPlayerCreatureReference = other.GetComponentInParent<CreatureReference>();
            if (nonPlayerCreatureReference == null)
                return;

            if (targetedCreatureReference == null)
            {
                targetedCreatureReference = nonPlayerCreatureReference;

                if (targetDecal != null)
                    return;

                targetDecal = Instantiate(Prefabs.UI.Load(Prefabs.UI.AoEDecal), targetedCreatureReference.transform.position, Quaternion.identity);
                targetDecal.transform.localScale = new Vector3(DecalScale, 0.5f, DecalScale);
            }

            if (nonPlayerCreatureReference != targetedCreatureReference)
                return;

            targetedSurfaces.Add(attackableSurface);
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out AttackableSurface attackableSurface))
                return;

            if (!targetedSurfaces.Contains(attackableSurface))
                return;

            targetedSurfaces.Remove(attackableSurface);

            if (!(targetedSurfaces.Count == 0))
                return;

            targetedCreatureReference = null;

            Destroy(targetDecal);
        }

        private void OnDestroy()
        {
            Destroy(targetDecal);
        }
    }
}
