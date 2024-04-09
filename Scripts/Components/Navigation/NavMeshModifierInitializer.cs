using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using UnityEngine.AI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Navigation
{
    [RequireComponent(typeof(NavMeshModifierVolume))]
    public class NavMeshModifierInitializer : MonoBehaviour
    {
        [SerializeField]
        NavigationArea navigationArea;

        [SerializeField]
        bool onAwake = true;
        public bool shouldPlayOnAwake => onAwake;

        NavMeshModifierVolume navMeshModifierVolume;
        NavMeshInstance navMeshInstance;

        private void Awake()
        {
            navMeshInstance = InjectorContainer.Injector.GetInstance<NavMeshInstance>();

            navMeshModifierVolume = GetComponent<NavMeshModifierVolume>();
            navMeshModifierVolume.area = (int)navigationArea;

            if (onAwake)
                InitializeArea();
        }

        public void InitializeArea()
        {
            navMeshInstance.RebuildNavMesh();
        }

        private void OnDestroy()
        {
            navMeshInstance.RebuildNavMesh();
        }
    }
}