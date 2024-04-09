using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class KnockBackIndicator : MonoBehaviour
    {
        GameObject creatureBeingKnockedBack;
        Collider targetingCollider;
        int environmentLayerMask;

        RaycastHit downHit;
        Vector3 newPosition;

        Vector3 creaturePosition;
        Vector3 closestPointOnCollider;
        Vector3 creatureToTargeterPosition;
        Vector3 creatureToClosestPointOnCollider;

        Vector3 arrowForward;

        private void Awake()
        {
            environmentLayerMask = 1 << (int)Layers.Environment;
        }

        public void Initialize(GameObject creatureBeingKnockedBack, Collider targetingCollider, float knockBackDistance)
        {
            this.creatureBeingKnockedBack = creatureBeingKnockedBack;
            this.targetingCollider = targetingCollider;

            transform.localScale = new Vector3(1.0f, 1.0f, knockBackDistance);
        }       

        void Update()
        {
            if (targetingCollider == null)
                return;

            arrowForward = GetKnockBackDirection();
            if(arrowForward != Vector3.zero)
                transform.forward = GetKnockBackDirection();

            PositionDecal();

        }

        //TODO: Improve calculation
        private Vector3 GetKnockBackDirection()
        {         
            creaturePosition = creatureBeingKnockedBack.transform.position;
            creaturePosition.y = 0;

            closestPointOnCollider = targetingCollider.ClosestPoint(creatureBeingKnockedBack.transform.position);
            closestPointOnCollider.y = 0;

            creatureToTargeterPosition = creaturePosition - targetingCollider.transform.position;
            creatureToTargeterPosition.y = 0;
            creatureToTargeterPosition.Normalize();

            creatureToClosestPointOnCollider = (closestPointOnCollider - creaturePosition).normalized;
                        
            creatureToClosestPointOnCollider = Quaternion.AngleAxis(
                Vector3.SignedAngle(creatureToClosestPointOnCollider, creatureToTargeterPosition, Vector3.up) * 2.0f,
                Vector3.up) * creatureToClosestPointOnCollider;

            return creatureToClosestPointOnCollider;
        }

        private void PositionDecal()
        {
            Physics.Raycast(creatureBeingKnockedBack.transform.position, Vector3.down, out downHit, Mathf.Infinity, environmentLayerMask);

            newPosition = downHit.point;
            newPosition += (transform.forward * transform.localScale.z / 2.0f);
            transform.position = newPosition;
        }
    }
}