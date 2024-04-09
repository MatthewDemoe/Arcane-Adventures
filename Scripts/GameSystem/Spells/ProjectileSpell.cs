using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells
{
    public abstract class ProjectileSpell : Spell
    {
        protected ProjectileSpell() : base() { }
        protected ProjectileSpell(ref Creature _caster) : base(ref _caster) { }
    }
}