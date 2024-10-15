using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace RedApple.SubwaySurfer
{

    [System.Serializable]
    public class PoolInfo
    {
        public PoolObjectType type;
        public int amount = 0;
        public GameObject[] prefab;
        public Transform container;

        [HideInInspector]
        public List<GameObject> pool = new List<GameObject>();
    }
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<PoolInfo> listOfPool;

        public static Action<GameObject, PoolObjectType> OnReturningToPool;
        public static Func<PoolObjectType, GameObject> OnFetchingFromPool;

        private void Start()
        {
            for (int i = 0; i < listOfPool.Count; i++)
            {
                FillPool(listOfPool[i]);
            }
        }

        private void OnEnable()
        {
            //OnFetchingFromPool += GetObjectFromPool;
            OnReturningToPool += ReturnObjectToPool;
        }

        private void OnDisable()
        {
            //OnFetchingFromPool -= GetObjectFromPool;
            OnReturningToPool -= ReturnObjectToPool;
        }

        void FillPool(PoolInfo info)
        {
            for (int i = 0; i < info.amount; i++)
            {
                GameObject ob = null;
                ob = Instantiate(info.prefab[UnityEngine.Random.Range(0, info.prefab.Length)], info.container.transform);
                ob.SetActive(false);
                ob.transform.position = Vector3.zero;
                info.pool.Add(ob);
            }
        }

        public GameObject GetObjectFromPool(PoolObjectType type)
        {
            PoolInfo selected = GetPoolByType(type);
            List<GameObject> pool = selected.pool;

            GameObject ob = null;
            if (pool.Count > 0)
            {
                ob = pool[pool.Count - 1];
                pool.Remove(ob);
            }
            else
                ob = Instantiate(selected.prefab[UnityEngine.Random.Range(0,selected.prefab.Length)], selected.container.transform);

            return ob;
        }

        private PoolInfo GetPoolByType(PoolObjectType type)
        {
            for (int i = 0; i < listOfPool.Count; i++)
            {
                if (type == listOfPool[i].type)
                    return listOfPool[i];
            }
            return null;
        }

        public void ReturnObjectToPool(GameObject obj, PoolObjectType type)
        {
            obj.SetActive(false);
            obj.transform.position = Vector3.zero;

            PoolInfo selected = GetPoolByType(type);
            List<GameObject> pool = selected.pool;

            if (!pool.Contains(obj))
                pool.Add(obj);
        }
    }
}
