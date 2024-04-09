using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Effects
{
    public class Returnable : MonoBehaviour
    {
        [SerializeField]
        float resetTime = 1.0f;

        [SerializeField]
        Transform returnTransform;

        private void Start()
        {
            if (returnTransform == null)
                returnTransform = transform.parent;
        }

        public void BeginResetPosition()
        {
            StopCoroutine("ResetPosition");
            StartCoroutine("ResetPosition");
        }

        IEnumerator ResetPosition()
        {
            //transform.rotation = Quaternion.identity;

            float t = 0.0f;

            float oldX = transform.position.x;
            float oldY = transform.position.y;
            float oldZ = transform.position.z;

            float oldPitch = transform.rotation.x;
            float oldYaw = transform.rotation.y;
            float oldRoll = transform.rotation.z;

            float newX = 0.0f;
            float newY = 0.0f;
            float newZ = 0.0f;

            float newPitch = 0.0f;
            float newYaw = 0.0f;
            float newRoll = 0.0f;

            while (t < resetTime)
            {
                newX = UtilMath.EasingFunction.EaseOutExpo(oldX, returnTransform.position.x, t);
                newY = UtilMath.EasingFunction.EaseOutExpo(oldY, returnTransform.position.y, t);
                newZ = UtilMath.EasingFunction.EaseOutExpo(oldZ, returnTransform.position.z, t);

                newPitch = UtilMath.EasingFunction.EaseOutExpo(oldPitch, returnTransform.rotation.x, t);
                newYaw = UtilMath.EasingFunction.EaseOutExpo(oldYaw, returnTransform.rotation.y, t);
                newRoll = UtilMath.EasingFunction.EaseOutExpo(oldRoll, returnTransform.rotation.z, t);

                transform.position = new Vector3(newX, newY, newZ);
                transform.rotation = new Quaternion(newPitch, newYaw, newRoll, 1.0f);

                yield return null;

                t += Time.deltaTime;
            }
        }

        public void Duplicate()
        {
            Instantiate(gameObject, returnTransform.position, returnTransform.rotation);
        }

        public void DestroyAfter(float time)
        {
            Destroy(gameObject, time);
        }
    }
}
