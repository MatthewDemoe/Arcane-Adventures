using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public static class CreatureBuilder
    {
        public static GameObject Build(PlayerCharacter playerCharacter)
        {

            var position = PlayerCharacterReference.Instance == null ?
                Vector3.zero :
                PlayerCharacterReference.Instance.transform.position;

            playerCharacter.SetUnwritable();

            return Build(playerCharacter, position);
        }

        public static IEnumerator GetAsyncBuildCoroutine(PlayerCharacter playerCharacter)
        {
            var position = PlayerCharacterReference.Instance is null ?
                Vector3.zero :
                PlayerCharacterReference.Instance.transform.position;

            var resource = Prefabs.LoadAsync(playerCharacter);
            resource.completed += (asyncOperation) => Instantiate(resource.asset, playerCharacter, position, Quaternion.identity);

            yield return null;
        }

        public static GameObject Build(Creature creature, Vector3 position) => Build(creature, position, Quaternion.Euler(0, 0, 0));

        public static GameObject Build(Creature creature, Vector3 position, Quaternion rotation)
        {
            var prefab = Prefabs.Load(creature);
            GameObject instance = Object.Instantiate(prefab, position, rotation);

            CreatureReference creatureReference = instance.GetComponent<CreatureReference>();
            creatureReference.creature = creature;

            return instance;
        }

        private static void Instantiate(Object prefab, Creature creature, Vector3 position, Quaternion rotation)
        {
            GameObject instance = Object.Instantiate(prefab, position, rotation) as GameObject;
            instance.GetComponentInChildren<CreatureReference>().creature = creature;
        }
    }
}