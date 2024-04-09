using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using System.Collections.Generic;
using System.Linq;
using Injection;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class CreatureTracker : IInjectable
    {
        List<CreatureBehaviour> creaturesInScene = new List<CreatureBehaviour>();

        public void TrackCreature(CreatureBehaviour creatureBehaviour)
        {
            if (!creaturesInScene.Contains(creatureBehaviour))
                creaturesInScene.Add(creatureBehaviour);
        }

        public void StopTrackingCreature(CreatureBehaviour creatureBehaviour)
        {
            if (creaturesInScene.Contains(creatureBehaviour))
                creaturesInScene.Remove(creatureBehaviour);
        }

        public List<CreatureBehaviour> GetAllCreaturesInScene()
        {
            return creaturesInScene;
        }

        public List<CreatureBehaviour> GetCreaturesOnTeam(CreatureBehaviour.Team team)
        {
            return creaturesInScene.Where((creatureBehaviour) => creatureBehaviour.team == team).ToList();
        }

        public CreatureReference GetLowestHealthCreatureOnTeam(CreatureBehaviour.Team team)
        {
            if (!creaturesInScene.Any())
                return null;

            return creaturesInScene.Where(creatureBehaviour => creatureBehaviour.team == team)
                .OrderByDescending(creatureBehaviour => creatureBehaviour.creatureReference.creature.stats.currentHealthPercent)
                .First().creatureReference;
        }
    }
}