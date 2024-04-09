using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items
{
    [RequireComponent(typeof(PhysicalHandHeldItem))]
    public class ItemActivatedAbility : MonoBehaviour
    {
        public UnityEvent OnItemActivated = new UnityEvent();

        PhysicalHandHeldItem physicalHandHeldItem;

        void Start()
        {
            physicalHandHeldItem = GetComponent<PhysicalHandHeldItem>();
            physicalHandHeldItem.OnEquipped.AddListener(AddActivationListener);
            physicalHandHeldItem.OnUnequipped.AddListener(RemoveActivationListener);
        }

        private void AddActivationListener()
        {
            physicalHandHeldItem.wielder.GetComponent<PlayerCharacterCreatureController>().OnTriggerPressed.AddListener(ActivateItem);
        }

        private void RemoveActivationListener()
        {
            physicalHandHeldItem.wielder.GetComponent<PlayerCharacterCreatureController>().OnTriggerPressed.RemoveListener(ActivateItem);
        }

        private void ActivateItem(HandSide handSide)
        {
            if(physicalHandHeldItem.handSide == handSide)
                OnItemActivated.Invoke();
        }
    }
}