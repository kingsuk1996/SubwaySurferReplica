using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class BlockSpawner : MonoBehaviour
    {
        [SerializeField] private Transform lastBlockTransform;
        [SerializeField] private GameObject lastBlock;

        [Space(10)]
        [SerializeField] private ObjectPool objectPool;
        [SerializeField] private PlayerManager player_Manager;
        [SerializeField] private GameSettings gameSetting;

        private List<GameObject> blockList = new List<GameObject>();


        private void Start()
        {
            lastBlockTransform.GetChild(0).gameObject.SetActive(false);
            lastBlockTransform.gameObject.GetComponent<ObjectSpawnerInBlock>().canSpawn = true;
        }

        private void SpawnBlocks()
        {
            int blockCount = Random.Range(gameSetting.MinSpawnCount, gameSetting.MaxSpawnCount);
            int tempBlockType = Random.Range(0, 10);
            for (int i = 0; i < blockCount; i++)
            {
                GameObject tempBlock = objectPool.ReturnInactiveBlock((BlockType)tempBlockType);
                tempBlock.transform.GetComponent<ObjectSpawnerInBlock>().canSpawn = true;

                blockList.Add(tempBlock);

                if (tempBlock != null)
                {
                    BlockMovement bm = tempBlock.GetComponent<BlockMovement>();
                    tempBlock.transform.position = new Vector3(lastBlockTransform.transform.position.x + bm.xLength, 0, 0);
                    tempBlock.SetActive(true);
                    lastBlockTransform = tempBlock.transform;
                }

                for (int j = 1; j < blockList.Count; j++)
                {
                    blockList[j].transform.GetChild(0).gameObject.SetActive(false);
                }
            }
            blockList.Clear();
        }


        private void OnEnable()
        {
            player_Manager.SpawnNewPlatform += SpawnBlocks;
        }

        private void OnDisable()
        {
            player_Manager.SpawnNewPlatform -= SpawnBlocks;
        }
    }


}