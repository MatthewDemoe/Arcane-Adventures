using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures.Characters;
using RootMotion.Dynamics;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Common
{
    public class SpawnPoint : MonoBehaviour
    {
        private void Update()
        {
            var playerCharacterReference = PlayerCharacterReference.Instance;
            
            if (playerCharacterReference is null)
                return;

            if (!playerCharacterReference.isGhost && playerCharacterReference.isUsingPuppetMaster)
            {
                var puppetMaster = playerCharacterReference.transform.parent.GetComponentInChildren<PuppetMaster>();
                puppetMaster.DisableImmediately();
                puppetMaster.Teleport(transform.position, transform.rotation, false);
                puppetMaster.SwitchToActiveMode();
            }
            else
            {
                var player = playerCharacterReference.gameObject;
                
                player.transform.position = transform.position;
                player.transform.forward = transform.forward;
            }

            foreach (var handSide in new HandSide[] { HandSide.Left, HandSide.Right })
            {
                var controllerLink = ControllerLink.Get(handSide);
                controllerLink.transform.position = transform.position;

                if (controllerLink.isHoldingItem)
                {
                    controllerLink.connectedItem.transform.position = transform.position;
                }
            }

            Destroy(this.gameObject);
        }
    }
}