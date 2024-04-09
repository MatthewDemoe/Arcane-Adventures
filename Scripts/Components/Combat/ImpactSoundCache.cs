using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using Injection;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    public class ImpactSoundCache : IInjectable
    {
        public ImpactSoundCache()
        {
            InitializeImpactSounds();
        }

        Dictionary<string, Dictionary<string, Dictionary<string, AudioClip[]>>> impactSounds = new Dictionary<string, Dictionary<string, Dictionary<string, AudioClip[]>>>();
        Dictionary<string, AudioClip> scrapeAudioClipsByName = new Dictionary<string, AudioClip>();

        const string Directory = "Prefabs/Audio/Impacts";

        public void InitializeImpactSounds()
        {            
            foreach (string surfaceType in Enum.GetNames(typeof(InGameMaterial)))
            {
                Dictionary<string, Dictionary<string, AudioClip[]>> impactSoundsByWeaponType = new Dictionary<string, Dictionary<string, AudioClip[]>>();

                AudioClip scrapeSound = Resources.Load<AudioClip>($"{Directory}/{surfaceType}/Scrape");

                if(!(scrapeSound is null))
                    scrapeAudioClipsByName.Add(surfaceType, scrapeSound);

                foreach (string weaponType in Enum.GetNames(typeof(DamageType)))
                {
                   Dictionary<string, AudioClip[]> impactSoundsByStrkeType = new Dictionary<string, AudioClip[]>();

                    foreach (string strikeType in Enum.GetNames(typeof(StrikeType)))
                    {
                        AudioClip[] audioClipsByStrikeType = Resources.LoadAll<AudioClip>($"{Directory}/{surfaceType}/{weaponType}/{strikeType}");

                        if (audioClipsByStrikeType.Any())
                            impactSoundsByStrkeType.Add(strikeType, audioClipsByStrikeType);
                    }

                    if (impactSoundsByStrkeType.Any())
                        impactSoundsByWeaponType.Add(weaponType, impactSoundsByStrkeType);
                }

                if (impactSoundsByWeaponType.Any())
                    impactSounds.Add(surfaceType, impactSoundsByWeaponType);
            }
        }

        public AudioClip[] GetImpactSounds(InGameMaterial surfaceType, DamageType weaponType, StrikeType strikeType)
        {
            if (!impactSounds.ContainsKey(surfaceType.ToString())
                || !impactSounds[surfaceType.ToString()].ContainsKey(weaponType.ToString())
                || !impactSounds[surfaceType.ToString()][weaponType.ToString()].ContainsKey(strikeType.ToString()))
                return null;

            return impactSounds[surfaceType.ToString()][weaponType.ToString()][strikeType.ToString()];
        }

        public AudioClip GetScrapeSound(InGameMaterial surfaceType)
        {
            if (!scrapeAudioClipsByName.ContainsKey(surfaceType.ToString()))
                return null;

            return scrapeAudioClipsByName[surfaceType.ToString()];
        }
    }
}