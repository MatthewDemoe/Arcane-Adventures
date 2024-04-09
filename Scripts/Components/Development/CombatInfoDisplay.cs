using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using System.Linq;
using TMPro;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class CombatInfoDisplay : MonoBehaviour
    {
        private const int cyclesToSkip = 20;

        [SerializeField] private TextMeshProUGUI body;
        private int cyclesLeftToSkip = cyclesToSkip;
        private PlayerCharacterReference playerCharacterReference;
        private ICombatSystem combatSystem;
        private bool dependenciesCached = false;

        private void Update()
        {
            if (!dependenciesCached)
                if (TryCacheDependencies())
                    dependenciesCached = true;
                else return;

            if (cyclesLeftToSkip == 0)
            {
                body.text = GetBodyText();
                cyclesLeftToSkip = cyclesToSkip;
            }
            else cyclesLeftToSkip--;
        }

        private bool TryCacheDependencies()
        {
            return InjectorContainer.Injector.TryGetInstance(out playerCharacterReference) &&
                InjectorContainer.Injector.TryGetInstance(out combatSystem);//TODO: Make "Inject" version of try.
        }

        private string GetBodyText()
        {
            var playerEffects = playerCharacterReference.creature.modifiers.effects.Any() ?
                string.Join(" / ", playerCharacterReference.creature.modifiers.effects.Select(effect => $"{effect.name} ({effect.description})")) :
                "-";
            var combatLog = string.Join("\n", combatSystem.combatLog);

            return $"Player Effects: {playerEffects}\n\n{combatLog}";
        }
    }
}