using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class ObjectLauncher : MonoBehaviour
    {
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private float launchInterval;
        [SerializeField] private Vector3 force;
        private float lastLaunchTime;

        private void Update()
        {
            if (lastLaunchTime + launchInterval < Time.time)
            {
                Launch();
            }
        }

        private void Launch()
        {
            var projectile = Instantiate(projectilePrefab, this.transform.position, this.transform.rotation);
            projectile.GetComponent<Rigidbody>().AddRelativeForce(force);
            lastLaunchTime = Time.time;
        }
    }
}