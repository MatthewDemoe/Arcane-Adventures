using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells.EnemySpells;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponFunctionality
{
    [RequireComponent(typeof(PhysicalWeapon))]
    public class TreeSlamDurabilityTracker : MonoBehaviour
    {
        PhysicalWeapon physicalWeapon;
        Creature wielder;

        [SerializeField]
        List<GameObject> breakOffObjects = new List<GameObject>();

        [SerializeField]
        List<UnityEvent> OnNumberOfCasts = new List<UnityEvent>();       

        public int numberOfCasts { get; private set; } = 0;

        public bool maximumDurabilityReached => numberOfCasts == OnNumberOfCasts.Count;

        const int ExplosionForce = 650;

        void Start()
        {
            physicalWeapon = GetComponent<PhysicalWeapon>();

            if (physicalWeapon.wielder != null)
                AddSpellCastListener();

            physicalWeapon.OnEquipped.AddListener(AddSpellCastListener);
            physicalWeapon.OnUnequipped.AddListener(RemoveSpellCastListener);

            OnNumberOfCasts[OnNumberOfCasts.Count - 1].AddListener(BreakTree);
        }

        private void ProcessSpellCasted(Spell spell)
        {
            if (!spell.name.Equals(TreeSlam.Instance.name) || numberOfCasts >= OnNumberOfCasts.Count)
                return;

            numberOfCasts++;
            OnNumberOfCasts[numberOfCasts - 1].Invoke();
        }

        public void BreakTree()
        {
            breakOffObjects.ForEach(breakOffObject =>
            {
                breakOffObject.transform.parent = null;
                Rigidbody treePieceRigidbody = breakOffObject.AddComponent<Rigidbody>();

                treePieceRigidbody.AddExplosionForce(ExplosionForce, treePieceRigidbody.transform.position + Random.onUnitSphere, 1);
            });
        }

        private void AddSpellCastListener()
        {
            physicalWeapon.wielder.creature.OnSpellCasted += ProcessSpellCasted;
            wielder = physicalWeapon.wielder.creature;
        }

        private void RemoveSpellCastListener()
        {
            wielder.OnSpellCasted -= ProcessSpellCasted;
        }
    }
}