using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions
{
    public class CharacterMaddened : MonoBehaviour
    {
        [SerializeField]
        List<GameObject> creaturesInRange = new List<GameObject>();

        CreatureReference thisCreature;

        GameObject closestCreature = null;

        SphereCollider maddenedCollider;
        Animator animator;

        void Start()
        {
            maddenedCollider = gameObject.AddComponent<SphereCollider>();
            thisCreature = GetComponentInParent<CreatureReference>();

            if (thisCreature is PlayerCharacterReference)
                (thisCreature as PlayerCharacterReference).avatarAnimator.SetBool(CharacterAnimatorParameters.Maddened, true);

            maddenedCollider.isTrigger = true;
            maddenedCollider.radius = 10.0f;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out CreatureReference creatureInRange))
                creatureInRange = other.gameObject.GetComponentInParent<CreatureReference>();

            if (creatureInRange == null || creatureInRange == thisCreature || creaturesInRange.Contains(creatureInRange.gameObject))
                return;

            creaturesInRange.Add(creatureInRange.gameObject);

            if(thisCreature.TryGetComponent(out CreatureBehaviour creatureBehaviour))
                creatureBehaviour.SetTarget(GetClosestCreature());
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent(out CreatureReference creatureInRange))
                creatureInRange = other.gameObject.GetComponentInParent<CreatureReference>();

            if (creatureInRange == null || !creaturesInRange.Contains(creatureInRange.gameObject))
                return;

            creaturesInRange.Remove(creatureInRange.gameObject);
        }

        private void OnDestroy()
        {
            Destroy(maddenedCollider);

            CreatureBehaviour creatureBehaviour = thisCreature.GetComponent<CreatureBehaviour>();

            if (!(thisCreature is PlayerCharacterReference))
            {
                creatureBehaviour.SetTarget(PlayerCharacterReference.Instance.transform);
            }

            else
            {
                (thisCreature as PlayerCharacterReference).avatarAnimator.SetBool(CharacterAnimatorParameters.Maddened, false);
                creatureBehaviour.SetTarget(null);
            }
        }

        public Transform GetClosestCreature()
        {
            creaturesInRange.ForEach((gameObject) =>
            {
                if (closestCreature == null || Vector3.Distance(transform.position, gameObject.transform.position) < Vector3.Distance(transform.position, closestCreature.transform.position))
                    closestCreature = gameObject;
            });

            return closestCreature.transform;
        }
    }
}