using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;
using System;
using System.Collections.Generic;
using NaughtyAttributes;

namespace com.AlteredRealityLabs.ArcaneAdventures.Identifiers
{
    public static class CreatureTypes
    {  
        [ReadOnly]
        public static Dictionary<CreatureType, Type> ByIdentifier = new Dictionary<CreatureType, Type>()
        {
            { CreatureType.Grell, typeof(Grell) },
            { CreatureType.OrcRaider, typeof(OrcRaider) },
            { CreatureType.OrcShaman, typeof(OrcShaman) },
        };
    }
}