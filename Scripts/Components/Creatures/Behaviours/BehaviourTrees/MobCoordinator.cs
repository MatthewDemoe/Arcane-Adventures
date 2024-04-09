using com.AlteredRealityLabs.ArcaneAdventures.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using System.Collections.Generic;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Behaviours.BehaviourTrees
{
    public class MobCoordinator : MonoBehaviour
    {
        List<CreatureBehaviour> creatures = new List<CreatureBehaviour>();
        List<CreatureBehaviour> assignedCreatures = new List<CreatureBehaviour>();
        List<CreatureBehaviour> unassignedCreatures = new List<CreatureBehaviour>();

        [SerializeField]
        public List<Vector3> patrolPoints = new List<Vector3>();

        [SerializeField]
        public List<Vector3> spawnPoints = new List<Vector3>();

        [SerializeField]
        public List<CreatureType> creatureTypes = new List<CreatureType>();

        private bool IsAssigned(CreatureBehaviour.Role role) => assignedCreatures.Exists(creature => creature.role == role);

        private void Start()
        {
            CreateCreatures();
            AssignNonCombatRoles();
        }

        private void CreateCreatures()
        {
            int creatureCounter = 0;

            spawnPoints.ForEach((spawnPoint) => 
            {
                int nextCreatureType = creatureCounter > creatureTypes.Count ? Random.Range(0, creatureTypes.Count) : creatureCounter;
                creatureCounter++;

                NavMesh.SamplePosition(spawnPoint, out NavMeshHit hit, 100, NavMesh.AllAreas);

                //TODO: Scale enemy with player level
                GameObject newCreature = CreatureBuilder.Build(DefaultCreatureResolver.GetDefaultCreature(CreatureTypes.ByIdentifier[creatureTypes[nextCreatureType]]), hit.position);
                AddCreature(newCreature.GetComponent<CreatureBehaviour>());
            });

            unassignedCreatures.AddRange(creatures);
        }

        public void AddCreature(CreatureBehaviour creatureBehaviour)
        {
            creatures.Add(creatureBehaviour);
            creatureBehaviour.mob = this;
            creatureBehaviour.GetComponent<CreatureReference>().OnDeath.AddListener(() => RemoveCreature(creatureBehaviour));
        }

        public void RemoveCreature(CreatureBehaviour creatureBehaviour)
        {
            if (!creatures.Contains(creatureBehaviour))
                return;

            creatures.Remove(creatureBehaviour);
            AssignCombatRoles();
        }

        public void AlertMob(Transform target, CreatureBehaviour alertingCreature)
        {
            creatures.ForEach((creatureBehaviour) => 
            {
                creatureBehaviour.SetTarget(target);
                Destroy(creatureBehaviour.GetComponent<EnemyAlertTrigger>());
            });

            AssignRole(alertingCreature, alertingCreature.preferredRole);
            AssignCombatRoles();
        }

        public void AssignNonCombatRoles()
        {
            if (!IsAssigned(CreatureBehaviour.Role.Patrol) && creatures.Any())
                creatures[0].role = CreatureBehaviour.Role.Patrol;

            creatures.ForEach(creatureBehaviour =>
            {
                if (creatureBehaviour.role != CreatureBehaviour.Role.Patrol)
                    creatureBehaviour.role = CreatureBehaviour.Role.Resting;
            });
        }

        //TODO: Add more roles, decide which creature gets which role based on which are applicable 
        public void AssignCombatRoles()
        {
            SoftAssignRole(CreatureBehaviour.Role.PrimaryAttacker);
            SoftAssignRole(CreatureBehaviour.Role.PrimaryRangedAttacker);
            FillRemainingRoles();
        }

        public void ForceRoleAndReshuffle(CreatureBehaviour creatureBehaviour, CreatureBehaviour.Role role)
        {
            AssignRole(creatureBehaviour, role);

            AssignCombatRoles();
        }

        private void AssignRole(CreatureBehaviour creatureBehaviour, CreatureBehaviour.Role role)
        {
            creatureBehaviour.role = role;

            assignedCreatures.Add(creatureBehaviour);
            unassignedCreatures.Remove(creatureBehaviour);
        }

        private void SoftAssignRole(CreatureBehaviour.Role role)
        {
            if (IsAssigned(role))
                return;

            creatures.ForEach(creatureBehaviour =>
            {
                if (creatureBehaviour.preferredRole == role && unassignedCreatures.Contains(creatureBehaviour))
                {
                    AssignRole(creatureBehaviour, role);
                    return;
                }
            });
        }

        private void FillRemainingRoles()
        {
            creatures.ForEach(creatureBehaviour => 
            {
                if(unassignedCreatures.Contains(creatureBehaviour))
                    AssignRole(creatureBehaviour, creatureBehaviour.alternativeRoles[Random.Range(0, creatureBehaviour.alternativeRoles.Count)]);
            });

            ResetCreatureLists();
        }

        private void ResetCreatureLists()
        {
            unassignedCreatures.Clear();
            assignedCreatures.Clear();
            unassignedCreatures.AddRange(creatures);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;

            for (int i = 0; i < patrolPoints.Count; i++)
            {
                Gizmos.DrawSphere(patrolPoints[i], 0.5f);
                Gizmos.DrawLine(patrolPoints[i], patrolPoints[(i + 1) % patrolPoints.Count]);
            }

            Gizmos.color = Color.red;

            for (int i = 0; i < spawnPoints.Count; i++)
            {
                Gizmos.DrawSphere(spawnPoints[i], 0.5f);
            }
        }
    }
}