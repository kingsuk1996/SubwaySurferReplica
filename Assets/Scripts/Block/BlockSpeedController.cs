using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class BlockSpeedController : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        private Coroutine coroutine;
        public float BlockSpeed;

        public Action OnBlockSpeed;
        public static BlockSpeedController Instance;


        private void Awake()
        {
            Instance = this;
        }

        public void EnableCoroutine()
        {
            coroutine = StartCoroutine(SpeedIncrease());
        }


        IEnumerator SpeedIncrease()
        {
            while (BlockSpeed < gameSettings.MaxSpeed)
            {
                yield return new WaitForSeconds(gameSettings.SpeedIncreaseDelay);
                BlockSpeed += gameSettings.SpeedMultiplier;
            }
        }

        private void OnEnable()
        {
            OnBlockSpeed += EnableCoroutine;
        }

        private void OnDisable()
        {
            OnBlockSpeed -= EnableCoroutine;
        }
    }
}
