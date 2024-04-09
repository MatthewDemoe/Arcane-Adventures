using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    [RequireComponent(typeof(Canvas))]
    public class SetCanvasWorldCameraToMain : MonoBehaviour
    {
        private void Start()
        {
            var canvas = GetComponent<Canvas>();
            canvas.worldCamera = Camera.main;
        }
    }
}