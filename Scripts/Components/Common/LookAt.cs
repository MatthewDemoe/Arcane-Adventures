using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class LookAt : MonoBehaviour
    {
        [SerializeField] private bool setTargetToMainCamera;

        public GameObject target;

        private void Start()
        {
            if (setTargetToMainCamera)
            {
                target = Camera.main.gameObject;
            }
        }

        private void Update()
        {
            if (target is GameObject)
            {
                var lookAtPosition = transform.position - target.transform.position;

                if (lookAtPosition != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(lookAtPosition);
                }
            }
        }
    }
}