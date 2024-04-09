using com.AlteredRealityLabs.ArcaneAdventures.Components.Creatures;
using com.AlteredRealityLabs.ArcaneAdventures.Combat;
using UnityEngine;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    public class ProjectileAimAssister : MonoBehaviour
    {
        public GameObject target { get; set; } = null;

        [SerializeField]
        Rigidbody projectileRigidbody;

        private float rotateStepMax => CombatSettings.Spells.MaxAimAssistAmount;
        private float rotateStepMin => CombatSettings.Spells.MinAimAssistAmount;

        private float AssistRange => CombatSettings.Spells.AimAssistRange;

        float assistAmount = 0.0f;

        private float AssistRadiusFraction => CombatSettings.Spells.AimAssistWidthFraction;

        private void Start()
        {
            projectileRigidbody = transform.parent.GetComponentInParent<Rigidbody>();

            float assistWidth = AssistRange / AssistRadiusFraction;
            Vector3 assistScale = new Vector3(assistWidth, assistWidth, AssistRange);

            transform.parent.localScale = assistScale;

            GetComponent<MeshRenderer>().enabled = CombatSettings.Spells.ShowTargetingCollider;
        }

        private void Update()
        {
            if (target == null)
                return;

            assistAmount = UtilMath.Lmap((projectileRigidbody.transform.position - target.transform.position).magnitude, 0.0f, AssistRange, rotateStepMin, rotateStepMax);

            projectileRigidbody.velocity = Vector3.RotateTowards(projectileRigidbody.velocity, (target.transform.position - projectileRigidbody.transform.position).normalized, assistAmount * Time.deltaTime, 0.0f);
        }

        //TODO: Reenable for aim assist based on projectile collider. 
        private void OnTriggerEnter(Collider other)
        {
            if (target != null || !CombatSettings.Spells.UseAutomaticTargeter)
                return;

            if (other.transform == transform.parent)
                return;

            if (other.GetComponentInParent<CreatureReference>() != null)
                target = other.gameObject;
        }
    }
}