using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Appearance.UMA;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CharacterCreation;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.UMA
{
    public class FeatureNavigationButton : MonoBehaviour
    {
        [SerializeField]
        Wardrobe.Feature feature;

        UMACreatureAssembler creatureAssembler;

        public void NavigateFeature(int direction)
        {
            creatureAssembler = CharacterCreator.Instance.creatureAssembler;

            if (creatureAssembler == null)
                return;

            creatureAssembler.NavigateFeature(feature, direction);
        }
    }
}
