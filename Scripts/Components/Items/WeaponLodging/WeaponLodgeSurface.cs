using com.AlteredRealityLabs.ArcaneAdventures.Components.Combat;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Player;
using com.AlteredRealityLabs.ArcaneAdventures.DependencyInjection;
using com.AlteredRealityLabs.ArcaneAdventures.GameSystem.Combat;
using Injection;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Items.WeaponLodging
{
    public class WeaponLodgeSurface : MonoBehaviour
    {
        [SerializeField] private Identifiers.InGameMaterial _material;
        [Inject] protected InGameMaterialCollisionProcessor inGameMaterialCollisionProcessor;
        private List<LodgedWeaponPoint> lodgedWeaponPoints = new List<LodgedWeaponPoint>();

        private bool cameraIsUnderWater => colliders.Any(collider => collider.bounds.Contains(Camera.main.transform.position));

        public Collider[] colliders { get; private set; }
        public AttackableSurface attackableSurface { get; private set; }

        public InGameMaterial material => inGameMaterialCollisionProcessor.Resolve(_material);
        public LodgedWeaponPoint GetLodgedWeaponPoint(PhysicalWeapon physicalWeapon) => lodgedWeaponPoints.SingleOrDefault(lodgedWeaponPoint => lodgedWeaponPoint != null && lodgedWeaponPoint.physicalWeapon.Equals(physicalWeapon));
        public void ReportLodgedWeaponPointRemoved(LodgedWeaponPoint lodgedWeaponPoint) => lodgedWeaponPoints.Remove(lodgedWeaponPoint);
        public void PlayContactEffect(Identifiers.InGameMaterial inGameMaterial, Vector3 point, float scale)
            => inGameMaterialCollisionProcessor.PlayContactEffects(_material, inGameMaterial, point, scale);

        private void Start()
        {
            InjectorContainer.Injector.Inject(this);
            colliders = GetComponents<Collider>();
            attackableSurface = GetComponent<AttackableSurface>();
        }

        private bool isSubmerged = false;

        private void FixedUpdate()
        {
            if (material.identifier.Equals(Identifiers.InGameMaterial.Water) && cameraIsUnderWater != isSubmerged)
            {
                Camera.main.GetComponentInChildren<WaterOverlay>().Set(cameraIsUnderWater);
                isSubmerged = cameraIsUnderWater;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            bool isCollisionWithPhysicalWeapon = collision.gameObject.TryGetComponent<PhysicalWeapon>(out var physicalWeapon);

            if (isCollisionWithPhysicalWeapon)
            {
                var weaponPart = physicalWeapon.parts.First(weaponPart => weaponPart.colliders.Contains(collision.collider));
                var weaponSurface = physicalWeapon.weapon.weaponSurfaces.First(surface => surface.name.Equals(weaponPart.name));

                //TODO: Get volume + play sound through inGameMaterialCollisionProcessor
                if(physicalWeapon.strikeType != StrikeType.NotStrike)
                    physicalWeapon.weaponSoundPlayer.PlayImpactSound(material.identifier, weaponSurface.damageType, physicalWeapon.strikeType, 1.0f);

                physicalWeapon.OnColliderEventWithWeaponLodgeSurface.Invoke(true);
            }

            if (material.canLodgeWeapon)
            {
                if (inGameMaterialCollisionProcessor.TryLodge(collision, this, out var newLodgedWeaponPoint))
                {
                    lodgedWeaponPoints.Add(newLodgedWeaponPoint);
                }
            }
            else if (isCollisionWithPhysicalWeapon)
            {
                var contactPoint = collision.contacts.First();
                var position = GetOverlappingPosition(contactPoint.point, contactPoint.otherCollider, contactPoint.thisCollider);

                if (material.canShatter && physicalWeapon.isStriking)
                {
                    var shatterableGlassInfo = new ShatterableGlassInfo(position, physicalWeapon.strikeDirection);
                    gameObject.SendMessage("Shatter3D", shatterableGlassInfo);
                }
                else
                {
                    var physicalWeaponPartInGameMaterial = physicalWeapon.GetColliderInGameMaterial(contactPoint.otherCollider);
                    inGameMaterialCollisionProcessor.PlayContactEffects(material.identifier, physicalWeaponPartInGameMaterial, position, 1);//TODO: Set scale.
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            bool isCollisionWithPhysicalWeapon = collision.gameObject.TryGetComponent<PhysicalWeapon>(out var physicalWeapon);

            if (isCollisionWithPhysicalWeapon)
            {
                physicalWeapon.OnColliderEventWithWeaponLodgeSurface.Invoke(false);
            }
        }

        private void OnTriggerEnter(Collider other) => ProcessTrigger(other, enter: true);
        private void OnTriggerExit(Collider other) => ProcessTrigger(other, enter: false);

        private void ProcessTrigger(Collider other, bool enter)
        {
            if (!other.gameObject.TryGetComponent<PhysicalWeapon>(out var physicalWeapon)) { return; }

            if (material.identifier.Equals(Identifiers.InGameMaterial.Water))
            {
                physicalWeapon.SetUnderWater(enter);
            }

            var position = GetOverlappingPosition(transform.position, other, colliders.First());//TODO: Pick correct collider. 
            var physicalWeaponPartInGameMaterial = physicalWeapon.GetColliderInGameMaterial(other);
            inGameMaterialCollisionProcessor.PlayContactEffects(material.identifier, physicalWeaponPartInGameMaterial, position, 1);//TODO: Set scale.
        }

        public Vector3 GetOverlappingPosition(Vector3 position, Collider weaponCollider, Collider surfaceCollider)
        {
            position = weaponCollider.ClosestPointOnBounds(position);

            return surfaceCollider.ClosestPointOnBounds(position);
        }
    }
}