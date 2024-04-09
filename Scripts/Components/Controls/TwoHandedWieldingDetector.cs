using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Controls
{
    [RequireComponent(typeof(Collider))]
    public class TwoHandedWieldingDetector : MonoBehaviour
    {
        private ControllerLink _otherControllerLink;
        private ControllerLink otherControllerLink
        {
            get
            {
                return _otherControllerLink == null ?
                    _otherControllerLink = ControllerLink.Get(controllerLink.handSide.Equals(HandSide.Left) ? HandSide.Right : HandSide.Left) :
                    _otherControllerLink;
            }
            set { _otherControllerLink = value; }
        }

        private ControllerLink controllerLink;
        private bool isOtherControllerInCollider;

        public bool isWieldingWithTwoHands => controllerLink.isHoldingItem && !otherControllerLink.isHoldingItem && isOtherControllerInCollider;

        private void Start()
        {
            controllerLink = GetComponentInParent<ControllerLink>();
            otherControllerLink = ControllerLink.Get(controllerLink.handSide.Equals(HandSide.Left) ? HandSide.Right : HandSide.Left);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == otherControllerLink.gameObject)
            {
                isOtherControllerInCollider = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject == otherControllerLink.gameObject)
            {
                isOtherControllerInCollider = false;
            }
        }
    }
}