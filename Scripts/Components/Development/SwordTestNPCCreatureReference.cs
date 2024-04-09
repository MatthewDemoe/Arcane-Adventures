using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using OrcRaider = com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters.OrcRaider;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class SwordTestNPCCreatureReference : CreatureReference
    {
        protected override Type creatureType => typeof(OrcRaider);
    }
}