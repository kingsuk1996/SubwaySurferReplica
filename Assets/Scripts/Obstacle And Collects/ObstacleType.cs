using System.Collections.Generic;
using UnityEngine;
namespace RedApple.SubwaySurfer
{
    public enum ObstacleDirection { middle = 0, left = 1, right = 2 };
    public class ObstacleType : MonoBehaviour
    {
        [Space(10)]
        [Header("HeightDistance")]
        [SerializeField] private float heightDistanceForJumpObstacle = 1.5f;
        [SerializeField] private float heightDistanceForSlideObstacle = 0.03f;
        [SerializeField] private float obstacleYpos = 0.03f;

        [Space(10)]
        public SpawnObstacleType obstacleType;
        public ObstacleDirection ObstacleDir;
        public bool IsObstacle;

        private float left = -3.6f;
        private float right = +3.6f;
        private GameObject collidedObstacle;

        public static System.Action OnSetObsPos;

        private void OnEnable()
        {
            OnSetObsPos += SetObstaclePosition;
        }

        private void OnDisable()
        {
            OnSetObsPos -= SetObstaclePosition;
        }

        private void SetObstaclePosition()
        {
            switch (obstacleType)
            {
                case SpawnObstacleType.Slide:
                    ChangeObstaclePosition();
                    break;
                case SpawnObstacleType.Jump:
                    ChangeObstaclePosition();
                    break;
                case SpawnObstacleType.Car:
                    ChangeObstaclePosition();
                    break;
                case SpawnObstacleType.LeftTurn:
                    ChangeObstaclePosition();
                    break;
                case SpawnObstacleType.RightTurn:
                    ChangeObstaclePosition();
                    break;
                case SpawnObstacleType.Coin:
                    break;
                default:
                    break;
            }
        }

        private void ChangeObstaclePosition()
        {
            transform.position = new Vector3(transform.position.x, obstacleYpos, transform.position.z);
        }


        private void OnTriggerEnter(Collider other)
        {
            GameObject collisionobj = other.gameObject;
            switch (collisionobj.tag)
            {
                case "Jumpable":
                    if (collisionobj.GetComponent<ObstacleType>() != null)
                    {
                        SpawnObstacleType spawnObstacleType = collisionobj.GetComponent<ObstacleType>().obstacleType;
                        if (spawnObstacleType == SpawnObstacleType.Jump)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, heightDistanceForJumpObstacle, this.transform.position.z);
                        }
                    }
                    break;

                case "Slider":
                    if (collisionobj.GetComponent<ObstacleType>() != null)
                    {
                        SpawnObstacleType spawnObstacleType = collisionobj.GetComponent<ObstacleType>().obstacleType;
                        if (spawnObstacleType == SpawnObstacleType.Slide)
                        {
                            this.transform.position = new Vector3(this.transform.position.x, heightDistanceForSlideObstacle, this.transform.position.z);
                        }
                    }
                    break;

                case "Car":
                    if (collisionobj.GetComponent<ObstacleType>() != null)
                    {
                        SpawnObstacleType spawnObstacleType = collisionobj.GetComponent<ObstacleType>().obstacleType;
                        if (spawnObstacleType == SpawnObstacleType.Car)
                        {
                            ObstacleDirection obstaclepos = collisionobj.GetComponent<ObstacleType>().ObstacleDir;
                            if (obstaclepos == ObstacleDirection.middle)
                            {
                                float z = Random.Range(left, right) > 0 ? left : right;
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, z);
                            }
                            else if (obstaclepos == ObstacleDirection.left)
                            {
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, 0);
                            }
                            else if (obstaclepos == ObstacleDirection.right)
                            {
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, 0);
                            }
                        }
                    }
                    break;

                case "TurnLeft":
                    if (collisionobj.GetComponent<ObstacleType>() != null)
                    {
                        SpawnObstacleType spawnObstacleType = collisionobj.GetComponent<ObstacleType>().obstacleType;
                        if (spawnObstacleType == SpawnObstacleType.LeftTurn)
                        {
                            ObstacleDirection obstaclepos = collisionobj.GetComponent<ObstacleType>().ObstacleDir;
                            if (obstaclepos == ObstacleDirection.right)
                            {
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, 0);
                            }
                            else if (obstaclepos == ObstacleDirection.middle)
                            {
                                float z = Random.Range(left, right) > 0 ? left : right;
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, z);
                            }
                        }
                    }
                    break;

                case "TurnRight":
                    if (collisionobj.GetComponent<ObstacleType>() != null)
                    {
                        SpawnObstacleType spawnObstacleType = collisionobj.GetComponent<ObstacleType>().obstacleType;
                        if (spawnObstacleType == SpawnObstacleType.RightTurn)
                        {
                            ObstacleDirection obstaclepos = collisionobj.GetComponent<ObstacleType>().ObstacleDir;
                            if(obstaclepos == ObstacleDirection.left)
                            {
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, 0);
                            }
                            else if(obstaclepos == ObstacleDirection.middle)
                            {
                                float z = Random.Range(left, right) > 0 ? left : right;
                                this.transform.position = new(this.transform.position.x, this.transform.position.y, z);
                            }
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        public void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Obs")
            {
                collidedObstacle = null;
            }
        }

    }
}
