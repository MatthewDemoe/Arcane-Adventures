using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.XR.Interactables
{

    public class XRBookInteractable : XRGrabInteractableSingleHand
    {
        protected override void Awake()
        {
            base.Awake();

            selectEntered.AddListener((selectEnterEventArgs) => colliders.ForEach((collider) => collider.enabled = false));
            selectExited.AddListener((selectEnterEventArgs) => colliders.ForEach((collider) => collider.enabled = true));
        }

    }
}
