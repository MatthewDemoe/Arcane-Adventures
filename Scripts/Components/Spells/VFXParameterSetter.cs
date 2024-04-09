using UnityEngine;
using UnityEngine.VFX;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class VFXParameterSetter : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        string vfxParameterToSet;

        [SerializeField]
        float scaleMultiplier = 1.0f;
        VisualEffect visualEffect;

        private bool physicalSpellSet = false;

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
                spellSourceTransform = _physicalSpell.spellSource;
                physicalSpellSet = true;
            } 
        }

        private Transform spellSourceTransform;

        private void Start()
        {
            visualEffect = GetComponentInChildren<VisualEffect>();            
        }

        void Update()
        {
            if (physicalSpellSet)
            {
                transform.forward = (spellSourceTransform.position - transform.position).normalized;
                visualEffect.SetFloat(vfxParameterToSet, Vector3.Magnitude(spellSourceTransform.position - transform.position) * scaleMultiplier);
            }
        }
    }
}