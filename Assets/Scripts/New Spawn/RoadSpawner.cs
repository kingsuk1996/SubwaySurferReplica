using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RedApple.SubwaySurfer
{
    public class RoadSpawner : MonoBehaviour
    {
        public static Action OnRoadSpawn;

        public TMP_Text speedText;

        public GameObject currentRoad;
        GameObject spawnedRoad { get; set; }

        private void OnEnable()
        {
            OnRoadSpawn += SpawnRoad;
        }
        private void OnDisable()
        {
            OnRoadSpawn -= SpawnRoad;
        }

        void SpawnRoad()
        {
            //int pickRandomRoad = UnityEngine.Random.Range(0, 2);

            //switch(pickRandomRoad)
            //{
            //    case 0:
            //        FetchRoadFromPool(PoolObjectType.road1);
            //        break;

            //    case 1:
            //        FetchRoadFromPool(PoolObjectType.road2);
            //        break;
            //}

            //FetchRoadFromPool(PoolObjectType.road1);

        }

        void FetchRoadFromPool(PoolObjectType poolObjectType)
        {
            spawnedRoad = ObjectPool.OnFetchingFromPool?.Invoke(poolObjectType);
            spawnedRoad.transform.position = currentRoad.transform.GetChild(0).position;
            currentRoad = spawnedRoad;
            currentRoad.SetActive(true);
        }
    }
}
