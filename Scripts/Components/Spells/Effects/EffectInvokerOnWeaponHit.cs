using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class EffectInvokerOnWeaponHit : MonoBehaviour
    {
        [ShowIf(nameof(effectOnHit))]
        public UnityEvent<Creature> OnWeaponHitCreature = new UnityEvent<Creature>();
        [ShowIf(nameof(effectOnHit))]
        public UnityEvent<CreatureReference> OnWeaponHitCreatureReference = new UnityEvent<CreatureReference>();

        [ShowIf(nameof(effectOnCreatureDeath))]
        public UnityEvent<Creature> OnCreatureDeath = new UnityEvent<Creature>();
        [ShowIf(nameof(effectOnCreatureDeath))]
        public UnityEvent<CreatureReference> OnCreatureReferenceDeath = new UnityEvent<CreatureReference>();

        [SerializeField]
        bool addListenerOnAwake = false;

        [SerializeField]
        bool specifyStrikeType = false;

        [SerializeField]
        bool effectOnHit = true;

        [SerializeField]
        bool effectOnCreatureDeath = true;

        [ShowIf(nameof(specifyStrikeType))] [SerializeField]
        StrikeType strikeType = StrikeType.NotStrike;        

        bool effectRemoved = false;

        private void Start()
        {
            if (addListenerOnAwake)
                GetComponentInParent<PhysicalWeapon>().OnAttack.AddListener(InvokeEvent);
        }

        private void InvokeEvent(CreatureReference target, StrikeType strikeType)
        {
            if (specifyStrikeType && strikeType != this.strikeType)
                return;

            if (effectOnHit)
            {
                OnWeaponHitCreature.Invoke(target.creature);
                OnWeaponHitCreatureReference.Invoke(target);
            }

            if (target.creature.isDead && effectOnCreatureDeath)
            {
                OnCreatureDeath.Invoke(target.creature);
                OnCreatureReferenceDeath.Invoke(target);
            }

            RemoveOnHitListener();
        }

        public void AddOnHitListener()
        {
            if(!addListenerOnAwake)
                GetComponentInParent<PhysicalWeapon>().OnAttack.AddListener(InvokeEvent);
        }

        public void RemoveOnHitListener()
        {
            if (effectRemoved)
                return;

            GetComponentInParent<PhysicalWeapon>().OnAttack.RemoveListener(InvokeEvent);

            effectRemoved = true;
        }
    }
}
