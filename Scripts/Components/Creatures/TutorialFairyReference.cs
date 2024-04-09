using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures
{
    public class TutorialFairyReference : Monster
    {
        protected override Type creatureType { get { return typeof(GameSystem.Creatures.Monsters.TutorialFairy); } }

        private const float WingDegreeChangePerSecond = 700;
        private const float TimeBetweenIdleSounds = 20;

        private const float HeadYForce = 4.5f;

        [SerializeField] private AudioClip deathSound;
        [SerializeField] private AudioClip distanceBadVelocityBadSound;
        [SerializeField] private AudioClip distanceBadVelocityOKSound;
        [SerializeField] private AudioClip distanceOKVelocityBadSound;
        [SerializeField] private AudioClip distanceOKVelocityOKSound;
        [SerializeField] private AudioClip[] idleSounds;
        [SerializeField] private Rigidbody headRigidbody;
        [SerializeField] private GameObject rightWing;
        [SerializeField] private GameObject leftWing;

        private bool isWingCyclePlus;
        private AudioSource audioSource;
        private float lastTimeSoundWasPlayed;
        private int nextIdleSound = 0;
        private Camera mainCamera;

        public override void ProcessDamage()
        {
            base.ProcessDamage();

            if (creature.isDead)
            {
                audioSource.PlayOneShot(deathSound, 1);
            }
        }

        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            mainCamera = Camera.main;
        }

        private void Update()
        {
            if (!creature.isDead)
            {
                Talk();
                FlapWings();
            }
        }

        private void FixedUpdate()
        {
            if (!creature.isDead && creature.isMovementEnabled)
            {
                var direction = mainCamera.transform.forward;
                direction.y = 0;
                var targetPosition = mainCamera.transform.position + direction * 4;
                targetPosition.y += 1;

                var force = (targetPosition - headRigidbody.position).normalized * 2;
                force.y *= HeadYForce;

                headRigidbody.AddForce(force, ForceMode.Impulse);
            }
        }

        private void FlapWings()
        {
            var wingDegreeChange = WingDegreeChangePerSecond * Time.deltaTime * (isWingCyclePlus ? 1 : -1);

            rightWing.transform.Rotate(0, 0, wingDegreeChange);
            leftWing.transform.Rotate(0, 0, wingDegreeChange);
            var wingDegrees = rightWing.transform.rotation.eulerAngles.z;

            if ((isWingCyclePlus && wingDegrees < 180 && wingDegrees > 15) ||
                (!isWingCyclePlus && wingDegrees > 180 && wingDegrees < 345))
            {
                isWingCyclePlus = !isWingCyclePlus;
            }
        }

        private void Talk()
        {
            if (PlayStrikeAdviceSound(HandSide.Left) || PlayStrikeAdviceSound(HandSide.Right))
            {
                return;
            }
            else if (!audioSource.isPlaying && Time.time > lastTimeSoundWasPlayed + TimeBetweenIdleSounds)
            {
                PlayNextIdleSound();
            }
        }

        private bool PlayStrikeAdviceSound(HandSide handSide)
        {
            var controllerLink = ControllerLink.Get(handSide);

            if (!controllerLink.isHoldingItem)
            {
                return false;
            }

            var hitStrike = controllerLink.connectedItem.GetComponent<PhysicalWeapon>().hitStrike;

            if (hitStrike == null || lastHitStrikeGuidByHandSide[handSide].Equals(hitStrike.strikeGuid))
            {
                return false;
            }

            lastHitStrikeGuidByHandSide[handSide] = hitStrike.strikeGuid;

            audioSource.Stop();
            PlayClip(GetStrikeAudioClip(hitStrike));

            return true;
        }

        private AudioClip GetStrikeAudioClip(Strike strike)
        {
            if (strike.momentumPoints < 2 && strike.velocityPoints < 2)
            {
                return distanceBadVelocityBadSound;
            }
            else if (strike.momentumPoints < 2)
            {
                return distanceBadVelocityOKSound;
            }
            else if (strike.velocityPoints < 2)
            {
                return distanceOKVelocityBadSound;
            }
            else
            {
                return distanceOKVelocityOKSound;
            }
        }

        private Dictionary<HandSide, Guid> lastHitStrikeGuidByHandSide = new Dictionary<HandSide, Guid>
        {
            { HandSide.Left, Guid.Empty },
            { HandSide.Right, Guid.Empty }
        };

        private void PlayNextIdleSound()
        {
            PlayClip(idleSounds[nextIdleSound]);
            nextIdleSound = nextIdleSound + 1 == idleSounds.Length ? 0 : nextIdleSound + 1;
        }

        private void PlayClip(AudioClip audioClip)
        {
            audioSource.PlayOneShot(audioClip, 1);
            lastTimeSoundWasPlayed = Time.time;
        }
    }
}