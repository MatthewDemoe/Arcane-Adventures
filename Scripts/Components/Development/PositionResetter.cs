using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class PositionResetter : MonoBehaviour
    {
        [SerializeField] private float resetTimeInSeconds;
        [SerializeField] private float resetDistance;

        private Vector3 initialPosition;
        private Rigidbody rigidBody;
        private bool isResetCounterStarted;
        private float timeUntilReset;

        private void Awake()
        {
            initialPosition = transform.position;
            rigidBody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (isResetCounterStarted)
            {
                timeUntilReset -= Time.fixedDeltaTime;

                if (timeUntilReset < 0)
                {
                    rigidBody.position = initialPosition;
                    rigidBody.rotation = Quaternion.Euler(0, 0, 0);
                    rigidBody.velocity = Vector3.zero;
                    rigidBody.angularVelocity = Vector3.zero;
                    isResetCounterStarted = false;
                }

                return;
            }

            var distanceFromInitialPosition = Vector3.Distance(initialPosition, rigidBody.position);

            if (distanceFromInitialPosition > resetDistance)
            {
                timeUntilReset = resetTimeInSeconds;
                isResetCounterStarted = true;
            }
        }
    }
}