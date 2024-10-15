using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RedApple.SubwaySurfer
{
    public class TrafficVehicle : MonoBehaviour
    {
        public float speed = 2f;
        public float wheelRotateSpeed = 5f;

        public GameObject[] wheels;

        [SerializeField] VehicleType type;

        void Update()
        {
            MoveVehicle();
            AnimateWheels();
        }

        void MoveVehicle()
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        void AnimateWheels()
        {
            for(int i=0; i<wheels.Length; i++)
            {
                wheels[i].transform.Rotate(0, 0, -wheelRotateSpeed * Time.deltaTime);
            }        
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.gameObject.tag == "Player")
            {
                switch (type)
                {
                    case VehicleType.VehicleSet1:
                        ObjectPool.OnReturningToPool(gameObject, PoolObjectType.Slide);
                        break;

                    case VehicleType.VehicleSet2:
                        ObjectPool.OnReturningToPool(gameObject, PoolObjectType.Jump);
                        break;

                    case VehicleType.VehicleSet3:
                        ObjectPool.OnReturningToPool(gameObject, PoolObjectType.Car);
                        break;

                    case VehicleType.VehicleSet4:
                        ObjectPool.OnReturningToPool(gameObject, PoolObjectType.LeftTurn);
                        break;

                    case VehicleType.VehicleSet5:
                        ObjectPool.OnReturningToPool(gameObject, PoolObjectType.RightTurn);
                        break;

                }
            }
        }
    }
}
