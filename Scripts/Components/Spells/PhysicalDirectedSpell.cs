using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using System;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    //TODO: Implement :)
    public class PhysicalDirectedSpell : PhysicalSpell
    {
        public override Type playerSpellTargeter => typeof(SpellArrowTargeter);

    }
}