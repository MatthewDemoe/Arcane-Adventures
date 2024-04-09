using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Book
{
    public class BookController : MonoBehaviour
    {
        public int currentPage { get; private set; } = 0;

        public int lastPage => _bookPages.Count - 1;

        [SerializeField]
        GameObject leftPageCorner;
        [SerializeField]
        GameObject rightPageCorner;

        [SerializeField]
        private List<BookPage> _bookPages = new List<BookPage>();
        public List<BookPage> bookPages 
        {
            get { return _bookPages; }
            private set 
            {
                _bookPages = value;
            }
        }

        protected void Start()
        {
            CheckPageCorners(0);
        }

        public void FlipPage(int direction)
        {
            direction = Mathf.Clamp(direction, -1, 1);

            currentPage = Mathf.Clamp(currentPage + direction, 0, lastPage);

            DisableAllPages();
            EnableCurrentPage();

            CheckPageCorners(0);
        }

        void DisableAllPages()
        {
            foreach (BookPage page in _bookPages)
            {
                page.SetPagesActive(false);
            }
        }

        void EnableCurrentPage()
        {
            _bookPages[currentPage].SetPagesActive(true);
        }

        public void ResetCurrentPage()
        {
            DisableAllPages();
            EnableCurrentPage();
        }

        public void StartFlip(int direction)
        {
            if (direction > 0)
            {
                bookPages[currentPage].rightPage.SetActive(false);
                bookPages[currentPage + 1].rightPage.SetActive(true);                             
            }

            else
            {
                bookPages[currentPage].leftPage.SetActive(false);
                bookPages[currentPage - 1].leftPage.SetActive(true);
            }

            CheckPageCorners(direction);
        }

        public void CheckPageCorners(int nextPageDirection)
        {
            int nextPage = currentPage + nextPageDirection;

            if (nextPageDirection >= 0)
                CheckRightPageCorner(nextPage);

            if (nextPageDirection <= 0)
                CheckLeftPageCorner(nextPage);

        }

        private void CheckLeftPageCorner(int nextPage)
        {
            leftPageCorner.SetActive(!(nextPage <= 0));
        }

        private void CheckRightPageCorner(int nextPage)
        {
            rightPageCorner.SetActive(!(nextPage >= (bookPages.Count - 1)));
        }
    }
}
