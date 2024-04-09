using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using RootMotion.Dynamics;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class SwordTestNPCCoordinator : MonoBehaviour
    {
        private const float TimeToRecover = 5f;
        private const float TargetPinWeight = 0.4f;
        private const float DropDelay = 0.25f;
        private const float AvoidanceEndDelay = 0.25f;

        [SerializeField] private CombatDemoSettings combatDemoSettings;
        [SerializeField] private Rigidbody knockbackRigidbody;
        [SerializeField] private Rigidbody hat;
        [SerializeField] private float weaponAvoidanceMovementSpeed;

        private PuppetMaster puppetMaster;
        private BehaviourPuppet behaviourPuppet;
        private SwordTestNPCItemEquipper swordTestNPCItemEquipper;
        private float timeLeftToRecover;
        private Transform lastLimitedRagdollTarget;
        private ConfigurableJoint baseConfigurableJoint;
        private bool droppedWeapon = false;
        private bool dropWeaponAfterDelay = false;
        private bool droppedHat = false;
        private bool dropHatAfterDelay = false;
        private SwordTestNPCCreatureController creatureController;
        private float lastWeaponAvoidanceReport;
        
        private CombatDemoSettings.RagdollOnBodyStrikeBehavior ragdollOnBodyStrikeBehavior => combatDemoSettings.ragdollOnBodyStrikeBehavior;

        private void Awake()
        {
            puppetMaster = GetComponentInChildren<PuppetMaster>();
            behaviourPuppet = GetComponentInChildren<BehaviourPuppet>();
            swordTestNPCItemEquipper = GetComponentInChildren<SwordTestNPCItemEquipper>();
            baseConfigurableJoint = knockbackRigidbody.GetComponent<ConfigurableJoint>();
            creatureController = GetComponentInChildren<SwordTestNPCCreatureController>();
        }

        private void Start()
        {
            puppetMaster.pinWeight = TargetPinWeight;
        }
        
        public void ReportHit(Vector3 strikeDirection, ConfigurableJoint joint)
        {
            if (timeLeftToRecover > 0)
                return;

            UpdateRagdollState(joint);

            var force = strikeDirection * combatDemoSettings.knockbackForce;
            knockbackRigidbody.AddForce(force, ForceMode.VelocityChange);
        }

        public void EnableWeaponAvoidance(Vector3 direction)
        {
            creatureController.targetPosition = direction * weaponAvoidanceMovementSpeed;
            lastWeaponAvoidanceReport = Time.time;
        }
        
        private void UpdateRagdollState(ConfigurableJoint joint)
        {
            if (ragdollOnBodyStrikeBehavior.Equals(CombatDemoSettings.RagdollOnBodyStrikeBehavior.TotalRagdoll))
            {
                timeLeftToRecover = TimeToRecover;
                behaviourPuppet.SetState(BehaviourPuppet.State.Unpinned);

                if (!droppedWeapon && combatDemoSettings.dropWeaponOnRagdoll)
                    dropWeaponAfterDelay = true;
                
                if (!droppedHat && combatDemoSettings.dropHatOnRagdoll)
                    dropHatAfterDelay = true;
            }
            else if (ragdollOnBodyStrikeBehavior.Equals(CombatDemoSettings.RagdollOnBodyStrikeBehavior.LimitedRagdoll))
            {
                if (joint == baseConfigurableJoint)
                    return;

                var muscle = puppetMaster.GetMuscle(joint);
                
                timeLeftToRecover = TimeToRecover;
                var limitedRagdollTarget = puppetMaster.GetMuscle(joint).target;
                puppetMaster.SetMuscleWeightsRecursive(limitedRagdollTarget, 1, 0, 1, 1);
                lastLimitedRagdollTarget = limitedRagdollTarget;
                
                if (!droppedWeapon && combatDemoSettings.dropWeaponOnRagdoll && muscle.props.group.Equals(Muscle.Group.Hand))
                    dropWeaponAfterDelay = true;
                
                if (!droppedHat && combatDemoSettings.dropHatOnRagdoll && muscle.props.group.Equals(Muscle.Group.Head))
                    dropHatAfterDelay = true;
            }
        }
        
        private void Update()
        {
            if (timeLeftToRecover > 0)
                ProcessRecovery();
            
            if (!creatureController.targetPosition.Equals(Vector3.zero) && Time.time - lastWeaponAvoidanceReport > AvoidanceEndDelay)
                creatureController.targetPosition = Vector3.zero;
        }

        private void ProcessRecovery()
        {
            timeLeftToRecover -= Time.deltaTime;

            if (dropWeaponAfterDelay && TimeToRecover - timeLeftToRecover > DropDelay)
            {
                dropWeaponAfterDelay = false;
                
                if (swordTestNPCItemEquipper != null)
                    swordTestNPCItemEquipper.UnequipItem(HandSide.Right);
                
                droppedWeapon = true;
            }

            if (dropHatAfterDelay && TimeToRecover - timeLeftToRecover > DropDelay)
            {
                dropHatAfterDelay = false;
                hat.transform.parent = null;
                hat.isKinematic = false;
                droppedHat = true;
            }
            
            if (timeLeftToRecover <= 0)
            {
                timeLeftToRecover = 0;
                
                if (ragdollOnBodyStrikeBehavior.Equals(CombatDemoSettings.RagdollOnBodyStrikeBehavior.TotalRagdoll))
                    behaviourPuppet.SetState(BehaviourPuppet.State.GetUp);
                else if (ragdollOnBodyStrikeBehavior.Equals(CombatDemoSettings.RagdollOnBodyStrikeBehavior.LimitedRagdoll))
                    puppetMaster.SetMuscleWeightsRecursive(lastLimitedRagdollTarget, 1, 1, 1, 1);
            }
        }
    }
}