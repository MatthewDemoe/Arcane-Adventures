using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private float spaceBetweenTargetAndThis;
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
            this.transform.position = target.transform.position + target.transform.forward * spaceBetweenTargetAndThis;
        }
    }
}