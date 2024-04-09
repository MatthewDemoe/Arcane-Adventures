using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class SwordTestNPCCreatureController : MonoBehaviour, ICreatureController
    {
        public Vector3 targetPosition { get; set; }
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