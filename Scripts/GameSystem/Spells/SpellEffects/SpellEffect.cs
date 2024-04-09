using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class SpellEffect 
    {
        protected Creature caster;
        protected Character character => caster as Character;


        public SpellEffect(ref Creature _caster)
        {
            caster = _caster;
        }

        public virtual void OnStartCast(ref Spell spell)
        {

        }

        public virtual void OnChannel()
        {

        }

        public virtual void OnUpkeep()
        {

        }

        public virtual void OnCastEnd()
        {

        }

        public virtual void OnDurationEnd()
        {

        }
    }
}
