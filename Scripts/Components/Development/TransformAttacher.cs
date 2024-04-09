using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class TransformAttacher : MonoBehaviour
    {
        private const string Hips = nameof(Hips);

        GameObject characterHips;
        Rigidbody attachedRigidbody;
        Transform attachTransform;

        bool isAttached = false;

        const float MaxDistanceDelta = 0.1f;

        public void Initialize(Transform attachTransform)
        {
            characterHips = transform.parent.GetComponentsInChildren<AttackableSurface>().First((attackableSurface) => attackableSurface.name.Contains(Hips)).gameObject;

            attachedRigidbody = gameObject.GetComponent<Rigidbody>();
            attachedRigidbody.isKinematic = true;

            isAttached = true;
            this.attachTransform = attachTransform;
        }

        public void Unattach()
        {
            isAttached = false;
            attachedRigidbody.isKinematic = false;

            Destroy(this);
        }

        private void Update()
        {
            if (isAttached)
            {
                Vector3 attachPointToHips = attachTransform.position - characterHips.transform.position;
                transform.position = Vector3.MoveTowards(transform.position, attachTransform.position + attachPointToHips, MaxDistanceDelta);
            }
        }
    }
}