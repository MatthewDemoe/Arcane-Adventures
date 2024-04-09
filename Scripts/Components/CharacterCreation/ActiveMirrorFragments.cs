using UnityEngine;
using System.Collections;
using RayFire;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{
    public class ActiveMirrorFragments : MonoBehaviour
    {
        GameObject _activeMirror = null;
        const float duration = 2.0f;


        private static ActiveMirrorFragments _instance;
        public static ActiveMirrorFragments Instance => _instance;

        private void Awake()
        {
            _instance = this;

            if (_activeMirror == null)
                return;

            Material sharedFragmentMaterial = _activeMirror.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial;
            sharedFragmentMaterial.SetFloat(ShaderProperties.ClipAmount, 0.0f);
        }

        public void SetActiveMirror(GameObject activeMirror)
        {
            if (_activeMirror == activeMirror)
                return;

            ResetActiveMirror();

            _activeMirror = activeMirror;
            activeMirror.SetActive(true);
            
        }

        public void StartFragmentFadeOut()
        {
            if (_activeMirror == null)            
                return;

            StartCoroutine(FadeOutFragments());
        }

        public void ResetActiveMirror()
        {
            if (_activeMirror == null)
                return;

            _activeMirror.transform.GetChild(0).GetComponent<RayfireRigid>().ResetRigid();

            _activeMirror.SetActive(false);

            _activeMirror = null;
        }

        IEnumerator FadeOutFragments()
        {
            float elapsedTime = 0.0f;
            float normalizedTime = 0.0f;

            Material sharedFragmentMaterial = _activeMirror.transform.GetChild(0).GetChild(0).gameObject.GetComponent<MeshRenderer>().sharedMaterial;


            while (elapsedTime < duration)
            {
                yield return null;

                elapsedTime += Time.deltaTime;

                normalizedTime = UtilMath.Lmap(elapsedTime, 0.0f, duration, 0.0f, 1.0f);                

                sharedFragmentMaterial.SetFloat(ShaderProperties.ClipAmount, normalizedTime);
            }

            ResetActiveMirror();
            sharedFragmentMaterial.SetFloat(ShaderProperties.ClipAmount, 0.0f);
        }
    }
}
