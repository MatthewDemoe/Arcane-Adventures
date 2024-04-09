using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(PhysicalSpell))]
    public class PersistentWeaponAttackBuff : MonoBehaviour
    {
        Creature caster;

        private PhysicalSpell _physicalSpell;
        public PhysicalSpell physicalSpell
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;

                caster = physicalSpell.correspondingSpell.GetCaster();

                creatureEffect = new CreatureEffect(
                    name: "Weapon Attack Buff",
                    description: $"Weapon attack increased by {buffPercent}.",
                    source: physicalSpell.correspondingSpell.name,
                    weaponDamageDealt: buffPercent
                    );
            }
        }

        CreatureEffect creatureEffect;

        [SerializeField]
        float buffPercent = 1.5f;

        bool isActive = false;

        public void AddEffectToCaster()
        {
            if (isActive)
                return;

            isActive = true;
            caster.modifiers.AddEffect(creatureEffect);
        }

        public void RemoveEffectFromCaster()
        {
            if (!isActive)
                return;

            isActive = false;
            caster.modifiers.RemoveEffect(creatureEffect);
        }

        private void OnDestroy()
        {
            RemoveEffectFromCaster();
        }
    }
}