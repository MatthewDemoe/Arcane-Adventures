using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using UnityEngine.EventSystems;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class SpellCastDisableEventTrigger : EventTrigger
    {
        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);

            PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().OnHoverUI.Invoke(true);
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);

            PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().OnHoverUI.Invoke(false);
        }
    }
}