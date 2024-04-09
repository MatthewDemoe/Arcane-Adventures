using com.AlteredRealityLabs.ArcaneAdventures.Components.Audio;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Identifiers;
using Injection;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Combat
{
    public class WeaponSoundPlayer : AudioSourcePoolPlayer
    {
        private const float ScrapeStopTime = 0.5f;

        private bool weaponIsMoving = false;
        private bool weaponIsScraping = false;
        private bool scrapeIsPlaying = false;
        private bool stopping = false;

        [Inject] ImpactSoundCache impactSoundCache;

        private static readonly Dictionary<StrikeType, float> StrikeSoundEffectVolumeByStrikeType = new Dictionary<StrikeType, float>
        {
            { StrikeType.Perfect, 1 },
            { StrikeType.Imperfect, 0.5f },
            { StrikeType.Incomplete, 0.25f }
        };

        [SerializeField] AudioClip strikeAudioClip;
        [SerializeField] AudioClip scrapeAudioClip;

        private AudioSource scrapeAudioSource;     

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);

            PhysicalWeapon physicalWeapon = GetComponentInParent<PhysicalWeapon>();

            if (physicalWeapon == null)//TODO: Items (vitality and spirit shards) do not have a PhysicalWeapon and throw null exception unless we escape the null case.
                return;
            
            physicalWeapon.OnColliderEventWithWeaponLodgeSurface.AddListener(SetWeaponScrapingState);
            physicalWeapon.GetStrikeCalculator(HandSide.Left).OnWeaponMoveStateChanged += SetWeaponMovingState;
            physicalWeapon.GetStrikeCalculator(HandSide.Right).OnWeaponMoveStateChanged += SetWeaponMovingState;
        }

        public void PlayStrikeSound(StrikeType strikeType)
        {
            GetFirstAvailableAudioSource().PlayAudioClip(strikeAudioClip, new GeneralAudioClipSettings(volume: StrikeSoundEffectVolumeByStrikeType[strikeType]));
        }

        public void PlayImpactSound(InGameMaterial surfaceType, DamageType weaponType, StrikeType strikeType, float volume)
        {
            AudioClip[] impactSounds = impactSoundCache.GetImpactSounds(surfaceType, weaponType, strikeType);

            if (impactSounds is null)
                return;

            AudioClip impactAudioClip = impactSounds.ToList()[Random.Range(0, impactSounds.ToList().Count)];

            GetFirstAvailableAudioSource().PlayAudioClip(impactAudioClip, new GeneralAudioClipSettings(volume: volume));
        }

        private void SetWeaponMovingState(bool weaponIsMoving)
        {
            this.weaponIsMoving = weaponIsMoving;
            UpdateScrapeState();
        }

        private void SetWeaponScrapingState(bool weaponIsScraping)
        {
            this.weaponIsScraping = weaponIsScraping;
            UpdateScrapeState();
        }

        private void UpdateScrapeState()
        {
            if (weaponIsMoving && !scrapeIsPlaying && weaponIsScraping)
            {
                StopAllCoroutines();

                scrapeIsPlaying = true;
                stopping = false;

                scrapeAudioSource = GetFirstAvailableAudioSource();
                scrapeAudioSource.PlayAudioClip(scrapeAudioClip, new GeneralAudioClipSettings(loop: true));
            }

            else if (scrapeIsPlaying && !stopping && ((!weaponIsMoving && weaponIsScraping) || !weaponIsScraping))
            {
                StopAllCoroutines();
                StartCoroutine(StopScrapeRoutine());
            }
        }

        IEnumerator StopScrapeRoutine()
        {
            float scrapeStopTimer = 0.0f;
            float normalizedTime = 0.0f;

            stopping = true;

            while (scrapeStopTimer < ScrapeStopTime)
            {
                normalizedTime = UtilMath.Lmap(scrapeStopTimer, 0.0f, ScrapeStopTime, 1.0f, 0.0f);

                scrapeAudioSource.volume = normalizedTime;

                //Placeholder for "Shing" sound
                if(!weaponIsScraping)
                    scrapeAudioSource.pitch = 1.0f + normalizedTime;

                yield return null;

                scrapeStopTimer += Time.deltaTime;
            }

            scrapeAudioSource.StopAudioClip(audioSourcePoolParent.transform);
            scrapeIsPlaying = false;
        }
    }
}