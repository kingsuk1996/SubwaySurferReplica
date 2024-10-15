using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class ObstacleSpawnInBlock : MonoBehaviour
    {
        private ObjectPool objectPool;
        private TypeOfObstacle typeOfObstacle;
        private int row = 3;
        private int column = 4;

        public PoolObjectType GetObjectType
        {
            get
            {
                return ReturnObstacle();
            }
        }

        private PoolObjectType ReturnObstacle()
        {
            if (typeOfObstacle == TypeOfObstacle.StaticObs)
            {
                return (PoolObjectType)Random.Range(10, 12);
            }
            else if (typeOfObstacle == TypeOfObstacle.BlankObs)
            {
                return PoolObjectType.LeftTurn;
            }
            else if (typeOfObstacle == TypeOfObstacle.MoveableObs)
            {
                return PoolObjectType.Car;
            }
            else
            {
                return PoolObjectType.RightTurn;
            }
        }

        private void Start()
        {
            
        }
       
    }
}
