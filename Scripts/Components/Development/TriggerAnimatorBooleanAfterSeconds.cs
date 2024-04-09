using System.Collections;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class TriggerAnimatorBooleanAfterSeconds : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private string parameterName;
        [SerializeField] private float delay;

        private void Start()
        {
            StartCoroutine(FlipBoolSwitch());
        }

        private IEnumerator FlipBoolSwitch()
        {
            yield return new WaitForSeconds(delay);
            var value = animator.GetBool(parameterName);
            animator.SetBool(parameterName, !value);
            StartCoroutine(FlipBoolSwitch());
        }
    }
}