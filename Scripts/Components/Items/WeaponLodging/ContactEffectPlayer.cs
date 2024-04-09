using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class ContactEffectPlayer : MonoBehaviour
    {
        [SerializeField] private ParticleSystem bloodEffect;
        [SerializeField] private ParticleSystem woodChipsEffect;
        [SerializeField] private ParticleSystem sparksEffect;
        [SerializeField] private ParticleSystem stoneFragmentsEffect;
        [SerializeField] private ParticleSystem shatteredGlassEffect;
        [SerializeField] private ParticleSystem waterSplashEffect;

        private Dictionary<ContactEffect, ParticleSystem> contactEffectByIdentifier;
        private ContactEffect activeContactEffectIdentifier = ContactEffect.Nothing;

        public bool isAvailable { get; private set; } = true;

        private void Awake()
        {
            contactEffectByIdentifier = new Dictionary<ContactEffect, ParticleSystem>
            {
                { ContactEffect.Blood, bloodEffect },
                { ContactEffect.WoodChips, woodChipsEffect },
                { ContactEffect.Sparks, sparksEffect },
                { ContactEffect.StoneFragments, stoneFragmentsEffect },
                { ContactEffect.ShatteredGlass, shatteredGlassEffect },
                { ContactEffect.WaterSplash, waterSplashEffect },
            };

            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
           if (isAvailable) { return; }

            var contactEffect = contactEffectByIdentifier[activeContactEffectIdentifier];

            if (contactEffect.IsAlive()) { return; }

            contactEffect.Stop();
            isAvailable = true;
            contactEffect.gameObject.SetActive(false);
        }

        public void PlayEffect(ContactEffect contactEffectIdentifier, Vector3 position, float scale)
        {
            isAvailable = false;
            activeContactEffectIdentifier = contactEffectIdentifier;

            transform.position = position;
            transform.localScale = new Vector3(scale, scale, scale);

            var contactEffect = contactEffectByIdentifier[contactEffectIdentifier];

            contactEffect.Simulate(0.0f, true, true);
            contactEffect.gameObject.SetActive(true);
            contactEffect.Play();
        }
    }
}