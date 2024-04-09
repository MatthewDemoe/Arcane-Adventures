using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class CreatureDamageTakenModifier : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        float damageTakenMultiplier = 1.0f;

        CreatureEffect creatureEffect;

        private PhysicalSpell _physicalSpell;
        public PhysicalSpell physicalSpell 
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;

                creatureEffect = new CreatureEffect(
                    name: "Damage Taken Modifier",
                    description: $"Modify damage taken by {damageTakenMultiplier}.",
                    source: physicalSpell.correspondingSpell.name,
                    damageTaken: damageTakenMultiplier
                );
            }
        }

        public void MultiplyDamageTaken(CreatureReference creatureReference)
        {
            creatureReference.creature.modifiers.AddEffect(creatureEffect);
        }

        public void ResetDamageTaken(CreatureReference creatureReference)
        {
            creatureReference.creature.modifiers.RemoveEffect(creatureEffect);
        }
    }
}