using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AddPersistentEffectToTarget : MonoBehaviour, ISpellReferencer
    {
        public PhysicalSpell physicalSpell { get; set; }

        [SerializeField]
        GameObject _effectToAdd;

        GameObject _effectInstance = null;
        SpellSingleTargeter spellTargeter = null;

        private void Start()
        {
            spellTargeter = GetComponentInParent<SpellSingleTargeter>();
        }

        public void AddEffectToTarget()
        {
            if (spellTargeter.targetedCreatureReference == null)
                return;

            GameObject targetCreature = spellTargeter.targetGameObject.GetComponentInParent<CreatureReference>().gameObject;

            GameObject spellEffectAnchor = targetCreature.GetComponentInChildren<SpellEffectAnchor>().gameObject;

            if (spellEffectAnchor == null)
                spellEffectAnchor = targetCreature;

            _effectInstance = Instantiate(_effectToAdd, spellEffectAnchor.transform.position, spellEffectAnchor.transform.rotation, spellEffectAnchor.transform);
            _effectInstance.GetComponent<SpellReference>().Initialize(physicalSpell);
        }

        public void AddEffectToTarget(CreatureReference creatureReference)
        {
            GameObject target = creatureReference.gameObject;

            _effectInstance = Instantiate(_effectToAdd, target.transform.position, target.transform.rotation, target.transform);
            _effectInstance.GetComponent<SpellReference>().Initialize(physicalSpell);
        }

        public void RemoveEffectFromTarget()
        {
            if(_effectInstance != null)
                Destroy(_effectInstance);
        }

        public void RemoveParentFromEffect()
        {
            _effectInstance.transform.parent = null;
        }
    }
}