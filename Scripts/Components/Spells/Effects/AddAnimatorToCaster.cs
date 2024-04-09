using com.AlteredRealityLabs.ArcaneAdventures.Components.Common;
using UnityEngine;
using UnityEngine.Events;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Spells.Effects
{
    [RequireComponent(typeof(SpellAnimatorPropertySetter))]
    public class AddAnimatorToCaster : MonoBehaviour, ISpellReferencer 
    {
        [SerializeField]
        RuntimeAnimatorController animatorControllerToAdd;

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

                caster = physicalSpell.spellCaster.gameObject;
                casterRigidBody = caster.GetComponent<Rigidbody>();
            }
        }

        GameObject caster;
        GameObject newParent;

        Rigidbody casterRigidBody;

        bool animatorStopped = false;

        public Animator animator { get; private set; }
        public UnityEvent OnAnimationInitialize = new UnityEvent();
        public UnityEvent OnAnimationEnd = new UnityEvent();

        public void StartAnimator()
        {
            newParent = Instantiate(new GameObject(), caster.transform.position, caster.transform.rotation);
            caster.transform.parent = newParent.transform;

            animator = newParent.AddComponent<Animator>();
            animator.runtimeAnimatorController = animatorControllerToAdd;
            animator.applyRootMotion = true;

            GetComponent<SpellAnimatorPropertySetter>().SetAnimatorRange();

            AnimationEventInvoker animationEndEventInvoker = newParent.AddComponent<AnimationEventInvoker>();
            
            animationEndEventInvoker.OnAnimationEnd.AddListener(StopAnimator);
            animationEndEventInvoker.OnAnimationInitialize.AddListener(OnAnimationInitialize.Invoke);

            SelfDestroyer selfDestroyer = GetComponent<SelfDestroyer>();

            animationEndEventInvoker.OnAnimationEnd.AddListener(selfDestroyer.DestroySelfAfterDelay);

            casterRigidBody.useGravity = false;
        }

        public void StopAnimator()
        {
            if (animatorStopped)
                return;

            OnAnimationEnd.Invoke();

            animatorStopped = true;

            animator.StopPlayback();

            newParent.transform.DetachChildren();
            casterRigidBody.useGravity = true;

            Destroy(newParent);
        }
    }
}