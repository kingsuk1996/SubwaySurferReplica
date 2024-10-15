//@Author Sabyasachi Thakur
using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public class Car : MonoBehaviour
    {
        [SerializeField] private float CarSpeed;
        [SerializeField] private float PathCheckingDistance;

        [SerializeField] private Transform PathChecker;

        private RaycastHit[] ObjectsInPath;
        private void Update()
        {
            //if (PlayerManager.GameON)
            //{
            //    MoveCar();
            //}
            DisableOtherObjectsInPath();
        }
        private void MoveCar()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * CarSpeed);
        }
        private void DisableOtherObjectsInPath()
        {
            ObjectsInPath = Physics.RaycastAll(PathChecker.position, PathChecker.forward, PathCheckingDistance);
            if (ObjectsInPath != null)
            {
                foreach (RaycastHit obj in ObjectsInPath)
                {
                    if (obj.collider.tag == "Obs" || obj.collider.tag == "Slider")
                    {
                        obj.transform.gameObject.SetActive(false);
                    }

                }
            }
           
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(PathChecker.position, PathChecker.forward);
        }
    }
}