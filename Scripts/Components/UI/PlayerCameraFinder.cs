using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class PlayerCameraFinder : MonoBehaviour
    {
        [SerializeField]
        Camera playerCamera = null;

        private void Awake()
        {
            IdentifyCamera();
        }
        // Start is called before the first frame update
        void Start()
        {
            IdentifyCamera();
        }

        // Update is called once per frame
        void Update()
        {
            IdentifyCamera();
        }

        void IdentifyCamera()
        {
            if (playerCamera == null)
            {
                var uiCamera = GameObject.Find("UI Camera");

                if (uiCamera == null)
                    return;
                
                playerCamera = uiCamera.GetComponent<Camera>();

                GetComponent<Canvas>().worldCamera = playerCamera;
            }
        }
    }
}
