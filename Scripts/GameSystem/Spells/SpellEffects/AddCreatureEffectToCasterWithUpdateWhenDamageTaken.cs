using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class AddCreatureEffectToCasterWithUpdateWhenDamageTaken : AddCreatureEffectToCaster
    {
        public delegate CreatureEffect CreateCreatureEffect();
        CreateCreatureEffect createCreatureEffect;

        public AddCreatureEffectToCasterWithUpdateWhenDamageTaken(ref Creature caster, CreateCreatureEffect createCreatureEffect) : base(ref caster, createCreatureEffect.Invoke())
        {
            this.createCreatureEffect = createCreatureEffect;
        }

        protected override void UpdateCreatureEffect()
        {
            caster.modifiers.RemoveEffect(effect);

            effect = createCreatureEffect.Invoke();

            caster.modifiers.AddEffect(effect);
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);
            caster.modifiers.AddEffect(effect);

            caster.OnDamageTaken += UpdateCreatureEffect;
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.OnDamageTaken -= UpdateCreatureEffect;
        }
    }
}