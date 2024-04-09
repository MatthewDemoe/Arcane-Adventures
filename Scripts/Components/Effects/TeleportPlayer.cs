using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.UI;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;
using com.AlteredRealityLabs.ArcaneAdventures.Components.CreatureReferences;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class TeleportPlayer : MonoBehaviour
    {

        public void TeleportCharacter(Vector3 position, Vector3 forward)
        {
            CameraFader.FadeOut(0.25f, () =>
                {
                    PlayerCharacterReference.Instance.gameObject.transform.position = position;
                    PlayerCharacterReference.Instance.gameObject.transform.forward = forward;
                    ControllerLink.Left.transform.position = position;
                    ControllerLink.Right.transform.position = position;
                    CameraFader.FadeIn();
                }
            );
        }
    }
}