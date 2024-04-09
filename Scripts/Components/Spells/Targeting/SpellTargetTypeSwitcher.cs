using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using System;
using System.Linq;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class SpellTargetTypeSwitcher 
    {
        SpellCaster spellCaster;
        public Dictionary<HandSide, SpellTargeter> spellTargeterByHandSide { get; private set; } = new Dictionary<HandSide, SpellTargeter>() { { HandSide.Left, null }, { HandSide.Right, null } };

        public SpellTargetTypeSwitcher(SpellCaster spellCaster)
        {
            this.spellCaster = spellCaster;
            this.spellCaster.OnSpellCreated.AddListener(SetNewTargetType);
        }

        private void SetNewTargetType(PhysicalSpell newSpell, HandSide handSide)
        {
            if (spellTargeterByHandSide[handSide] != null)
                DestroyCurrentTargeter(handSide);

            Type componentToAdd = spellCaster is EnemySpellCaster ? newSpell.enemySpellTargeter : newSpell.playerSpellTargeter;

            spellTargeterByHandSide[handSide] = newSpell.spellSource.gameObject.AddComponent(componentToAdd) as SpellTargeter;

            newSpell.OnEndCast.AddListener(() => DestroyCurrentTargeter(handSide));
            spellTargeterByHandSide[handSide].InitializeWithSpellInformation(newSpell.correspondingSpell, spellCaster, handSide);
        }

        public void DestroyCurrentTargeter(HandSide handSide)
        {
            UnityEngine.Object.Destroy(spellTargeterByHandSide[handSide]);
        }

        public void DestroyAllTargeters()
        {
            spellTargeterByHandSide.Keys.ToList().ForEach(handSide => DestroyCurrentTargeter(handSide));
        }
    }
}