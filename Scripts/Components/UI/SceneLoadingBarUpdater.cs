using UnityEngine;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class SceneLoadingBarUpdater : MonoBehaviour
    {
        [SerializeField]
        Image loadingBarInterior;
    
        public static SceneLoadingBarUpdater Instance { get; private set; }
    
        private void Start()
        {
            Instance = this;
            ShowLoadingBar(false);
            
        }
    
        private void Update()
        {
            if (SceneLoader.loadingWithFadeEffect)
            {
                loadingBarInterior.fillAmount = SceneLoader.progress;
            }
        }
    
        public void ShowLoadingBar(bool val)
        {
            gameObject.SetActive(val);
        }
    }
}
