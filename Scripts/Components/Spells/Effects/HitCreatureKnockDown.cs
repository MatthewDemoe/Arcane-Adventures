using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using System.Collections;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class HitCreatureKnockDown : MonoBehaviour
    {
        Animator animator;
        CreatureReference hitCreature;

        [SerializeField]
        float duration = 3.0f;

        private void KnockDownForDuration(float duration)
        {
            StartCoroutine(KnockDownRoutine(duration));
        }

        IEnumerator KnockDownRoutine(float duration)
        {
            animator.SetTrigger(CharacterAnimatorParameters.KnockedDown);
            hitCreature.creature.isMovementEnabled = false;

            yield return new WaitForSeconds(duration);

            animator.SetTrigger(CharacterAnimatorParameters.StandingUp);

            yield return new WaitUntil(() => animator.GetBool(CharacterAnimatorParameters.IsKnockedDown) == false);

            hitCreature.creature.isMovementEnabled = true;
        }

        public void KnockDownHitCreature(CreatureReference hitCreatureReference)
        {
            hitCreature = hitCreatureReference;
            animator = hitCreatureReference.GetComponent<Animator>();

            KnockDownForDuration(duration);
        }
    }
}