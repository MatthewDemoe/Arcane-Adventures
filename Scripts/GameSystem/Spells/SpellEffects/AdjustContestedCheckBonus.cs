using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.SpellEffects
{
    public class AdjustContestedCheckBonus : SpellEffect
    {
        int contestedCheckBonus = 0;
        CreatureEffect creatureEffect;

        public AdjustContestedCheckBonus(ref Creature caster, int amount) : base(ref caster)
        {
            contestedCheckBonus = amount;
        }

        public override void OnStartCast(ref Spell spell)
        {
            base.OnStartCast(ref spell);

            creatureEffect = new CreatureEffect(
                name: "Contested Check Bonus",
                description: $"Gain a bonus of {contestedCheckBonus} on all contested checks.",
                source: spell.name,
                contestedCheckBonus: contestedCheckBonus
                ); ;

            caster.modifiers.AddEffect(creatureEffect);
        }

        public override void OnDurationEnd()
        {
            base.OnDurationEnd();

            caster.modifiers.RemoveEffect(creatureEffect);
        }
    }
}