using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    [RequireComponent(typeof(Button))]
    public class SpellButton : EventTrigger
    {
        Spell spell;

        public void Initialize(Spell spell)
        {
            this.spell = spell;
        }

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

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            HandSide handSide = (HandSide)(eventData.pointerId == 1 ? 2 : 1);

            //Need to invert the value because eventData.pointerId is inverted when running in Android vs. Editor
#if UNITY_EDITOR
            handSide = (HandSide)eventData.pointerId;
#endif

            PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().TryEquipSpell(spell, handSide);
        }
    }
}