using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class WheelRotator : MonoBehaviour
    {
        [SerializeField] private List<GameObject> WheelList;
        [SerializeField] float wheelSpeed = 10f;
        [SerializeField] float carSpeed = 10f;
        public CarMovingDirection movingDirection;
        BlockType blockType;

        private void Start()
        {
            blockType = this.transform.parent.gameObject.GetComponent<BlockMovement>().BlockType;
        }
        void Update()
        {
            if (UImanager.CanMove)
            {
                //if (blockType == BlockType.CrossRoad)
                //{
                //    switch (movingDirection)
                //    {
                //        case CarMovingDirection.Forward:
                //            ChangePosition(Vector3.forward);
                //            break;
                //        case CarMovingDirection.Backward:
                //            ChangePosition(Vector3.back);
                //            break;
                //    }
                //}
                //foreach (GameObject wheel in WheelList)
                //{
                //    wheel.transform.Rotate(1 * wheelSpeed * Time.deltaTime, 0, 0);
                //}
            }
        }

        void ChangePosition(Vector3 dir)
        {
            transform.position += dir * Time.deltaTime * carSpeed;
        }
    }

    
    public enum CarMovingDirection
    {
        Forward,
        Backward
    }
}
