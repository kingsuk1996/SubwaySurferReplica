using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class ObjectPooler : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ObjInPool = new List<GameObject>();
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private List<GameObject> BlockPrefab;


        void Awake()
        {
            CreateObjectPool();
        }

        private void CreateObjectPool()
        {
            GameObject gameObject;
            for (int j = 0; j < gameSettings.MaxObect; j++)
            {
                for (int i = 0; i < BlockPrefab.Count; i++)
                {
                    gameObject = Instantiate(BlockPrefab[i]);
                    gameObject.SetActive(false);
                    gameObject.transform.parent = transform;
                    ObjInPool.Add(gameObject);
                }
            }
        }

       /* public GameObject GetObjectFromPool(BlockType blockType)
        {
            GameObject tempGObj = null;
            BlockType block;
            for (int i = 0; i < ObjInPool.Count; i++)
            {
                block = ObjInPool[i].GetComponent<BlockProperties>().blockType;
                if (!ObjInPool[i].activeInHierarchy && block == blockType)
                {
                    tempGObj = ObjInPool[i];
                    tempGObj.SetActive(true);
                    return tempGObj;
                }
               
            }
            return tempGObj;
        }*/
    }
}

