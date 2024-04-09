using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(SpellReference))]
    public class ComboCastEffectApplier : MonoBehaviour
    {
        public UnityEvent OnInfernoCombo = new UnityEvent();
        public UnityEvent OnOceanCombo = new UnityEvent();
        public UnityEvent OnEarthCombo = new UnityEvent();
        public UnityEvent OnStormCombo = new UnityEvent();

        private Dictionary<Element, UnityEvent> _comboEventsByElement => new Dictionary<Element, UnityEvent>()
        {
            { Element.Inferno, OnInfernoCombo },
            { Element.Ocean, OnOceanCombo },
            { Element.Earth, OnEarthCombo },
            { Element.Storm, OnStormCombo },
        };

        SpellReference _spellReference;
        bool _comboTriggered = false;

        void Start()
        {
            _spellReference = GetComponent<SpellReference>();
        }

        public void CheckForCombos(Spell comboingSpell)
        {
            if (_comboTriggered)
                return;

            var comboEventsByElement = _comboEventsByElement;

            comboingSpell.elements.ForEach((element) =>
            {
                comboEventsByElement[element].Invoke();

                if (comboEventsByElement[element].GetPersistentEventCount() > 0)
                    _comboTriggered = true;
            });
        }
    }
}