using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.AnimationHelpers
{
    public class BookAnimationHelper : MonoBehaviour
    {
        Animator bookAnimator;

        private const string IsOpen = "isOpen";

        // Start is called before the first frame update
        void Start()
        {
            bookAnimator = GetComponent<Animator>();
        }


        public void SetBookOpen(bool isOpen)
        {
            bookAnimator.SetBool(IsOpen, isOpen);
        }
    }
}
