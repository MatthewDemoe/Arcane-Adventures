using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class ExampleNPCCreatureController : MonoBehaviour, ICreatureController
    {
        [SerializeField] private Transform target;
        
        public Vector3 targetPosition => target.position - transform.position;
        public float targetSpeed => 1;
        public Vector3 lookPosition { get; }
        public bool crouch { get; }
        public bool jump { get; }
        
        private void Awake()
        {
            GetComponent<CreatureControlInterpreter>().creatureController = this;
        }
    }
}