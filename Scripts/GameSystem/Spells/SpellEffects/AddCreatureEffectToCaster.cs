using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class AddCreatureEffectToCaster : SpellEffect
    {
        protected CreatureEffect effect;

        protected Spell spell;
        protected string effectDescription;

        public AddCreatureEffectToCaster(ref Creature caster, CreatureEffect effect) : base(ref caster)
        {
            this.effect = effect;            
        }

        protected virtual void UpdateCreatureEffect()
        {            
            caster.modifiers.AddEffect(effect);
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);
            this.spell = spell;

            UpdateCreatureEffect();
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.modifiers.RemoveEffect(effect);
        }
    }
}