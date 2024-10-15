using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class ObjectPool : MonoBehaviour
    {
        [SerializeField] private List<GameObject> ObjInPool = new List<GameObject>();
        [SerializeField] private GameSettings gameSettings;

        public List<GameObject> BlockPrefab;


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

        public GameObject ReturnInactiveBlock(BlockType blockType)
        {
            GameObject tempGObj = null;
            BlockType block;
            for (int i = 0; i < ObjInPool.Count; i++)
            {
                block = ObjInPool[i].GetComponent<BlockMovement>().BlockType;
                if (!ObjInPool[i].activeInHierarchy && block == blockType)
                {
                    tempGObj = ObjInPool[i];
                    tempGObj.SetActive(true);
                    return tempGObj;
                }
            }
            return tempGObj;
        }

        public GameObject ReturnActiveBlock()
        {
            for (int i = 0; i < ObjInPool.Count; i++)
            {
                if (ObjInPool[i].activeInHierarchy)
                {
                    GameObject gameObject = ObjInPool[i];
                }
            }
            return gameObject;
        }

       
    }
}

