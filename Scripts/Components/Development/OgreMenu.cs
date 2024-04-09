using UnityEngine;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class OgreMenu : MonoBehaviour
    {
        [SerializeField] private Toggle ogre1Toggle;
        [SerializeField] private Toggle ogre2Toggle;
        [SerializeField] private GameObject ogre1;
        [SerializeField] private GameObject ogre2;

        private void Start()
        {
            ogre1Toggle.onValueChanged.AddListener(delegate (bool value) { ogre1.SetActive(value); });
            ogre2Toggle.onValueChanged.AddListener(delegate (bool value) { ogre2.SetActive(value); });
        }
    }
}