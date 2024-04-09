using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UI
{
    public class CanvasButtonHoverEventInitializer : MonoBehaviour
    {        
        void Start()
        {
            //TODO: Extend this to more than button elements. 
            List<Button> childrenWithButton = GetComponentsInChildren<Button>().ToList();

            childrenWithButton.ForEach((button) => button.gameObject.AddComponent<SpellCastDisableEventTrigger>());
        }

        private void OnDisable()
        {
            PlayerCharacterReference.Instance.GetComponent<PlayerSpellCaster>().ResetHoveringUI();
        }
    }
}