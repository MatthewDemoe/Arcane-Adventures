using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Book
{

    public class BookPage : MonoBehaviour
    {
        [SerializeField]
        private GameObject _leftPage; 
        public GameObject leftPage 
        { 
            get { return _leftPage; }
            private set
            {
                _leftPage = value;
            }
        }

        [SerializeField]
        private GameObject _rightPage; 
        public GameObject rightPage 
        {
            get { return _rightPage; }
            private set
            {
                _rightPage = value;
            }
        }

        public void SetPagesActive(bool isActive)
        {
            _leftPage.SetActive(isActive);
            _rightPage.SetActive(isActive);
        }
    }
}
