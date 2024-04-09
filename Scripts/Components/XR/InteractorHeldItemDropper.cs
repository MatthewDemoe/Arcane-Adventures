using UnityEngine;
using System;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR
{
    public static class InteractorHeldItemDropper 
    {      
        public static void DropHeldItems()
        {
            XRReferences xRReferences = XRReferences.Instance;
            XRBaseInteractor leftInteractor = xRReferences.leftHandController.GetComponent<XRBaseInteractor>();
            XRBaseInteractor rightInteractor = xRReferences.rightHandController.GetComponent<XRBaseInteractor>();

            leftInteractor.interactionManager.CancelInteractorSelection(leftInteractor);
            rightInteractor.interactionManager.CancelInteractorSelection(rightInteractor);
        }
    }
}
