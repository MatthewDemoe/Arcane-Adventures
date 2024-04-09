using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Items;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using UMA;
using UMA.CharacterSystem;
using UMA.Dynamics;
using UnityEngine.AI;
using TheKiwiCoder;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public abstract class HumanoidMonster : Monster
    {
        private bool isUsingUMA = false;

        public NPCItemEquipper itemEquipper { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            itemEquipper = GetComponent<NPCItemEquipper>();

            if (TryGetComponent<DynamicCharacterAvatar>(out var dynamicCharacterAvatar))
            {
                isUsingUMA = true;
                dynamicCharacterAvatar.CharacterUpdated.AddListener(OnCharacterBegunCallback);
            }

            OnDeath.AddListener(DisableCreatureBehaviours);
        }

        protected override void Start()
        {
            base.Start();

            if (!isUsingUMA)
                InstantiateEquippedWeapons();
        }

        private void OnCharacterBegunCallback(UMAData umaData)
        {
            InstantiateEquippedWeapons();
        }

        public override void ProcessDamage()
        {
            base.ProcessDamage();

            animator.SetBool(CharacterAnimatorParameters.IsRecoilingFromHit, true);
        }

        protected void InstantiateEquippedWeapons()
        {
            if (creature is null)
            {
                UnityEngine.Debug.LogWarning("Humanoid monster did not have proper creature set, cannot initialize equipped weapons.");
                return;
            }

            var humanoidMonster = creature as GameSystem.Creatures.Monsters.HumanoidMonster;

            if (humanoidMonster.leftHandItem is HandHeldItem)
                itemEquipper.InstantiateEquippedItem(humanoidMonster.leftHandItem, HandSide.Left);

            if (humanoidMonster.rightHandItem is HandHeldItem)
                itemEquipper.InstantiateEquippedItem(humanoidMonster.rightHandItem, HandSide.Right);
        }

        private void DisableCreatureBehaviours()
        {
            //TODO: Reintroduce when new animation is ready: animator.SetBool(BanditAnimatorParameters.IsDead, true);
            itemEquipper.UnequipItem(HandSide.Left);
            itemEquipper.UnequipItem(HandSide.Right);

            if (TryGetComponent<UMAPhysicsAvatar>(out var umaPhysicsAvatar))
                umaPhysicsAvatar.ragdolled = true;

            if (TryGetComponent(out NavMeshAgent navMeshAgent))
                navMeshAgent.enabled = false;

            if (TryGetComponent(out BehaviourTreeRunner behaviourTreeRunner))
                behaviourTreeRunner.enabled = false;
        }
    }
}