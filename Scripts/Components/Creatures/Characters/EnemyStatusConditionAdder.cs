using com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using UnityEngine.AI;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public class EnemyStatusConditionAdder : StatusConditionAdder
    {
        protected override void HandleStatusConditionAdded(StatusCondition statusCondition)
        {
            base.HandleStatusConditionAdded(statusCondition);

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.KnockedBack)
            {
                CharacterKnockedBack characterKnockedBack = gameObject.AddComponent<CharacterKnockedBack>();
                characterKnockedBack.knockedBackCondition = statusCondition as KnockedBack;
            }

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Maddened)
            {
                CharacterMaddened enemyMaddened = gameObject.AddComponent<CharacterMaddened>();
            }

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Feared)
            {
                CharacterFeared characterFeared = gameObject.AddComponent<CharacterFeared>();
            }

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Restrained)
            {
                CharacterRestrained characterRestrained = gameObject.AddComponent<CharacterRestrained>();
            }

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Immobilized)
            {
                creatureBehaviour.GetComponent<NavMeshAgent>().enabled = false;
            }
        }

        protected override void RemoveConditions()
        {
            base.RemoveConditions();

            statusConditionsToRemove.ForEach((statusCondition) =>
            {
                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.KnockedBack)
                {
                    if (gameObject.TryGetComponent(out CharacterKnockedBack characterKnockedBack))
                        Destroy(characterKnockedBack);
                }

                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Maddened)
                {
                    if (gameObject.TryGetComponent(out CharacterMaddened enemyMaddened))
                        Destroy(enemyMaddened);
                }

                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Feared)
                {
                    if (gameObject.TryGetComponent(out CharacterFeared characterFeared))
                        Destroy(characterFeared);
                }

                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Restrained)
                {
                    if (gameObject.TryGetComponent(out CharacterRestrained characterRestrained))
                        Destroy(characterRestrained);
                }

                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Immobilized)
                {
                    creatureBehaviour.GetComponent<NavMeshAgent>().enabled = true;
                }
            });

            statusConditionsToRemove.Clear();
        }
    }
}