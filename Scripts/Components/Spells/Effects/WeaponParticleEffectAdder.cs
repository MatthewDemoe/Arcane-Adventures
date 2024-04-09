using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class WeaponParticleEffectAdder : PhysicalWeaponModifier
    {
        [SerializeField]
        public GameObject particlesToInstantiate;
        GameObject instanceOfParticles;

        protected override void ModifyWeapon(GameObject gameObject)
        {
            instanceOfParticles = Instantiate(particlesToInstantiate, physicalSpell.spellSource.GetComponentInParent<PhysicalWeapon>().transform);
        }

        protected override void ResetWeapon(GameObject gameObject)
        {
            if (!(instanceOfParticles is null))
                Destroy(instanceOfParticles);
        }
    }
}