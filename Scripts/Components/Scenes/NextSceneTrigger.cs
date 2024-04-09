using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Scenes
{
    public class NextSceneTrigger : MonoBehaviour
    {
        private bool triggered = false;

        [SerializeField] private GameScene scene;
        [SerializeField] private bool useFadeEffect;
        [SerializeField] private bool resetGame;
        [SerializeField] private bool isTriggeredOnStart;

        private void Start()
        {
            if (isTriggeredOnStart)
            {
                triggered = true;
                LoadScene();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!triggered && other.gameObject == PlayerCharacterReference.Instance.gameObject)
            {
                triggered = true;
                LoadScene();
            }
        }

        private void LoadScene()
        {
            if (resetGame)
            {
                ResetGame();
            }

            if (useFadeEffect)
            {
                SceneLoader.LoadWithFadeEffect(scene);
            }
            else
            {
                SceneLoader.Load(scene);
            }
        }

        private void ResetGame()
        {

            foreach (var o in FindObjectsOfType<GameObject>())
            {
                Destroy(o);
            }
        }
    }
}