using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using UnityEngine;
using static com.AlteredRealityLabs.ArcaneAdventures.Components.Development.SetupPhaseInputHandler;
using static com.AlteredRealityLabs.ArcaneAdventures.Components.Development.SetupPhaseInputHandler.SetupPhaseSettings;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class PuppetMasterDemoScenePlayerCharacterInitializer : MonoBehaviour
    {
        [SerializeField] private GameObject orc;
        [SerializeField] private GameObject dwarf;
        [SerializeField] private GameObject ogre;
        [SerializeField] private GameObject human;

        private void Awake()
        {
            //var prefab = GetPlayerCharacterPrefab();
            //Instantiate(prefab, parent: null);
            Destroy(this.gameObject);
        }
//TODO: Delete
/*
        private GameObject GetPlayerCharacterPrefab()
        {
            var race = InjectorContainer.Injector.GetInstance<SetupPhaseSettings>().race;

            switch (race)
            {
                case SelectableRace.Orc: return orc;
                case SelectableRace.Dwarf: return dwarf;
                case SelectableRace.Ogre: return ogre;
                case SelectableRace.Human: return human;
            }

            return null;
        }*/
    }
}