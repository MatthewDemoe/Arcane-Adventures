using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;
using System.Collections.Generic;


namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class SpellCooldownTracker 
    {
        List<Spell> playerSpells = new List<Spell>();
        List<float> timeSinceSpellCast = new List<float>();

        public void InitializeSpellList(List<Spell> spells)
        {
            playerSpells = spells;

            timeSinceSpellCast.Clear();

            foreach (Spell spell in playerSpells)
            {
                timeSinceSpellCast.Add(spell.cooldown);
            }
        }

        public void ResetCooldown(Spell spell)
        {
            int index = playerSpells.IndexOf(spell);

            timeSinceSpellCast[index] = 0.0f;
        }

        public bool IsOnCooldownBySpell(Spell spell)
        {
            int index = playerSpells.IndexOf(spell);

            return timeSinceSpellCast[index] <= playerSpells[index].cooldown;
        }

        public float RemainingSpellCooldownTime(Spell spell, bool normalized)
        {
            int index = playerSpells.IndexOf(spell);            

            float remainingCooldown = playerSpells[index].cooldown - timeSinceSpellCast[index];

            return remainingCooldown / (normalized ? playerSpells[index].cooldown : 1.0f);
        }

        public void UpdateCooldowns()
        {
            for (int i = 0; i < playerSpells.Count; i++)
            {
                if (timeSinceSpellCast[i] <= playerSpells[i].cooldown)
                    timeSinceSpellCast[i] += Time.deltaTime;
            }
        }
    }
}
