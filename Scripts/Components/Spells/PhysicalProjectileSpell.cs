using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Spells;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Targeting;
using UnityEngine;
using System;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells
{
    [RequireComponent(typeof(ProjectileController))]
    public class PhysicalProjectileSpell : PhysicalSpell
    {
        ProjectileController projectileController;
        Collider attachedCollider;

        [SerializeField]
        GameObject vfxToEnable;

        [SerializeField]
        bool isThrown = false;

        private Vector3 currentSpellSourcePosition = Vector3.zero;
        private Vector3 previousSpellSourcePosition = Vector3.zero;

        public override Type playerSpellTargeter => typeof(SpellSingleTargeter);
        public override Type enemySpellTargeter => typeof(EnemySingleTargeter);

        protected override void Awake()
        {
            base.Awake();

            OnBeginCast.AddListener(() =>
            {
                vfxToEnable.SetActive(true);
            });
        }

        private void Start()
        {
            attachedCollider = GetComponent<Collider>();
            attachedCollider.enabled = false;

            OnEndCast.AddListener(FireProjectile);

            projectileController = GetComponent<ProjectileController>();
        }

        private void Update()
        {
            if (!isThrown)
                return;

            UpdateSpellSourcePosition();
        }

        public override void InitializeSpellInformation(Spell spell, SpellCaster playerSpellCaster, HandSide handSide)
        {
            base.InitializeSpellInformation(spell, playerSpellCaster, handSide);

            spellSource = playerSpellCaster.SpellSourceTransform(handSide);
        }

        public void FireProjectile()
        {
            if(projectileController == null)
                projectileController = GetComponent<ProjectileController>();

            if (isThrown)
                projectileController.Throw((currentSpellSourcePosition - previousSpellSourcePosition).normalized);

            else
                projectileController.Fire();
        }

        private void UpdateSpellSourcePosition()
        {
            previousSpellSourcePosition = currentSpellSourcePosition;
            currentSpellSourcePosition = spellSource.position;
        }
    }
}

