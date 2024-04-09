using System.Collections;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat
{
    public class SpellSurface : AttackSurface
    {
        public Creature caster;
        public List<Damage> damage;
    }
}
