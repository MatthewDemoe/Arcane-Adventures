using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using UnityEngine;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class SpellDurationScaler : MonoBehaviour
    {
        Spell thisSpell = null;

        void Start()
        {
            Initialize();   
        }

        public void ScaleSpellDuration(float amount)
        {
            if (thisSpell is null)
                Initialize();

            thisSpell.durationMultiplier *= amount;
        }

        private void Initialize()
        {
            if (TryGetComponent(out SpellReference spellReference))
                thisSpell = spellReference.spell;

            else if (TryGetComponent(out PhysicalSpell physicalSpell))
                thisSpell = physicalSpell.correspondingSpell;
        }
    }
}