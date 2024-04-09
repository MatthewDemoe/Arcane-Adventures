using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    [RequireComponent(typeof(SelfTargetedSpellEvents))]
    public class PhysicalSelfTargetedSpell : PhysicalSpell
    {
        [SerializeField]
        bool drawAreaOfEffect = false;

        SpellAreaTargeter spellAreaTargeter;

        public override void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {
            base.InitializeSpellInformation(spell, playerSpellCaster, handSide);

            if (!drawAreaOfEffect)
                return;
            
            spellAreaTargeter = gameObject.AddComponent<SpellAreaTargeter>();            
            spellAreaTargeter.InitializeWithSpellInformation(spell, spellCaster, handSide);    
            spellSource = playerSpellCaster.GetComponentInChildren<SpellEffectAnchor>().transform;
        }

        private void OnDestroy()
        {
            if (!drawAreaOfEffect)
                return;

            Destroy(spellAreaTargeter);
        }
    }
}