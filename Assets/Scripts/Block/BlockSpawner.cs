using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject lastBlock;

        [Space(10)]
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private PlayerManager player_Manager;
        [SerializeField] private GameSettings gameSetting;

        public List<GameObject> blockList = new List<GameObject>();
        PoolObjectType ObjectType;

        private void Start()
        {
            //lastBlockTransform.GetChild(0).gameObject.SetActive(false);
            //lastBlockTransform.gameObject.GetComponent<ObjectSpawnerInBlock>().canSpawn = true;
        }

        /* private void SpawnBlocks()
         {
             int blockCount = Random.Range(gameSetting.MinSpawnCount, gameSetting.MaxSpawnCount);
             int tempBlockType = Random.Range(0, 10);
             for (int i = 0; i < blockCount; i++)
             {
                 GameObject tempBlock = objectPool.GetObjectFromPool((BlockType)tempBlockType);
                 //tempBlock.transform.GetComponent<ObjectSpawnerInBlock>().canSpawn = true;

                 blockList.Add(tempBlock);

                 if (tempBlock != null)
                 {
                     BlockProperties bm = tempBlock.GetComponent<BlockProperties>();
                     tempBlock.transform.position = new Vector3(0 , 0, lastBlockTransform.position.z + bm.xLength );
                     tempBlock.SetActive(true);
                     lastBlockTransform = tempBlock.transform;
                 }

                 for (int j = 1; j < blockList.Count; j++)
                 {
                     blockList[j].transform.GetChild(0).gameObject.SetActive(false);
                 }
             }
             blockList.Clear();
         }*/


        private void SpawnNewBlock(Transform _getSpawnPos)
        {
            int randomBlock = Random.Range(0, 10);

            switch (randomBlock)
            {
                case 0:
                    GetBlockFromPool(PoolObjectType.City1, _getSpawnPos);
                    break;
                case 1:
                    GetBlockFromPool(PoolObjectType.City2, _getSpawnPos);
                    break;
                case 2:
                    GetBlockFromPool(PoolObjectType.City3, _getSpawnPos);
                    break;
                case 3:
                    GetBlockFromPool(PoolObjectType.Crossing1, _getSpawnPos);
                    break;
                case 4:
                    GetBlockFromPool(PoolObjectType.Crossing2, _getSpawnPos);
                    break;
                case 5:
                    GetBlockFromPool(PoolObjectType.Urban1, _getSpawnPos);
                    break;
                case 6:
                    GetBlockFromPool(PoolObjectType.Urban2, _getSpawnPos);
                    break;
                case 7:
                    GetBlockFromPool(PoolObjectType.Bridge1, _getSpawnPos);
                    break;
                case 8:
                    GetBlockFromPool(PoolObjectType.Bridge2, _getSpawnPos);
                    break;
                case 9:
                    GetBlockFromPool(PoolObjectType.Tunnel, _getSpawnPos);
                    break;
                default:
                    break;
            }
        }

        private void GetBlockFromPool(PoolObjectType poolObjectType, Transform _blockSpawnPos)
        {
            ObjectType = poolObjectType;
            GameObject tempBlock = objectPool.GetObjectFromPool(poolObjectType);
            blockList.Add(tempBlock);
            if (tempBlock != null)
            {
                tempBlock.SetActive(true);
                Transform getBlockSpawnPos = _blockSpawnPos;
                tempBlock.transform.position = getBlockSpawnPos.position;
                tempBlock.transform.rotation = getBlockSpawnPos.rotation;
            }
        }

        private void OnEnable()
        {
            //player_Manager.SpawnNewPlatform += SpawnBlocks;
            player_Manager.OnBlockSpawn += SpawnNewBlock;
        }

        private void OnDisable()
        {
            //player_Manager.SpawnNewPlatform -= SpawnBlocks;
            player_Manager.OnBlockSpawn -= SpawnNewBlock;
        }
    }


}