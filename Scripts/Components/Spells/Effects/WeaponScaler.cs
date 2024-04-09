using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class WeaponScaler : PhysicalWeaponModifier
    {
        [SerializeField]
        float amountToScale = 2.0f;

        PhysicalHandHeldItem item;

        private const float ScaleTime = 1.0f;

        protected override void ModifyWeapon(GameObject gameObject)
        {
            item = gameObject.GetComponent<PhysicalHandHeldItem>();

            StopAllCoroutines();
            StartCoroutine(ScaleWeaponRoutine(amountToScale));
        }

        protected override void ResetWeapon(GameObject gameObject)
        {
            if (item == null)
                return;

            StopAllCoroutines();
            item.StartCoroutine(ScaleWeaponRoutine(1.0f));
        }

        IEnumerator ScaleWeaponRoutine(float newScale)
        {
            float startTime = Time.time;

            while (Time.time - startTime < ScaleTime)
            {
                float scaleAmount = UtilMath.Lmap(Time.time - startTime, 0.0f, ScaleTime, 0.0f, 1.0f);

                item.transform.localScale = Vector3.Lerp(item.transform.localScale, Vector3.one * newScale, scaleAmount);
                item.RefreshConnectedJoint();

                yield return null;
            }
        }
    }
}