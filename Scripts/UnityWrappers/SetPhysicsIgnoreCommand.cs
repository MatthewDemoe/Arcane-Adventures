using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.UnityWrappers
{
    public class SetPhysicsIgnoreCommand
    {
        private Collider[] collidersA;
        private Collider[] collidersB;
        private bool ignore;
        private float remainingDelay;
        public bool hasBeenUsedAtLeastOnce { get; private set; } = false;
        public bool isSet { get; private set; } = false;

        public void Set(Collider[] collidersA, Collider[] collidersB, bool ignore, float delay = 0)
        {
            this.collidersA = collidersA;
            this.collidersB = collidersB;
            this.ignore = ignore;
            remainingDelay = delay;
            isSet = true;
        }

        public void Reuse(bool invert = false, float delay = 0)
        {
            if (!hasBeenUsedAtLeastOnce)
            {
                throw new System.Exception("Cannot reuse command that has never been used");
            }

            if (invert)
            {
                ignore = !ignore;
            }

            remainingDelay = delay;
            isSet = true;
        }

        public bool TryExecute(float timePassed = 0)
        {
            if (!isSet)
            {
                throw new System.Exception("Command is not set");
            }

            remainingDelay -= timePassed;

            if (remainingDelay > 0)
            {
                return false;
            }

            foreach (var colliderA in collidersA)
            {
                foreach (var colliderB in collidersB)
                {
                    if (colliderA != null && colliderB != null)
                    {
                        Physics.IgnoreCollision(colliderA, colliderB, ignore);
                    }
                }
            }

            hasBeenUsedAtLeastOnce = true;
            isSet = false;

            return true;
        }
    }
}