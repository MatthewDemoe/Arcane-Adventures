using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting
{
    public class EnemySingleTargeter : SpellSingleTargeter
    {
        public override Vector3 targetDirection => (enemyBehaviour.GetTargetPosition - transform.position).normalized;
        public override Vector3 targetPoint => enemyBehaviour.target.transform.position;

        public override GameObject targetGameObject => enemyBehaviour.target.gameObject;
        public override CreatureReference targetedCreatureReference => targetGameObject.GetComponent<CreatureReference>();

        EnemyBehaviour enemyBehaviour;

        public override void InitializeWithSpellInformation(Spell spell, SpellCaster spellCaster, HandSide handSide)
        {
            base.InitializeWithSpellInformation(spell, spellCaster, handSide);

            enemyBehaviour = spellCaster.GetComponent<EnemyBehaviour>();
        }
    }
}