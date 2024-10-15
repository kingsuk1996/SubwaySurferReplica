using DevCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class BlockProperties : Singleton<BlockProperties>
    {
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private GameSettings gameSettings;

        [Space(10)]
        [SerializeField] private Transform lastBlockTransform;


        private List<GameObject> blockList = new List<GameObject>();
        private float currentBlockSpeed;
        public static Action OnBlockSpeed;

        private Coroutine speedIncreaseCoroutine;

        public float CurrentBlockSpeed { get => currentBlockSpeed; set => currentBlockSpeed = value; }

        protected override void Awake()
        {
            base.Awake();
            CurrentBlockSpeed = gameSettings.InitialBlockSpeed;
        }

        private void BlockSpawn()
        {
            int maxBlockSpawn = UnityEngine.Random.Range(3, 5);
            PoolObjectType _getRandomBlock = (PoolObjectType)(UnityEngine.Random.Range(0, 10));

            for (int i = 0; i < maxBlockSpawn; i++)
            {
                GameObject _currentBlock = ObjectPool.OnFetchingFromPool(_getRandomBlock);
                blockList.Add(_currentBlock);

                if (_currentBlock != null)
                {
                    _currentBlock.transform.position = new Vector3(0, 0, lastBlockTransform.position.z + gameSettings.Zoffset);
                    _currentBlock.SetActive(true);
                    lastBlockTransform = _currentBlock.transform.GetChild(0);

                    _currentBlock.GetComponent<ObstacleSetSpawner>().Init();
                }

                playerManager.blockSpawnPoint = blockList[0].transform.GetChild(0).transform;
            }
            blockList.Clear();
        }

        private void EnableCoroutine()
        {
            speedIncreaseCoroutine = StartCoroutine(SpeedIncrease());
        }

        private IEnumerator SpeedIncrease()
        {
            while (CurrentBlockSpeed < gameSettings.MaxSpeed)
            {
                yield return new WaitForSeconds(gameSettings.SpeedIncreaseDelay);
                CurrentBlockSpeed += gameSettings.SpeedMultiplier;
            }
        }

        private void OnEnable()
        {
            PlayerManager.OnBlockSpawn += BlockSpawn;
            OnBlockSpeed += EnableCoroutine;
        }

        private void OnDisable()
        {
            PlayerManager.OnBlockSpawn -= BlockSpawn;
            OnBlockSpeed -= EnableCoroutine;
        }
    }
}