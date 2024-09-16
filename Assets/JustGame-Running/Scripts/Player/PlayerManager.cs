using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using FirstGearGames.SmoothCameraShaker;

namespace JustGame.SubwaySurfer
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private PlayerController playerController;

        [Space(10)]
        [SerializeField] private List<SkinnedMeshRenderer> playerMesh;

        [Space(10)]
        [SerializeField] private float fallingGravityScale = 40;
        [SerializeField] private Rigidbody playerRB;
        [SerializeField] private Animator playerAnim;

        [Space(10)]
        [SerializeField] private RectTransform targetRect_Coin;
        [SerializeField] private float durationOfCoinAnimation = 1f;

        [Space(10)]
        [SerializeField] private ShakeData shakeData;

        [Space(10)]
        [SerializeField] private ParticleSystem coinCollect;
        [SerializeField] private ParticleSystem playerCollisionEffect;
        [SerializeField] private ParticleSystem speedEffect;

        [Space(10)]
        public byte lifeCounter = 3;
        public Transform blockSpawnPoint;

        private float time = 0;
        private float gravityScale = -9.81f;
        private int coinCount = 0;

        private bool startBlinking = false;
        private bool once = false;

        private float playerBlinkingTimer;
        private float playerBlinkingTotalTimer;
        private float playerBlinkingTotalDuration;
        private float playerBlinkingMiniDuration;

        public static Action OnBlockSpawn;

        internal void Init()
        {
            playerBlinkingTimer = gameSettings.PlayerBlinkingTimer;
            playerBlinkingTotalTimer = gameSettings.PlayerBlinkingTotalTimer;
            playerBlinkingTotalDuration = gameSettings.PlayerBlinkingTotalDuration;
            playerBlinkingMiniDuration = gameSettings.PlayerBlinkingMiniDuration;
        }

        private void Update()
        {
            if (GameConstants.CurrentGamePlayState == GamePlayState.Play)
            {
                BlockSpawnOverDistance();
                UpdatePlayerGravity();
                DistanceCalculation();
            }

            if (lifeCounter > 0)
            {
                if (startBlinking == true)
                {
                    PlayerBlinkingEffect();
                }
            }
        }

        private void DistanceCalculation()
        {
            time += Time.deltaTime;
            float distance = (time * BlockProperties.Instance.CurrentBlockSpeed);

            UIManager.Instance.SetDistance(distance);
        }

        private void UpdatePlayerGravity()
        {
            if (playerRB.velocity.y > 0)
            {
                Physics.gravity = new Vector3(0, gravityScale, 0);
                playerRB.mass = 0.3f;
            }
            else if (playerRB.velocity.y < -0.1f)
            {
                Physics.gravity = new Vector3(0, -fallingGravityScale, 0);
                playerRB.mass = 10f;
            }
        }

        private void BlockSpawnOverDistance()
        {
            if ((transform.position.z - blockSpawnPoint.transform.position.z) <= 0.5f && !once)
            {
                once = true;
                OnBlockSpawn?.Invoke();
                once = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CheckTag checkTrigger))
            {
                switch (checkTrigger._tag)
                {
                    case TagType.Obstacle:
                        UIManager.Instance.AudioController(AudioType.Hurt);
                        CameraShakerHandler.Shake(shakeData);
                        playerCollisionEffect.Play();
                        speedEffect.Stop();
                        PlayerLifeSystem(collision.gameObject);
                        break;

                    case TagType.Ground:
                        Physics.gravity = new Vector3(0, gravityScale, 0);
                        playerController.canJump = true;
                        playerController.canSlide = true;
                        playerAnim.SetBool("Jump", false);
                        break;
                }
            }
        }

        private void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CheckTag checkTrigger))
            {
                switch (checkTrigger._tag)
                {
                    case TagType.Ground:
                        playerController.canSlide = true;
                        break;
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out CheckTag checkTrigger))
            {
                switch (checkTrigger._tag)
                {
                    case TagType.Ground:
                        playerController.canSlide = false;
                        break;
                }
            }
        }


        private void PlayerLifeSystem(GameObject _collidedGO)
        {
            _collidedGO.SetActive(false);
            startBlinking = true;
            Handheld.Vibrate();
            lifeCounter -= 1;
            playerAnim.SetBool("Jump", false);
            playerAnim.SetBool("Slide", false);
            playerAnim.enabled = false;
            //UIManager.Instance.UpdateLifeSystem(lifeCounter);

            if (lifeCounter == 0)
            {
                GameManager.Instance.ChangePanelState(PanelStates.GameOver);
                speedEffect.Stop();
            }
            else if (lifeCounter > 0)
            {
                GameConstants.CurrentGamePlayState = GamePlayState.OnCollision;
                StartCoroutine(GameStartAfterCollision(_collidedGO));
            }
        }

        IEnumerator GameStartAfterCollision(GameObject gameObject)
        {
            yield return new WaitForSeconds(2f);
            if (GameConstants.CurrentGamePlayState != GamePlayState.Pause)
            {
                playerAnim.enabled = true;
                playerAnim.SetTrigger("Run");
                GameConstants.CurrentGamePlayState = GamePlayState.Play;
            }
            yield return new WaitForSeconds(1.7f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent(out CheckTag checkTrigger))
            {
                switch (checkTrigger._tag)
                {
                    case TagType.Coin:
                        UIManager.Instance.AudioController(AudioType.CoinCollect);
                        coinCollect.Play();

                        coinCount++;
                        GameConstants.Score = coinCount;
                        UIManager.Instance.CurrentCoinCollection.text = coinCount.ToString();
                        other.gameObject.SetActive(false);

                        GameObject _coin = ObjectPool.OnFetchingFromPool(PoolObjectType.Coin);
                        _coin.SetActive(true);
                        RectTransform _coinRect = _coin.GetComponent<RectTransform>();
                        _coinRect.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
                        _coinRect.DOAnchorPos(targetRect_Coin.anchoredPosition, durationOfCoinAnimation).OnComplete(() => ObjectPool.OnReturningToPool(_coin, PoolObjectType.Coin));
                        break;
                }
            }
        }

        private void PlayerBlinkingEffect()
        {
            playerBlinkingTotalTimer += Time.deltaTime;
            if (playerBlinkingTotalTimer >= playerBlinkingTotalDuration)
            {
                startBlinking = false;
                playerBlinkingTotalTimer = 0.0f;
                foreach (var item in playerMesh)
                {
                    item.enabled = true;
                }
                return;
            }

            playerBlinkingTimer += Time.deltaTime;
            if (playerBlinkingTimer >= playerBlinkingMiniDuration)
            {
                playerBlinkingTimer = 0.0f;
                foreach (var item in playerMesh)
                {
                    if (item.enabled)
                    {
                        item.enabled = false;
                    }
                    else
                    {
                        item.enabled = true;
                    }
                }
            }
        }
    }
}