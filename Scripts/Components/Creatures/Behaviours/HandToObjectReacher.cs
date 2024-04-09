using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours
{
    [RequireComponent(typeof(RigBuilder))]
    public class HandToObjectReacher : MonoBehaviour
    {
        [SerializeField]
        TwoBoneIKConstraint ikConstraint;

        RigBuilder rigBuilder;

        GameObject oldTransformObject;

        const float ReachDuration = 1.0f;
        float reachTimer = 0.0f;

        void Start()
        {
            rigBuilder = GetComponent<RigBuilder>();
        }

        public void SetIKTarget(GameObject gameObject)
        {
            StopAllCoroutines();

            ikConstraint.data.target = gameObject.transform;

            StartCoroutine(nameof(BeginReach));
        }

        public void RemoveIKTarget(GameObject gameObject)
        {
            StopAllCoroutines();

            oldTransformObject = Instantiate(new GameObject());
            oldTransformObject.transform.position = gameObject.transform.position;

            ikConstraint.data.target = oldTransformObject.transform;

            StartCoroutine(nameof(EndReach));
        }

        IEnumerator BeginReach()
        {
            StartCoroutine(LerpIKWeight(ikConstraint.weight, 1.0f));

            yield return new WaitUntil(() => reachTimer >= ReachDuration);
        }

        IEnumerator EndReach()
        {
            StartCoroutine(LerpIKWeight(ikConstraint.weight, 0.0f));

            yield return new WaitUntil(() => reachTimer >= ReachDuration);

            ikConstraint.data.target = null;
            rigBuilder.Build();

            Destroy(oldTransformObject);
        }

        IEnumerator LerpIKWeight(float fromAmount, float toAmount)
        {
            reachTimer = 0.0f;
            float reachTimerNormalized = 0.0f;

            while (reachTimer <= ReachDuration)
            {
                reachTimerNormalized = UtilMath.Lmap(reachTimer, 0.0f, ReachDuration, fromAmount, toAmount);
                UtilMath.EasingFunction.EaseOutCirc(fromAmount, toAmount, reachTimerNormalized);

                ikConstraint.weight = reachTimerNormalized;
                rigBuilder.Build();

                yield return new WaitForEndOfFrame();

                reachTimer += Time.deltaTime;
            }
        }

        private void OnDestroy()
        {
            EndReach();
        }
    }
}