using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using UnityEngine.AI;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Navigation
{
    [RequireComponent(typeof(NavMeshSurface))]
    public class NavMeshInstance : MonoBehaviour, IInjectable
    {
        NavMeshSurface navMeshSurface;

        private void Awake()
        {
            InjectorContainer.Injector.Bind(this);

            navMeshSurface = GetComponent<NavMeshSurface>();
            navMeshSurface.BuildNavMesh();
        }

        public void RebuildNavMesh()
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}