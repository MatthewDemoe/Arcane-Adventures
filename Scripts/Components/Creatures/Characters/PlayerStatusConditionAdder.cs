using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions.Player;
using com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Player;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Development;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters
{
    public class PlayerStatusConditionAdder : StatusConditionAdder
    {
        bool shouldShowGhostHands => CombatSettings.Controller.showGhostHandsWhenStunned && 
            creatureReference.HasOneOfStatusCondition(new List<AllStatusConditions.StatusConditionName>()
            { 
                AllStatusConditions.StatusConditionName.Stunned, 
                AllStatusConditions.StatusConditionName.Immobilized,
                AllStatusConditions.StatusConditionName.Dazed,
                AllStatusConditions.StatusConditionName.Maddened,
                AllStatusConditions.StatusConditionName.Staggered,
            });

        bool isPlayerDisoriented => creatureReference.HasOneOfStatusCondition(new List<AllStatusConditions.StatusConditionName>()
            {
                AllStatusConditions.StatusConditionName.Stunned,
                AllStatusConditions.StatusConditionName.Dazed,
                AllStatusConditions.StatusConditionName.Staggered,
            });

        protected override void HandleStatusConditionAdded(StatusCondition statusCondition)
        {
            base.HandleStatusConditionAdded(statusCondition);

            GhostHand.Set(on: shouldShowGhostHands);

            if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Feared)
                gameObject.AddComponent<PlayerCharacterFeared>();

            else if (isPlayerDisoriented)
            {
                ControllerLink.Left.StartDelayingHandMovements();
                ControllerLink.Right.StartDelayingHandMovements();
            
                if(!gameObject.TryGetComponent(out PlayerCharacterDisoriented playerCharacterDisoriented))
                    gameObject.AddComponent<PlayerCharacterDisoriented>();
            }   

            else if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Muted)
            {
                creatureReference.GetComponent<PlayerSpellCaster>().CancelSpells();
            }

            else if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Maddened)
            {
                gameObject.AddComponent<CharacterMaddened>();
                (creatureBehaviour as PlayerCharacterBehaviour).SetBehaviourTreeActive(true);
            }
        }

        protected override void RemoveConditions()
        {
            base.RemoveConditions();

            statusConditionsToRemove.ForEach((statusCondition) =>
            {
                if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Feared)
                {
                    if (TryGetComponent(out PlayerCharacterFeared playerFeared))
                    {
                        Destroy(playerFeared);
                    }
                }

                else if (statusCondition.statusConditionName == AllStatusConditions.StatusConditionName.Maddened)
                {
                    if (TryGetComponent(out CharacterMaddened playerMaddened))
                    {
                        (creatureBehaviour as PlayerCharacterBehaviour).SetBehaviourTreeActive(false);
                        Destroy(playerMaddened);
                    }
                }
            });

            if (!isPlayerDisoriented)
            {
                if (TryGetComponent(out PlayerCharacterDisoriented playerDisoriented))
                {
                    Destroy(playerDisoriented);
                }
            }

            GhostHand.Set(on: shouldShowGhostHands);

            statusConditionsToRemove.Clear();
        }
    }
}