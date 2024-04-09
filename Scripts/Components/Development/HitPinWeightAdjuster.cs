using System;
using System.Collections.Generic;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.UnityExtensions;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class HitPinWeightAdjuster : MonoBehaviour
    {
        private const float MaxPinWeightTargetParentJumps = 0;//TODO: Unclear if we want to climb up the bones under certain circumstances, we need to either delete this if we don't, or set this depending on those circumstances.

        [SerializeField] private bool contactPromotesMovement;
        
        private SwordTestNPCCoordinator swordTestNpcCoordinator;
        private List<HitPinWeightAdjuster> hitPinWeightAdjusterParents;
        private ConfigurableJoint limitedRagdollTargetJoint;

        private void Awake()
        {
            swordTestNpcCoordinator = GetComponentInParent<SwordTestNPCCoordinator>();
            limitedRagdollTargetJoint = GetComponent<ConfigurableJoint>();
            
            var jumps = 0;
            
            while (jumps < MaxPinWeightTargetParentJumps &&
                   limitedRagdollTargetJoint.transform.parent.TryGetComponent<ConfigurableJoint>(out var parentJoint))
            {
                limitedRagdollTargetJoint = parentJoint;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (!collision.TryGetCollidingComponent<PhysicalWeapon>(out var physicalWeapon, out _))
                return;
            
            if (physicalWeapon.isStriking)
                swordTestNpcCoordinator.ReportHit(physicalWeapon.strikeDirection, limitedRagdollTargetJoint);
            else if (contactPromotesMovement)
                AvoidWeapon(collision);
        }

        private void OnCollisionStay(Collision collision)
        {
            if (contactPromotesMovement
             && collision.TryGetCollidingComponent<PhysicalWeapon>(out var physicalWeapon, out _)
             && !physicalWeapon.isStriking)
                AvoidWeapon(collision);
        }

        private void AvoidWeapon(Collision collision)
        {
            var direction = -(collision.contacts[0].point - transform.position).normalized;
            swordTestNpcCoordinator.EnableWeaponAvoidance(direction);
        }
    }
}