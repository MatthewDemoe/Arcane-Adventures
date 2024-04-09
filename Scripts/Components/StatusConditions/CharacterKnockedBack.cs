using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Conditions;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.StatusConditions
{
    public class CharacterKnockedBack : MonoBehaviour
    {
        CapsuleCollider knockbackCollider;
        CapsuleCollider creatureCollider;

        Creature affectedCreature;

        public KnockedBack knockedBackCondition { private get; set; }

        Rigidbody creatureRigidbody;

        private const float velocityCutoff = 2f;

        void Start()
        {
            knockbackCollider = gameObject.AddComponent<CapsuleCollider>();
            creatureCollider = transform.parent.GetComponent<CapsuleCollider>();
            creatureRigidbody = creatureCollider.GetComponent<Rigidbody>();

            affectedCreature = creatureCollider.GetComponent<CreatureReference>().creature;

            knockbackCollider.isTrigger = true;

            knockbackCollider.center = creatureCollider.center;

            knockbackCollider.height = creatureCollider.height * 0.75f;
            knockbackCollider.radius = creatureCollider.radius * 1.25f;
            knockbackCollider.direction = creatureCollider.direction;

            DisableCollisionWithChildren();

            StartCoroutine(CheckVelocity());
        }

        private void OnTriggerEnter(Collider other)
        {
            knockedBackCondition.OnObjectCollision();
        }

        private void OnDestroy()
        {
            Destroy(knockbackCollider);
        }

        IEnumerator CheckVelocity()
        {
            yield return null;

            while (affectedCreature.statusConditionTracker.HasStatusCondition(AllStatusConditions.StatusConditionName.KnockedBack))
            {
                if (creatureRigidbody.velocity.magnitude < velocityCutoff)
                    affectedCreature.statusConditionTracker.RemoveStatusCondition(AllStatusConditions.StatusConditionName.KnockedBack);

                yield return new WaitForFixedUpdate();
            }
        }

        private void DisableCollisionWithChildren()
        {
            creatureCollider.GetComponentsInChildren<Collider>().ToList().ForEach((collider) => Physics.IgnoreCollision(collider, knockbackCollider));
        }
    }
}