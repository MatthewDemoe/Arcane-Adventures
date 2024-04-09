using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using com.AlteredRealityLabs.ArcaneAdventures.Components.ItemEquipper;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Items;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace com.AlteredRealityLabs.ArcaneAdventures.XR
{
    public class XRSpellSourceRayInteractor : XRRayInteractor
    {
        [SerializeField]
        HandSide handSide;

        

        protected override void Awake()
        {     
            base.Awake();

            StartCoroutine(InitializeRoutine());
        }

        private void SetOriginalAttachTransform(GameObject gameObject)
        {
            originalAttachTransform = gameObject.GetComponent<PhysicalWeapon>().spellSource.transform;
        }

        IEnumerator InitializeRoutine()
        {
            yield return new WaitUntil(() => GetComponentInParent<XRRig>().transform.parent != null);

            ItemEquipper itemEquipper = GetComponentInParent<XRRig>().transform.parent.GetComponentInChildren<ItemEquipper>();
            originalAttachTransform = itemEquipper.GetWeaponInHand(handSide) == null ? transform : itemEquipper.GetWeaponInHand(handSide).spellSource.transform;

            itemEquipper.OnEquipItem.AddListener(SetOriginalAttachTransform);
        }
    }
}