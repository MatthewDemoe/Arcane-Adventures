using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Creatures;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Missions
{
    public class TutorialDungeonMaster : MonoBehaviour
    {
        private void Start()
        {
            var tutorialFairy = DefaultCreatureResolver.GetDefaultCreature<TutorialFairy>();
            CreatureBuilder.Build(tutorialFairy, new Vector3(60, 10, 41.5f), Quaternion.Euler(0, 0, 0));
        }
    }
}