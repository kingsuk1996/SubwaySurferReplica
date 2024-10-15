using System.Collections;
using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public class DogController : MonoBehaviour
    {
        [SerializeField] private Animator dogAnim;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float raycastDistance = 1f;

        private Rigidbody rb;
        public static System.Action OnStart;

        private bool canDogMove = false;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (canDogMove)
            {
                transform.position += Vector3.right * Time.deltaTime * gameSettings.DogSpeed;
                dogAnim.SetTrigger("Run");
                Jump();
            }
        }

        private void StartMove()
        {
            canDogMove = true;
        }

        private void Jump()
        {
            RaycastHit hit;
            Debug.DrawRay(transform.position, transform.forward * raycastDistance, Color.red);
            if (Physics.Raycast(transform.position, transform.forward, out hit, raycastDistance))
            {
                if (hit.collider.tag.Equals("Car") || hit.collider.tag.Equals("Jumpable"))
                {
                    dogAnim.SetTrigger("Jump");
                    rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }
                rb.velocity = Vector3.zero;
            }
        }
        private void OnEnable()
        {
            OnStart += StartMove;
        }

        private void OnDisable()
        {
            OnStart -= StartMove;
        }
    }
}
