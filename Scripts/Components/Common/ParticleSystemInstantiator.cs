using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class ParticleSystemInstantiator : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        GameObject _startParticleSystemPrefab;

        [SerializeField]
        GameObject _collisionParticleSystemPrefab;

        GameObject _startParticleSystemInstance;
        GameObject _collisionParticleSystemInstance;


        [SerializeField]
        bool useCollisionParticles = true;

        [SerializeField]
        bool _attached = true;

        [SerializeField]
        bool _fromSpellSource = false;

        [SerializeField]
        bool _fromCaster = false;

        [SerializeField]
        bool neutralRotation = false;

        Transform targetTransform;

        public PhysicalSpell physicalSpell { get; set; }

        public void InitializeParticleSystems(GameObject startParticleSystem, GameObject collisionParticleSystem, bool attached = true, bool fromSpellSource = false, bool fromCaster = false)
        {
            _startParticleSystemPrefab = startParticleSystem;
            _collisionParticleSystemPrefab = collisionParticleSystem;

            _attached = attached;
            _fromSpellSource = fromSpellSource;
            _fromCaster = fromCaster;

            InstantiateParticles();
        }

        public void InstantiateParticles()
        {
            targetTransform = _fromSpellSource ? physicalSpell.spellSource : _fromCaster ? physicalSpell.spellCaster.transform: physicalSpell.transform;

            _startParticleSystemInstance = Instantiate(_startParticleSystemPrefab, 
                targetTransform.position, 
                neutralRotation ? Quaternion.identity : targetTransform.rotation, 
                _attached ? targetTransform : null);

            _startParticleSystemInstance.transform.localScale = Vector3.one * physicalSpell.correspondingSpell.radius;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!useCollisionParticles)
                return;

            if (_attached)
                Destroy(_startParticleSystemInstance);

            _collisionParticleSystemInstance = Instantiate(_collisionParticleSystemPrefab, physicalSpell.transform.position, physicalSpell.transform.rotation);
            _collisionParticleSystemInstance.transform.localScale = physicalSpell.spellSource.localScale;
        }

        private void OnDestroy()
        {
            if (!(_startParticleSystemInstance is null))
                Destroy(_startParticleSystemInstance);
        }
    }
}