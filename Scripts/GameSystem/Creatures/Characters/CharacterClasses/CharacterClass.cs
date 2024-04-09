using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters.CharacterClasses
{
    public abstract class CharacterClass 
    {
        public abstract List<Trait> traits { get; }
        public abstract List<Spell> spells { get; }
    }
}