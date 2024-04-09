using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    [RequireComponent(typeof(CapsuleCollider))]
    public class SpellArrowTargeter : SpellPointTargeter
    {
        CapsuleCollider attachedCollider;

        private void Awake()
        {
            attachedCollider = GetComponent<CapsuleCollider>();
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

            attachedCollider.center = new Vector3(0.0f, 0.0f, spell.range / 2.0f);

            attachedCollider.transform.forward = GetTargetDirection();

            decalInstance = Instantiate(Prefabs.UI.Load(Prefabs.UI.ArrowDecal), transform.position, transform.rotation);
            PositionDecal();
        }

        protected override void PositionDecal()
        {
            base.PositionDecal();

            float distance = Vector3.Distance(downHit.point, _maxRangePoint);
            Vector3 decalDirection = _maxRangePoint - downHit.point;
            decalDirection.y = 0;
            decalDirection.Normalize();

            Vector3 newPosition = downHit.point + (decalDirection * distance / 2.0f);

            Vector3 newScale = new Vector3(decalInstance.transform.localScale.x, Mathf.Clamp(Mathf.Abs(downHit.point.y - forwardHit.point.y), 0.5f, 2.0f), distance);

            decalInstance.transform.forward = Vector3.MoveTowards(decalInstance.transform.forward, decalDirection, frameLerpSpeed);
            decalInstance.transform.localScale = Vector3.MoveTowards(decalInstance.transform.localScale, newScale, frameLerpSpeed);
            decalInstance.transform.position = Vector3.MoveTowards(decalInstance.transform.position, newPosition, frameLerpSpeed);
        }
    }
}