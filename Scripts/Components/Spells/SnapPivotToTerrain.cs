using com.AlteredRealityLabs.ArcaneAdventures.Lookups;
using UnityEngine;
using NaughtyAttributes;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SnapPivotToTerrain : MonoBehaviour
    {
        [SerializeField]
        float unitDistanceToScaleMultiplier = 1.0f;

        [SerializeField]
        GameObject objectToScale;

        [SerializeField]
        bool rotateBaseToHitNormal = true;

        [SerializeField][ShowIf(nameof(rotateBaseToHitNormal))]
        GameObject baseToRotate;

        const float MaxRaycastDistance = 10.0f;

        void Start()
        {
            CheckGroundPosition();
            Destroy(this);
        }

        private void CheckGroundPosition()
        {
            int layerMask = 1 << (int)Layers.Environment;
            RaycastHit raycastHit;

            if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out raycastHit, MaxRaycastDistance, layerMask) || 
                Physics.Raycast(transform.position + Vector3.down, Vector3.up, out raycastHit, MaxRaycastDistance, layerMask))
                RepositionPivot(raycastHit);
        }

        private void RepositionPivot(RaycastHit collisionPoint)
        {
            float distance = transform.position.y - collisionPoint.point.y;
            transform.position = collisionPoint.point;

            objectToScale.transform.localScale = new Vector3(objectToScale.transform.localScale.x, objectToScale.transform.localScale.y * (1 + (distance * unitDistanceToScaleMultiplier)), objectToScale.transform.localScale.z);

            if (rotateBaseToHitNormal)
                baseToRotate.transform.up = collisionPoint.normal;
        }
    }
}