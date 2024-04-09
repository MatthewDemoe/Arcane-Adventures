using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.AnimationHelpers
{
    public class FlippablePageAnimationHelper : MonoBehaviour
    {
        [HideInInspector]
        public XR.Interactables.XRPageInteractable pageInteractable = null;

        Animator pageAnimator;

        [SerializeField]
        float completeFlipDuration = 0.1f;

        [SerializeField]
        float undoFlipDuration = 0.75f;
        [SerializeField]
        float undoFlipSpeed = 2.0f;

        float pageDistance = 0.0f;

        const string pageDistanceParam = "PageDistance";
        bool flipping = true;
        public bool positiveDirection => pageInteractable.directionToOppositePage.x < 0.0f;


        private void Start()
        {
            pageAnimator = GetComponent<Animator>();
            UpdateAnimator();
        }

        private void Update()
        {
            UpdateAnimator();
        }

        void UpdateAnimator()
        {
            if (flipping)
                pageDistance = pageInteractable.handDistanceFromOppositePageNormalized;

            pageAnimator.SetFloat(pageDistanceParam, positiveDirection ? pageDistance : -pageDistance);
        }

        public void CompleteFlip()
        {
            flipping = false;
            StartCoroutine(nameof(CompleteFlipRoutine));
        }

        public void UndoFlip()
        {
            undoFlipDuration = (1.0f - pageDistance) / undoFlipSpeed;

            flipping = false;
            StartCoroutine(nameof(UndoFlipRoutine));
        }

        IEnumerator CompleteFlipRoutine()
        {
            float timer = 0.0f;
            float duration = completeFlipDuration;
            float pageDistanceAtStart = pageDistance;

            while(timer < duration)
            {
                yield return null;

                timer += Time.deltaTime;

                pageDistance = UtilMath.Lmap(timer, 0.0f, duration, pageDistanceAtStart, -1.0f);

            }

            pageInteractable.OnPageFlip.Invoke();
            pageInteractable.SetFlippingFalse();

            Destroy(gameObject);
        }

        IEnumerator UndoFlipRoutine()
        {

            float timer = 0.0f;
            float pageDistanceAtStart = pageDistance;

            while (timer < undoFlipDuration)
            {
                yield return null;

                timer += Time.deltaTime;

                pageDistance = UtilMath.Lmap(timer, 0.0f, undoFlipDuration, pageDistanceAtStart, 1.0f);
            }

            pageInteractable.OnPageFlipUndo.Invoke();
            pageInteractable.SetFlippingFalse();

            Destroy(gameObject);
        }
    }
}
