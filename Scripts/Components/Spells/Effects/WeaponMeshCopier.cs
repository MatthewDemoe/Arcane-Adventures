using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class WeaponMeshCopier : MonoBehaviour, ISpellReferencer
    {
        private PhysicalSpell _physicalSpell;

        public PhysicalSpell physicalSpell
        {
            get
            {
                return _physicalSpell;
            }

            set
            {
                _physicalSpell = value;

                InstantiateWeaponMesh();
            }
        }

        [SerializeField]
        Transform instantiateParent;

        [SerializeField]
        Material weaponMaterial;

        public void InstantiateWeaponMesh()
        {
            MeshRenderer weaponMesh = physicalSpell.spellSource.parent.GetComponentInChildren<MeshRenderer>();

            GameObject newWeapon = Instantiate(weaponMesh.gameObject, instantiateParent);
            newWeapon.GetComponent<MeshRenderer>().material = weaponMaterial;
        }
    }
}