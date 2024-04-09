using UnityEngine;
using UnityEngine.AI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class NavMeshCreatureController : MonoBehaviour, ICreatureController
    {
        NavMeshAgent navMeshAgent;

        private const float DistanceFromAgentCutoff = 0.25f;

        public Vector3 targetPosition => navMeshAgent.desiredVelocity;
        public float targetSpeed => 1;

        public Vector3 lookPosition { get; }
        public bool crouch { get; }
        public bool jump { get; }

        private void Awake()
        {
            GetComponent<CreatureControlInterpreter>().creatureController = this;
            navMeshAgent = GetComponentInParent<NavMeshAgent>();
            navMeshAgent.updatePosition = false;
        }

        private void Update()
        {
            if (Vector3.Distance(navMeshAgent.nextPosition, transform.position) > DistanceFromAgentCutoff)
                navMeshAgent.nextPosition = transform.position;
        }
    }
}