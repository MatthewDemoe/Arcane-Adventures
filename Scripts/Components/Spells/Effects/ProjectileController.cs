using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(PhysicalSpellSurface), typeof(PhysicalSpell))]
    public class ProjectileController : MonoBehaviour, ISpellReferencer
    {
        [SerializeField]
        bool isLobbed = false;

        Vector3 _startPosition = new Vector3();
        Vector3 direction = new Vector3();

        bool _isFired = false;
        float _distanceTravelled = 0.0f;
        
        Rigidbody _rigidBody;
        public PhysicalSpell physicalSpell { get; set; }
        Collider _attachedCollider;

        const float LobDirectionStep = 0.4f;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody>();
            _attachedCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (!_isFired)
                return;

            CheckDistanceTravelled();

            if(_rigidBody.velocity.normalized != Vector3.zero)
                transform.forward = _rigidBody.velocity.normalized;
        }

        public void Fire()
        {
            SpellTargeter projectileTargeter = physicalSpell.spellSource.GetComponentInParent<SpellTargeter>(); 
            ProjectileAimAssister aimAssister = GetComponentInChildren<ProjectileAimAssister>();

            direction = projectileTargeter is null ? Vector3.zero : projectileTargeter.targetDirection;

            InitializeProjectile();

            if (projectileTargeter is SpellSingleTargeter)
            {
                SpellSingleTargeter singleTargeter = projectileTargeter as SpellSingleTargeter;

                if (singleTargeter.targetedCreatureReference != null && CombatSettings.Spells.UseSingleTargeter)
                    aimAssister.target = singleTargeter.targetGameObject;
            }

            _rigidBody.AddForce(direction * physicalSpell.correspondingSpell.force);
        }

        public void Throw(Vector3 direction)
        {
            InitializeProjectile();

            _rigidBody.AddForce(direction * physicalSpell.correspondingSpell.force);
        }

        private void InitializeProjectile()
        {
            if (_rigidBody == null)
                _rigidBody = GetComponent<Rigidbody>();

            if (isLobbed)
            {
                _rigidBody.useGravity = true;
                direction = Vector3.RotateTowards(direction, Vector3.up, LobDirectionStep, 0.0f);
            }

            _rigidBody.isKinematic = false;
            _attachedCollider.enabled = true;
            _startPosition = transform.position;
            _isFired = true;
            gameObject.transform.parent = null;
        }

        private void CheckDistanceTravelled()
        {
            _distanceTravelled = Vector3.Distance(_startPosition, transform.position);

            if (_distanceTravelled >= physicalSpell.correspondingSpell.range)
                Destroy(gameObject);
        }
    }
}
