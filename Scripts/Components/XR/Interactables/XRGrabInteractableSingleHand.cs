using UnityEngine.XR.Interaction.Toolkit;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR.Interactables
{
    public class XRGrabInteractableSingleHand : XRGrabInteractable
    {
        //Stops you from grabbing the object with the second hand
        public override bool IsSelectableBy(XRBaseInteractor interactor)
        {
            bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        
            return base.IsSelectableBy(interactor) && !isAlreadyGrabbed;
        }
    }
}
