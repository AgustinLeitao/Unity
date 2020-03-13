using UnityEngine;

namespace Utilitarios
{
    public class PlayerMovement : MonoBehaviour
    {
        public float speed = 6f;     
        private Rigidbody playerRigidbody;
        private bool isWalking;
        private AudioSource m_AudioSource;
        private float m_StepCycle;
        private float m_NextStep;
        [SerializeField] private AudioClip[] m_FootstepSounds;

        void Start()
        {
            playerRigidbody = GetComponent<Rigidbody>();
            m_AudioSource = GetComponent<AudioSource>();
            if (GameObject.Find("Opciones") != null)
                m_AudioSource.volume = GameObject.Find("Opciones").GetComponent<OpcionesDelJuego>().volumenEfectos;
        }

        void FixedUpdate()
        {          
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            isWalking = !(h == 0 && v == 0);      
            
            Move(h, v);

            if(isWalking)
                ProgressStepCycle(speed);
        }

        private void ProgressStepCycle(float speed)
        {
            if (playerRigidbody.velocity.sqrMagnitude > 0 && (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0))
            {
                m_StepCycle += (playerRigidbody.velocity.magnitude + (speed * 1f)) * Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + 2.5f;

            PlayFootStepAudio();
        }

        private void PlayFootStepAudio()
        {                 
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);          
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }

        void Move(float h, float v)
        {
            var forward = Camera.main.transform.TransformDirection(Vector3.forward);                    
            var right = new Vector3(forward.z, 0, -forward.x);
            var moveDirection = (h * right + v * forward);
            var movement = moveDirection * speed * 15 * Time.deltaTime;
            var moveFinal = new Vector3(movement.x, playerRigidbody.velocity.y, movement.z);

            playerRigidbody.velocity = moveFinal;
        }                
    }
}