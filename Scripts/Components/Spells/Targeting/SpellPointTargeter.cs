using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class SpellPointTargeter : SpellTargeter
    {
        const float LerpSpeed = 125.0f;
        public override Vector3 targetDirection => (GetMaxRangeGroundPosition() - spellObject.transform.position).normalized;
        public Vector3 fromSelfToTargetPoint => GetMaxRangeGroundPosition() - downHit.point;
        public override Vector3 targetPoint => GetMaxRangeGroundPosition();


        protected GameObject decalInstance = null;
        protected GameObject spellObject;

        protected RaycastHit downHit;
        protected RaycastHit forwardHit;

        protected Vector3 _maxRangePoint = Vector3.zero;
        protected Vector3 decalScale = Vector3.one;

        protected float range = 0.0f;
        protected float frameLerpSpeed => Time.deltaTime * LerpSpeed;        

        protected int environmentLayerMask;

        public override void InitializeWithSpellInformation(Spell spell, SpellCaster spellCaster, HandSide handSide)
        {
            base.InitializeWithSpellInformation(spell, spellCaster, handSide);

            spellObject = spellCaster.spellObjectByHandSide[handSide];
            environmentLayerMask = 1 << (int)Layers.Environment;
            range = spell.range;
        }

        void Update()
        {
            if (decalInstance == null)
                return;

            PositionDecal();
        }

        protected virtual void PositionDecal()
        {
            Vector3 direction = GetTargetDirection();

            if (Physics.Raycast(transform.position, direction, out forwardHit, Mathf.Infinity, environmentLayerMask))
            {
                Physics.Raycast(transform.position, Vector3.down, out downHit, Mathf.Infinity, environmentLayerMask);

                _maxRangePoint = downHit.point + Vector3.ClampMagnitude(forwardHit.point - downHit.point, range);
            }
        }

        protected Vector3 GetMaxRangeGroundPosition()
        {
            if (Physics.Raycast(_maxRangePoint, Vector3.down, out RaycastHit groundHit, Mathf.Infinity, environmentLayerMask))
            {
                decalInstance.transform.up = groundHit.normal;

                return groundHit.point;
            }

            else if (Physics.Raycast(_maxRangePoint, Vector3.up, out RaycastHit ceilingHit, Mathf.Infinity, environmentLayerMask))
            {
                decalInstance.transform.up = ceilingHit.normal;

                return ceilingHit.point;
            }

            return _maxRangePoint;
        }

        private void OnDestroy()
        {
            Destroy(decalInstance);
        }
    }
}