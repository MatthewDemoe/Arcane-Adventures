using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation
{
    public class ControllerLocationMimicker : MonoBehaviour
    {
        [SerializeField]
        GameObject leftHandIKLocation;

        [SerializeField]
        GameObject rightHandIKLocation;

        [SerializeField]
        GameObject headLocation;

        GameObject playerHeadObject;
        GameObject playerLeftHandObject;
        GameObject playerRightHandObject;

        public Transform HandTransform(HandSide handSide) => (handSide == HandSide.Left) ? leftHandIKLocation.transform : rightHandIKLocation.transform;


        void Start()
        {
            playerHeadObject = Camera.main.gameObject;

            playerLeftHandObject = XR.XRReferences.Instance.leftHandController;
            playerRightHandObject = XR.XRReferences.Instance.rightHandController;
        }

        void Update()
        {
            Vector3 playerHeadToLeftHand = playerLeftHandObject.transform.position - playerHeadObject.transform.position;
            Vector3 playerHeadToRightHand = playerRightHandObject.transform.position - playerHeadObject.transform.position;

            leftHandIKLocation.transform.localPosition = leftHandIKLocation.transform.worldToLocalMatrix * playerHeadToLeftHand;
            leftHandIKLocation.transform.forward = PlayerCharacterReference.Instance.transform.forward;

            rightHandIKLocation.transform.localPosition = rightHandIKLocation.transform.worldToLocalMatrix * playerHeadToRightHand;
            rightHandIKLocation.transform.forward = PlayerCharacterReference.Instance.transform.forward;
        }

        public void SetHeadLocation(Transform headTransform)
        {
            headLocation.transform.position = headTransform.position;
        }
    }
}