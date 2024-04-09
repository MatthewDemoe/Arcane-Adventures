using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellButtonCooldownTimer : MonoBehaviour
    {
        [SerializeField]
        Image cooldownImage;

        [SerializeField]
        TextMeshProUGUI cooldownText;

        Spell spellToTrack;

        SpellCooldownTracker cooldownTracker;

        bool currentlyOnCooldown = false;

        public void InitializeWithSpell(Spell spell)
        {
            spellToTrack = spell;
            cooldownTracker = PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().spellCooldownTracker;

            SetCooldownUI();
        }

        void Update()
        {
            currentlyOnCooldown = cooldownTracker.IsOnCooldownBySpell(spellToTrack);

            ToggleUI();
            SetCooldownUI();
        }

        private void SetCooldownUI()
        {
            cooldownImage.fillAmount = cooldownTracker.RemainingSpellCooldownTime(spellToTrack, normalized: true);
            cooldownText.text = ((int)cooldownTracker.RemainingSpellCooldownTime(spellToTrack, normalized: false)).ToString();
        }

        private void ToggleUI()
        {
            cooldownText.enabled = currentlyOnCooldown;
        }
    }
}