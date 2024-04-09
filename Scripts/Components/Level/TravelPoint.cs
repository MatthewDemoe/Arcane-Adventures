using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Scenes;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Level
{
    public class TravelPoint : MonoBehaviour
    {
        private const float FadeDistance = 30;
        private const float SceneLoadDistance = 10;

        private static Color32 White = new Color32(255, 255, 255, 255);

        [SerializeField] private GameScene scene;

        private bool isColorSet;

        private void Update()
        {
            var distance = Vector3.Distance(PlayerCharacterReference.Instance.transform.position, this.transform.position);

            if (distance > FadeDistance) { return; }

            SetColor();

            if (distance < SceneLoadDistance)
            {
                SceneLoader.LoadWithFadeEffect(scene);
                this.gameObject.SetActive(false);
            }
            else
            {
                var alpha = 1 - Mathf.InverseLerp(SceneLoadDistance, FadeDistance, distance);
                CameraFader.SetAlpha(alpha);
            }
        }

        private void SetColor()
        {
            if (!isColorSet)
            {
                CameraFader.SetColor(White);
                isColorSet = true;
            }
        }
    }
}