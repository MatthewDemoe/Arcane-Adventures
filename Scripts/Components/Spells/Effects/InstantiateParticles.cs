using UnityEngine;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class InstantiateParticles : MonoBehaviour
    {
        [SerializeField]
        GameObject particlePrefab;
        GameObject particleInstance;

        public void InstantiatePrefab()
        {
            particleInstance = Instantiate(particlePrefab, transform.position, transform.rotation);
        }
    }
}
