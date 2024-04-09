using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR
{
    public class XRReferences : MonoBehaviour
    {
        private static XRReferences _instance;
        public static XRReferences Instance => _instance;
        public static bool Exists => Instance != null;

        [SerializeField] private GameObject _leftHandController;
        [SerializeField] private GameObject _rightHandController;

        public GameObject leftHandController => _leftHandController;
        public GameObject rightHandController => _rightHandController;

        public static GameObject GetController(HandSide handSide) => handSide.Equals(HandSide.Left) ? Instance.leftHandController : Instance.rightHandController;

        private void Awake()
        {
            //TODO: Use dependency injection.
            _instance = this;
        }

        public Vector3 headsetPosition { get; private set; }
        public Quaternion headsetRotation { get; private set; }

        public void SetHeadsetPosition(Vector3 position)
        {
            headsetPosition = position;
        }

        public void SetHeadsetRotation(Quaternion rotation)
        {
            headsetRotation = rotation;
        }
    }
}