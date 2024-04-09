using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class EffectInvokerOnWeaponClash : MonoBehaviour, ISpellReferencer
    {
        public UnityEvent<Creature> OnWeaponClashCreature = new UnityEvent<Creature>();
        public UnityEvent<CreatureReference> OnWeaponClashCreatureReference = new UnityEvent<CreatureReference>();

        [SerializeField]
        bool addListenerOnAwake = false;

        bool effectRemoved = false;
        public PhysicalSpell physicalSpell { get; set; }

        private void Start()
        {
            if (addListenerOnAwake)
                physicalSpell.spellCaster.GetComponent<CreatureReference>().OnWeaponClash.AddListener(InvokeEvent);
        }

        private void InvokeEvent(PhysicalWeapon targetWeapon)
        {
            OnWeaponClashCreature.Invoke(targetWeapon.wielder.creature);
            OnWeaponClashCreatureReference.Invoke(targetWeapon.wielder);

            RemoveOnClashListener();
        }

        public void AddOnClashListener()
        {
            if (!addListenerOnAwake)
                physicalSpell.spellCaster.GetComponent<CreatureReference>().OnWeaponClash.AddListener(InvokeEvent);
        }

        public void RemoveOnClashListener()
        {
            if (effectRemoved)
                return;

            physicalSpell.spellCaster.GetComponent<CreatureReference>().OnWeaponClash.RemoveListener(InvokeEvent);

            effectRemoved = true;
        }
    }
}
