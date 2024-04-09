using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Book;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR.Interactables
{
    public enum PageSide { Left, Right }


    public class XRPageInteractable : XRSimpleInteractable
    {
        BookController bookController;

        [SerializeField]
        PageSide pageSide;

        [SerializeField]
        Transform pageEdgeTransform;

        [SerializeField]
        XRPageInteractable oppositePage;
        Collider oppositePageEdgeCollider;

        [SerializeField]
        GameObject flippablePagePrefab;
        [SerializeField]
        Transform flippablePageParent;

        GameObject flippablePageInstance = null;
        XRBaseInteractor flippingInteractor = null;

        float autoEndFlipThreshold = 0.3f;

        public bool flipping = false;

        float distanceBetweenPages = 0.0f;
        float handDistanceFromOppositePage = 0.0f;
        public float handDistanceFromOppositePageNormalized { get; private set; } = 0.0f;

        public Vector3 directionToOppositePage { get; private set; } = new Vector3();

        public UnityEvent OnPageFlip = new UnityEvent();
        public UnityEvent OnPageFlipUndo = new UnityEvent();

        protected override void Awake()
        {
            bookController = GetComponentInParent<BookController>();

            base.Awake();

            selectEntered.AddListener(StartFlip);
            selectExited.AddListener(StopFlip);

            oppositePageEdgeCollider = oppositePage.pageEdgeTransform.gameObject.GetComponent<Collider>();

            OnPageFlipUndo.AddListener(bookController.ResetCurrentPage);
            OnPageFlipUndo.AddListener(() => bookController.CheckPageCorners(0));
        }

        public void StartFlip(SelectEnterEventArgs selectEnterEventArgs)
        {
            if ((pageSide == PageSide.Left) && (bookController.currentPage == 0))
                return;

            if ((pageSide == PageSide.Right) && (bookController.currentPage == bookController.lastPage))
                return;

            if (flipping)
                return;

            flipping = true;

            flippingInteractor = selectEnterEventArgs.interactor;

            distanceBetweenPages = (pageEdgeTransform.position - oppositePage.pageEdgeTransform.position).magnitude;
            directionToOppositePage = (pageEdgeTransform.localPosition - oppositePage.pageEdgeTransform.localPosition).normalized;
            CalculateHandDistance();

            flippablePageInstance = Instantiate(flippablePagePrefab, flippablePageParent);
            flippablePageInstance.GetComponent<AnimationHelpers.FlippablePageAnimationHelper>().pageInteractable = this;

            PageFlipContent pageFlipContent = flippablePageInstance.GetComponent<PageFlipContent>();
            
            pageFlipContent.SetFrontPageContent(bookController.bookPages[bookController.currentPage + (pageSide == PageSide.Left ? -1 : 0)].rightPage);
            pageFlipContent.SetBackPageContent(bookController.bookPages[bookController.currentPage + (pageSide == PageSide.Right ? 1 : 0)].leftPage);

            bookController.StartFlip(pageSide == PageSide.Left ? -1 : 1);
        }

        public void StopFlip(SelectExitEventArgs selectEnterEventArgs)
        {
            if (!flipping)
                return;

            if (handDistanceFromOppositePage <= autoEndFlipThreshold)
                CompleteFlip();

            else
                UndoFlip();
        }

        void CompleteFlip()
        {
            if(flippablePageInstance != null)
                flippablePageInstance.GetComponent<AnimationHelpers.FlippablePageAnimationHelper>().CompleteFlip();


        }

        void UndoFlip()
        {
            if (flippablePageInstance != null)
                flippablePageInstance.GetComponent<AnimationHelpers.FlippablePageAnimationHelper>().UndoFlip();
        }

        public void SetFlippingFalse()
        {
            flipping = false;
            flippablePageInstance = null;
            flippingInteractor = null;
        }

        private void Update()
        {
            if(flipping)
            {
                CalculateHandDistance();

                if(handDistanceFromOppositePage < autoEndFlipThreshold)
                {
                    StopFlip(new SelectExitEventArgs());
                }
            }
        }

        void CalculateHandDistance()
        {
            handDistanceFromOppositePage = Mathf.Clamp((oppositePageEdgeCollider.bounds.ClosestPoint(flippingInteractor.transform.position) - flippingInteractor.transform.position).magnitude, 0.0f, distanceBetweenPages);
            handDistanceFromOppositePageNormalized = UtilMath.Lmap(handDistanceFromOppositePage, 0.0f, distanceBetweenPages, -1.0f, 1.0f);
        }
    }
}

