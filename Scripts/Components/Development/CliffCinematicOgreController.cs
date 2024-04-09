using UnityEngine;
using UnityEngine.VFX;

namespace com.AlteredRealityLabs.ArcaneAdventures.Components.Development
{
    public class CliffCinematicOgreController : MonoBehaviour
    {
        [SerializeField] private Transform roarAudioSourceTarget;
        [SerializeField] private AudioSource roarAudioSource;
        [SerializeField] private float roarFromZPosition;
        [SerializeField] private GameObject spitSpray;

        private Animator animator;
        private bool isRoaring;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            animator.Play("Walk_Forward", 0);
        }

        private void Update()
        {
            if (!isRoaring && transform.position.z < roarFromZPosition)
                StartRoar();
        }

        private void StartRoar()
        {
            animator.CrossFade("Roar_KickStarter", 0.25f, 0);
            roarAudioSource.transform.position = roarAudioSourceTarget.transform.position;
            roarAudioSource.PlayOneShot(roarAudioSource.clip);
            
            isRoaring = true;
        }

        public void PlayRoar()
        {
            spitSpray.SetActive(true);
        }

        public void StopRoar()
        {
            spitSpray.transform.parent = null;
            spitSpray.GetComponentInChildren<VisualEffect>().SetBool("Loop", false);
        }
    }
}