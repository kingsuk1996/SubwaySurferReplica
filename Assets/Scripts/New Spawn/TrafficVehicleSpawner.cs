using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RedApple.SubwaySurfer
{
    public class TrafficVehicleSpawner : MonoBehaviour
    {
        public static Action<Vector3> OnSpawnObstacle;
        [SerializeField] private ObjectPool objectPool;

        GameObject car;

        private void OnEnable()
        {
            OnSpawnObstacle += SpawnObstacles;
        }

        private void OnDisable()
        {
            OnSpawnObstacle -= SpawnObstacles;
        }

        public void SpawnObstacles(Vector3 spawnPos)
        {

            int pickRandomObstacle = UnityEngine.Random.Range(0, 5);
            switch(pickRandomObstacle)
            {
                case 0:
                    FetchObstacleFromPool(1.3f, spawnPos, PoolObjectType.Slide);
                    break;

                case 1:
                    FetchObstacleFromPool(1.3f, spawnPos, PoolObjectType.Jump);
                    break;

                case 2:
                    FetchObstacleFromPool(1.3f, spawnPos, PoolObjectType.Car);
                    break;

                case 3:
                    FetchObstacleFromPool(1.75f, spawnPos, PoolObjectType.LeftTurn);
                    break;

                case 4:
                    FetchObstacleFromPool(1.44f, spawnPos, PoolObjectType.RightTurn);
                    break;

            }
        }

        void FetchObstacleFromPool(float Y_offset, Vector3 spawnPosition, PoolObjectType poolObjectType)
        {

            car = objectPool.GetObjectFromPool(poolObjectType);
            car.transform.position = spawnPosition + new Vector3(0, Y_offset, 0);
            car.SetActive(true);
        }
    }
}
