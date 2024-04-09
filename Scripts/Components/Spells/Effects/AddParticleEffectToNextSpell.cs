using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Creatures.Characters;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class AddParticleEffectToNextSpell : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        GameObject initialParticlesPrefab;
        GameObject initialParticlesInstance;

        [SerializeField]
        GameObject collisionParticlesPrefab;

        public PhysicalSpell physicalSpell { get; set; }

        public void SubscribeToAddParticles()
        {
            (physicalSpell.correspondingSpell.GetCaster() as Character).OnStartCastSpell += AttachParticleSystem;

            initialParticlesInstance = Instantiate(initialParticlesPrefab, transform.parent.position, transform.parent.rotation, transform.parent);
            initialParticlesInstance.transform.localScale = transform.localScale;
        }

        private void AttachParticleSystem(Spell spell)
        {
            Destroy(initialParticlesInstance);

            ParticleSystemInstantiator particleSystemInstantiator = physicalSpell.gameObject.AddComponent<ParticleSystemInstantiator>();
            particleSystemInstantiator.physicalSpell = physicalSpell;
            particleSystemInstantiator.InitializeParticleSystems(initialParticlesPrefab, collisionParticlesPrefab);

            (physicalSpell.correspondingSpell.GetCaster() as Character).OnStartCastSpell -= AttachParticleSystem;
        }
    }
}