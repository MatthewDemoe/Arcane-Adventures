using System.Collections;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Lookups;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Materials
{
    public class DissolveEffect : MonoBehaviour
    {        
        MeshRenderer _renderer;

        Material dissolveMaterial;

        private Material[] originalMaterials;
        private Material[] dissolveMaterials;
        private MaterialPropertyBlock materialPropertyBlock;

        public float dissolveAmount { get; private set; } = 1.0f;

        private void Start()
        {
            _renderer = GetComponent<MeshRenderer>();
            dissolveMaterial = Prefabs.Materials.Load(Prefabs.Materials.Dissolve);

            int numMaterials = _renderer.sharedMaterials.Length;

            materialPropertyBlock = new MaterialPropertyBlock();
            originalMaterials = _renderer.materials;
            dissolveMaterials = new Material[numMaterials];

            for (int i = 0; i < numMaterials; i++)
            {
                materialPropertyBlock.Clear();

                materialPropertyBlock.SetTexture(ShaderProperties.BaseMap, _renderer.materials[i].GetTexture(ShaderProperties.BaseMap));
                materialPropertyBlock.SetTexture(ShaderProperties.MetallicMap, _renderer.materials[i].GetTexture(ShaderProperties.MetallicMap));
                materialPropertyBlock.SetTexture(ShaderProperties.NormalMap, _renderer.materials[i].GetTexture(ShaderProperties.NormalMap));

                materialPropertyBlock.SetFloat(ShaderProperties.Smoothness, _renderer.materials[i].GetFloat(ShaderProperties.Smoothness));

                _renderer.SetPropertyBlock(materialPropertyBlock, i);
                dissolveMaterials[i] = dissolveMaterial;
            }
        }

        public static void Dissolve(MeshRenderer meshRenderer, float clipOnBegin, float clipOnEnd, float duration)
        {
            DissolveEffect dissolveComponent;

            if (!meshRenderer.gameObject.TryGetComponent(out dissolveComponent))
                dissolveComponent = meshRenderer.gameObject.AddComponent<DissolveEffect>();

            dissolveComponent.StartDissolve(clipOnBegin, clipOnEnd, duration);
        }

        public void StartDissolve(float clipOnBegin, float clipOnEnd, float duration)
        {
            StopAllCoroutines();

            transform.parent.gameObject.SetActive(true);

            StartCoroutine(DissolveRoutine(clipOnBegin, clipOnEnd, duration));
        }

        public void StartDissolveFromCurrent(float clipOnEnd, float duration)
        {
            StopAllCoroutines();

            transform.parent.gameObject.SetActive(true);

            StartCoroutine(DissolveRoutine(dissolveAmount, clipOnEnd, duration));
        }

        IEnumerator DissolveRoutine(float clipOnBegin, float clipOnEnd, float duration)
        {
            float elapsedTime = 0.0f;
            float normalizedTime = 0.0f;

            while (elapsedTime <= duration)
            {
                yield return null;

                normalizedTime = UtilMath.Lmap(elapsedTime, 0.0f, duration, 0.0f, 1.0f);
                normalizedTime = Mathf.Clamp(normalizedTime, 0.0f, 1.0f);

                SetDissolveAmount(normalizedTime, clipOnBegin, clipOnEnd);

                elapsedTime += Time.deltaTime;
            }

        }

        private void SetDissolveAmount(float normalizedTime,float clipOnBegin, float clipOnEnd)
        { 

            dissolveAmount = UtilMath.Lmap(normalizedTime, 0.0f, 1.0f, clipOnBegin, clipOnEnd);

            if (dissolveAmount <= 0.0f)
            {
                _renderer.materials = originalMaterials;
                return;
            }

            _renderer.materials = dissolveMaterials;

            int numMaterials = _renderer.materials.Length;
            for (int i = 0; i < numMaterials; i++)
            {
                _renderer.GetPropertyBlock(materialPropertyBlock, i);

                if (dissolveAmount == clipOnEnd)
                {
                    StopAllCoroutines();
                    _renderer.materials = originalMaterials;
                    return;
                }

                materialPropertyBlock.SetFloat(ShaderProperties.ClipAmount, dissolveAmount);
                _renderer.SetPropertyBlock(materialPropertyBlock, i);
            }
            
        }
    }
}
