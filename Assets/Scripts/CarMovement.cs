using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class CarMovement : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        [SerializeField] private float distanceOffset = 15f;
        private float distance = 16f;

        private bool canMove;

       
        private void Update()
        {
            canMove = true;
            RaycastHit hit;
            Debug.DrawRay(transform.position, -transform.forward * distance);
            if (Physics.Raycast(transform.position, -transform.forward, out hit, distance))
            {
                if (hit.collider.tag == "Jumpable" || hit.collider.tag == "Slider" || hit.collider.tag == "TurnRight" || hit.collider.tag == "TurnLeft")
                {
                    canMove = false;
                }
            }
            if (Vector3.Distance(this.transform.position, PlayerManager.Instance.transform.position) <= distanceOffset && canMove && UImanager.CanMove)
            {
                this.transform.Translate(Vector3.back * speed * Time.deltaTime);
            }
        }


    }
}