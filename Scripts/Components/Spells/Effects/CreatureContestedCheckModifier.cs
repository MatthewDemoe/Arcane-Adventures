using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class CreatureContestedCheckModifier : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        int contestedCheckModifier = 0;

        public PhysicalSpell physicalSpell { get; set; }

        CreatureEffect creatureEffect;

        private void Start()
        {
            creatureEffect = new CreatureEffect(
                name: "Contested Check Bonus",
                description: $"Gain a bonus of {contestedCheckModifier} on all contested checks.",
                source: physicalSpell.correspondingSpell.name,
                contestedCheckBonus: contestedCheckModifier);
        }

        public void ModifyContestedCheck(CreatureReference creatureReference)
        {
            creatureReference.creature.modifiers.AddEffect(creatureEffect);
        }

        public void ResetContestedCheck(CreatureReference creatureReference)
        {
            creatureReference.creature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}