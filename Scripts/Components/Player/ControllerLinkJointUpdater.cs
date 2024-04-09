using UnityEngine;
using com.AlteredRealityLabs.ArcaneAdventures.Components.Controls;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Player
{
    [RequireComponent(typeof(PlayerAvatar))]
    public class ControllerLinkJointUpdater : MonoBehaviour
    {
        private ConfigurableJoint leftControllerLinkJoint;
        private ConfigurableJoint rightControllerLinkJoint;
        private PlayerAvatar playerAvatar;

        private void Awake()
        {
            playerAvatar = GetComponent<PlayerAvatar>();
        }

        private void Start()
        {
            CreateJoints();
        }

        private void Update()
        {
            if (playerAvatar.leftArmTransform is null || playerAvatar.rightArmTransform is null ||
                leftControllerLinkJoint == null || rightControllerLinkJoint == null) { return; }

            leftControllerLinkJoint.anchor = transform.InverseTransformPoint(playerAvatar.leftArmTransform.position);
            rightControllerLinkJoint.anchor = transform.InverseTransformPoint(playerAvatar.rightArmTransform.position);
        }

        private void CreateJoints()
        {
            leftControllerLinkJoint = AddControllerLinkJoint();
            leftControllerLinkJoint.connectedBody = ControllerLink.Left.rigidBody;

            rightControllerLinkJoint = AddControllerLinkJoint();
            rightControllerLinkJoint.connectedBody = ControllerLink.Right.rigidBody;
        }

        private void DestroyJoints()
        {
            Destroy(leftControllerLinkJoint);
            Destroy(rightControllerLinkJoint);
        }

        private ConfigurableJoint AddControllerLinkJoint()
        {
            var controllerLinkJoint = gameObject.AddComponent<ConfigurableJoint>();
            controllerLinkJoint.autoConfigureConnectedAnchor = false;
            controllerLinkJoint.connectedAnchor = Vector3.zero;
            controllerLinkJoint.xMotion = ConfigurableJointMotion.Limited;
            controllerLinkJoint.yMotion = ConfigurableJointMotion.Limited;
            controllerLinkJoint.zMotion = ConfigurableJointMotion.Limited;
            controllerLinkJoint.linearLimit = new SoftJointLimit
            {
                limit = 0.65f,
                contactDistance = 0.1f
            };

            return controllerLinkJoint;
        }

        public void SetJointsActive(bool active)
        {
            if (active)
                CreateJoints();

            else
                DestroyJoints();
        }
    }
}