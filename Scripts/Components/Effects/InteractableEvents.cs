using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine;
using UnityEngine.Events;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class InteractableEvents : MonoBehaviour
    {
        [SerializeField]
        UnityEvent<GameObject> OnSelectEntered = new UnityEvent<GameObject>();

        [SerializeField]
        UnityEvent<GameObject> OnSelectExited = new UnityEvent<GameObject>();

        public void TriggerOnSelectEntered(SelectEnterEventArgs args)
        {
            OnSelectEntered.Invoke(gameObject);
        }

        public void TriggerOnSelectExited(SelectExitEventArgs args)
        {
            OnSelectExited.Invoke(gameObject);
        }
    }
}