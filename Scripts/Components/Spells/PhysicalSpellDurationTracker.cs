using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    public class PhysicalSpellDurationTracker : MonoBehaviour, ISpellReferencer
    {
        public PhysicalSpell physicalSpell { get; set; }

        public void StartDuration(PhysicalSpell physicalSpell)
        {
            this.physicalSpell = physicalSpell;

            StartCoroutine(StartUpdatingSpellDuration());
        }

        IEnumerator StartUpdatingSpellDuration()
        {
            while (physicalSpell.correspondingSpell.IsSpellActive(Time.deltaTime))
            {
                yield return null;
            }
            
            physicalSpell.OnDurationEnd.Invoke();
        }
    }
}
