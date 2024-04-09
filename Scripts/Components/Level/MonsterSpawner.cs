using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Monsters;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Level
{
    [RequireComponent(typeof(Collider))]
    public class MonsterSpawner : MonoBehaviour
    {
        //TODO: Take monster type from serialized field.

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject == PlayerCharacterReference.Instance.gameObject)
            {
                var monster = DefaultCreatureResolver.GetDefaultCreature(typeof(OrcRaider));
                Creatures.CreatureBuilder.Build(monster, transform.position);
                UnityEngine.Debug.Log($"Spawning monster ...");//TODO: Rename debug namespace to avoid having to specify UnityEngine.
                Destroy(this.gameObject);
            }
        }
    }
}