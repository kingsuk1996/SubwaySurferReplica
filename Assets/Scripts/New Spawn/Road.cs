using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class Road : MonoBehaviour
    {
        int pickRandomLane;

        private void Start()
        {
            //SpawnOnRandomLane();
        }
        private void OnEnable()
        {
            SpawnOnRandomLane();
        }

        void SpawnOnRandomLane()
        {
            for (int i = 1; i <= 2; i++)
            {
                pickRandomLane = Random.Range(0, 3);

                switch (pickRandomLane)
                {
                    case 0:
                        SpawnVehicle(3.5f, 10f, 20f);
                        break;

                    case 1:
                        SpawnVehicle(0f, 20f, 20f);
                        break;

                    case 2:
                        SpawnVehicle(-3.5f, 7f, 20f);
                        break;
                }
            }
        }

        void SpawnVehicle(float X_offset, float initial_Z_Offset, float final_Z_Offset)
        {
            float offset = initial_Z_Offset;
            //Debug.Log("1st offset" +offset);
            //for (int i = 1; i < 5; i++)
            //{
                Vector3 spawnPos = new Vector3(X_offset, 0f, this.transform.position.z + offset);
                offset += final_Z_Offset;
            //Debug.Log("last offset" + offset);
            //TrafficVehicleSpawner.OnSpawnObstacle?.Invoke(spawnPos);
            //}
        }
       
    }
}
