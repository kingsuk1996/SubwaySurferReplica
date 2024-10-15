using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class BlockProperties : MonoBehaviour
    {
        [SerializeField] private Transform nextBlockSpawnPos;
        [SerializeField] private Transform rightBlockSpawnPos;
        [SerializeField] private Transform leftBlockSpawnPos;
        private LaneDirection laneDirection;
        public List<Transform> RightWayPoints = new List<Transform>();
        public List<Transform> LeftWayPoints = new List<Transform>();
        public PoolObjectType blockType;

        public LaneDirection _LaneDirection
        {
            get
            {
                return laneDirection;
            }
            set
            {
                laneDirection = value;
            }
        }


        public Transform SpawnPos
        {
            get
            {
                return ReturnSpawnPos();
            }
        }


        Transform ReturnSpawnPos()
        {
            if (laneDirection == LaneDirection.forward)
                return nextBlockSpawnPos;
            else if (laneDirection == LaneDirection.left)
                return leftBlockSpawnPos;
            else if (laneDirection == LaneDirection.right)
                return rightBlockSpawnPos;
            else
                return null;
        }
    }
}
