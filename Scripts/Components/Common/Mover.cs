using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class Mover : MonoBehaviour
    {
        [SerializeField] private Vector3 speedPerSecond;

        private void Update()
        {
            this.transform.position += speedPerSecond * Time.deltaTime;
        }
    }
}