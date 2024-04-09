using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Traits;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters
{
    public abstract class Monster : Creature
    {

        protected List<Trait> traits = new List<Trait>();
        public Monster(Stats stats, Identifiers.Race race) : base(stats, race) {}
    }
}
