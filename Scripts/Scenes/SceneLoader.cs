using UnityEngine;
using UnityEngine.SceneManagement;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using System;
using System.Collections;
using com.AlteredRealityLabs.ArcaneAdventures.Components.XR;

namespace com.AlteredRealityLabs.ArcaneAdventures.Scenes
{
    public class SceneLoader
    {       
        public static bool loadingWithFadeEffect { get; private set; } = false;

        private static float minimumLoadTime = 3.0f;
        private static float maxLoadTime = 15.0f;

        public static float progress { get; private set; } = 0.0f;

        public static void Load(GameScene scene)
        {
            SceneManager.LoadScene((int)scene);
        }

        public static void LoadWithFadeEffect(GameScene scene)
        {
            if (loadingWithFadeEffect)
            {
                throw new Exception("Scene loading with fade effect already in progress");
            }

            XRReferences.Instance.StartCoroutine(StartLoadingWithFadeEffect(scene));
        }

        public static IEnumerator StartLoadingWithFadeEffect(GameScene scene)
        {
            yield return null;

            loadingWithFadeEffect = true;

            CameraFader.FadeOut(() =>
                {
                    AsyncOperation preloadedScene = SceneManager.LoadSceneAsync((int)GameScene.Loading);

                    preloadedScene.completed += (asyncOperation) =>
                    {
                        SceneLoadingBarUpdater.Instance.ShowLoadingBar(true);
                        SceneLoadingBarUpdater.Instance.StartCoroutine(LoadWithMinimumTimeRoutine(scene));
                    };

                    CameraFader.FadeIn();
                }
            );
        }

        public static IEnumerator LoadWithMinimumTimeRoutine(GameScene scene)
        {
            progress = 0.0f;

            AsyncOperation preloadedScene = SceneManager.LoadSceneAsync((int)scene);
            preloadedScene.allowSceneActivation = false;

            //TODO: If loading times become a problem. 
            //Application.backgroundLoadingPriority = ThreadPriority.Low;

            float elapsedTime = 0.0f;
            float elapsedTimeNormalized = 0.0f;

            while (progress < 1.0f)
            {
                yield return null;

                elapsedTime += Time.deltaTime;
                elapsedTimeNormalized = UtilMath.Lmap(elapsedTime, 0.0f, minimumLoadTime, 0.0f, 1.0f);

                progress = UtilMath.Lmap(preloadedScene.progress, 0.0f, 1.0f, 0.0f, elapsedTimeNormalized);
            }

            CameraFader.FadeOut(() =>
                {
                    if (elapsedTime > maxLoadTime)
                    {
                        Debug.LogWarning($"Scene loading took {elapsedTime} seconds. Load time should be within {maxLoadTime} seconds");
                    }

                    loadingWithFadeEffect = false;
                    preloadedScene.allowSceneActivation = true;
                    preloadedScene = null;

                    CameraFader.FadeIn();
                    SceneLoadingBarUpdater.Instance.ShowLoadingBar(false);
                }
            );
        }
    }
}