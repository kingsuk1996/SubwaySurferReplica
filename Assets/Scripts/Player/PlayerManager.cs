using System;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class PlayerManager : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private PlayerController controller;
        [SerializeField] private PlayerLaneSystem lanesSystem;
        [SerializeField] private GameSettings gameSettings;
        [SerializeField] private CoinManager coinManager;
        [SerializeField] private CoinPooling coinPool;

        [Space(10)]
        [SerializeField] private Rigidbody PlayerRB;
        [SerializeField] private ConstantForce PlayerDownForce;
        [SerializeField] private Animator Anim;
        [SerializeField] private ParticleSystem deadParticle;
        [SerializeField] private ParticleSystem coinParticle;
        [SerializeField] private GameObject speedParticle;
        [SerializeField] private List<SkinnedMeshRenderer> playerMesh;
        [SerializeField] private Transform coin2DParent;
        [SerializeField] private float coinCounter = 0;
        [Space(10)]

        private bool once = true;
        private bool canTrigger = true;
        private bool startBlinking = false;
        private GameObject collidedGO;
        private int lifeCounter = 3;

        /// <summary>
        /// Static Actions For Player Management
        /// </summary>
        public static Action PlayerFly;
        public static Action PlayerBackToSurface;
        public static Action GameStarted;
        public static Action OnPlayerCrushed;
        public static PlayerManager Instance;

        public Action SpawnNewPlatform;
        public Action PlayerRun;

        public Transform coinNextPos;
        public ShakeData shakeData;


        private void Awake()
        {
            Instance = this;
            speedParticle.SetActive(false);
        }

        private void Start()
        {
            deadParticle.Stop();
            coinParticle.Stop();
            UImanager.Instance.coinCounter.text = coinCounter.ToString();
        }

        private void Update()
        {
            if (Input.touchCount == 1 || Input.GetMouseButtonDown(0))
            {
                if (once)
                {
                    once = false;
                    GameStarted?.Invoke();
                    PlayerRun?.Invoke();
                }
            }

            if (lifeCounter > 0)
            {
                if (startBlinking == true)
                {
                    PlayerBlinkingEffect();
                }
            }

            if (UImanager.CanMove)
            {
                speedParticle.SetActive(true);
            }
        }

        private void StartRunning()
        {
            controller.enabled = true;
            lanesSystem.enabled = true;
        }


        private void OnCollisionEnter(Collision collision)
        {
            switch (collision.gameObject.tag)
            {
                case "Jumpable":
                    LifeCalculation();
                    collidedGO = collision.gameObject;
                    break;
                case "Slider":
                    LifeCalculation();
                    collidedGO = collision.gameObject;
                    break;
                case "Car":
                    LifeCalculation();

                    collidedGO = collision.gameObject;
                    break;
                case "TurnLeft":
                    LifeCalculation();
                    collidedGO = collision.gameObject;
                    break;
                case "TurnRight":
                    LifeCalculation();
                    collidedGO = collision.gameObject;
                    break;
            }
        }


        private void LifeCalculation()
        {
            startBlinking = true;
            UImanager.CanMove = false;
            AudioManager.Instance.PlayOneShot("HurtSound");
            lifeCounter -= 1;
            UImanager.Instance.LifeSystem(lifeCounter);
            deadParticle.Play();
            speedParticle.SetActive(false);
            Anim.enabled = false;

            CameraShakerHandler.Shake(shakeData);
            Handheld.Vibrate();

            if (lifeCounter != 0)
            {
                StartCoroutine(PlayerRespawn());
            }
            else if (lifeCounter == 0)
            {
                StartCoroutine(GameOver());
            }
        }

        IEnumerator PlayerRespawn()
        {
            yield return new WaitForSeconds(.1f);

            collidedGO.SetActive(false);
            yield return new WaitForSeconds(2);

            Anim.enabled = true;
            speedParticle.SetActive(true);
            BlockSpeedController.Instance.BlockSpeed = 20;
            UImanager.CanMove = true;
            yield return new WaitForSeconds(2f);

            BlockSpeedController.OnBlockSpeed?.Invoke();
        }

        IEnumerator GameOver()
        {
            Anim.enabled = true;
            Anim.SetBool("Death", true);
            speedParticle.SetActive(false);
            UImanager.CanMove = false;
            BlockSpeedController.Instance.BlockSpeed = 0;
            yield return new WaitForSeconds(1.7f);

            UImanager.CanMove = false;
            OnPlayerCrushed?.Invoke();
        }


        private void PlayerBlinkingEffect()
        {
            gameSettings.playerBlinkingTotalTimer += Time.deltaTime;
            if (gameSettings.playerBlinkingTotalTimer >= gameSettings.playerBlinkingTotalDuration)
            {
                startBlinking = false;
                gameSettings.playerBlinkingTotalTimer = 0.0f;
                foreach (var item in playerMesh)
                {
                    item.enabled = true;
                }
                return;
            }

            gameSettings.playerBlinkingTimer += Time.deltaTime;
            if (gameSettings.playerBlinkingTimer >= gameSettings.playerBlinkingMiniDuration)
            {
                gameSettings.playerBlinkingTimer = 0.0f;
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

        private void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "SpawnTrig":
                    if (canTrigger)
                    {
                        canTrigger = false;
                        SpawnNewPlatform?.Invoke();
                    }
                    break;

                case "Coin":
                    AudioManager.Instance.PlayOneShot("CoinCollect");
                    coinCounter++;
                    coinParticle.Play();
                    UImanager.Instance.TotalCoin = coinCounter;
                    UImanager.Instance.coinCounter.text = coinCounter.ToString();
                    other.gameObject.SetActive(false);

                    GameObject g = coinPool.ReturnInactiveCoin();
                    g.SetActive(true);
                    RectTransform rect = g.GetComponent<RectTransform>();
                    rect.transform.position = Camera.main.WorldToScreenPoint(other.transform.position);
                    coinManager.MoveCoin(rect);
                    break;
            }
        }
        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "SpawnTrig":
                    canTrigger = true;
                    break;

                case "Coin":
                    break;
            }
        }


        private void TakeFlight()
        {
            Debug.Log("Player Should Fly Now");
            PlayerRB.useGravity = false;
            PlayerDownForce.enabled = false;
            StartCoroutine(FlyTime());
        }

        private void BackToSurface()
        {
            Debug.Log("Player Should be back to surface");
            PlayerRB.useGravity = true;
            PlayerDownForce.enabled = true;
        }

        IEnumerator FlyTime()
        {
            yield return new WaitForSeconds(5);
            PlayerBackToSurface?.Invoke();
        }

        private void OnEnable()
        {
            PlayerRun += StartRunning;
            PlayerFly += TakeFlight;
            PlayerBackToSurface += BackToSurface;
        }

        private void OnDisable()
        {
            PlayerRun -= StartRunning;
            PlayerFly -= TakeFlight;
            PlayerBackToSurface -= BackToSurface;
        }
    }
}