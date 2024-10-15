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
        private bool canTrigger = false;
        private bool startBlinking = false;
        private GameObject collidedGO;
        private int lifeCounter = 3;
        private BlockProperties blockProperties;


        /// <summary>
        /// Static Actions For Player Management
        /// </summary>
        public static Action PlayerFly;
        public static Action PlayerBackToSurface;
        public static Action GameStarted;
        public static PlayerManager Instance;
        public bool CanRightMove = false;

        public Action OnPlayerCrushed;
        public Action SpawnNewPlatform;
        public Action<Transform> OnBlockSpawn;
        public Action PlayerRun;

        public Transform getPos = null;
        public Transform coinNextPos;
        public ShakeData shakeData;

        int currentWaypoint = 0;

        private TransformProperty transformProperty;


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

            if (UImanager.Instance.CanMove)
            {
                speedParticle.SetActive(true);
                transform.Translate(transform.forward * 13 * Time.deltaTime, Space.World);
            }

            if (CanRightMove)
                DecideDirection(blockProperties, true);
            else if (!CanRightMove)
                DecideDirection(blockProperties, false);
        }

        public void DecideDirection(BlockProperties _blockProperties, bool _isRight)
        {
            if (_blockProperties != null)
            {
                if (currentWaypoint < GetTransformProperty<int>(TransformProperty.count, _blockProperties, _isRight))
                {
                    if (Vector3.Distance(this.transform.position, GetTransformProperty<Vector3>(TransformProperty.position, _blockProperties, _isRight)) <= 0.09f)
                    {
                        currentWaypoint++;
                        if (currentWaypoint == GetTransformProperty<int>(TransformProperty.count, _blockProperties, _isRight))
                        {
                            UImanager.Instance.CanMove = true;
                            //CanRightMove = false;
                            CanRightMove = !_isRight;
                            currentWaypoint = 0;
                        }
                    }
                    if (!UImanager.Instance.CanMove)
                    {
                        this.transform.position = Vector3.MoveTowards(this.transform.position, GetTransformProperty<Vector3>(TransformProperty.position, _blockProperties, _isRight), 10 * Time.deltaTime);
                        Quaternion target = GetTransformProperty<Quaternion>(TransformProperty.rotation, _blockProperties, _isRight);
                        this.transform.rotation = Quaternion.Lerp(this.transform.rotation, target, Time.deltaTime * 10);
                    }
                }
            }
        }

        public T GetTransformProperty<T>(TransformProperty setTransformProperty, BlockProperties block, bool isRight)
        {
            transformProperty = setTransformProperty;

            switch (transformProperty)
            {
                case TransformProperty.count:
                    if (isRight)
                        return (T)Convert.ChangeType(block.RightWayPoints.Count, typeof(T));
                    else
                        return (T)Convert.ChangeType(block.LeftWayPoints.Count, typeof(T));

                case TransformProperty.position:
                    if (isRight)
                        return (T)Convert.ChangeType(block.RightWayPoints[currentWaypoint].position, typeof(T));
                    else
                        return (T)Convert.ChangeType(block.LeftWayPoints[currentWaypoint].position, typeof(T));

                case TransformProperty.rotation:
                    if (isRight)
                        return (T)Convert.ChangeType(block.RightWayPoints[4].rotation, typeof(T));
                    else
                        return (T)Convert.ChangeType(block.LeftWayPoints[4].rotation, typeof(T));
            }
            return (T)Convert.ChangeType(block, typeof(T));
        }

        private void StartRunning()
        {
            controller.enabled = true;
            //lanesSystem.enabled = true;
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
            UImanager.Instance.CanMove = false;
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
            UImanager.Instance.CanMove = true;

            yield return new WaitForSeconds(2f);
            BlockSpeedController.Instance.OnBlockSpeed?.Invoke();
        }

        IEnumerator GameOver()
        {
            Anim.enabled = true;
            Anim.SetBool("Death", true);
            speedParticle.SetActive(false);
            UImanager.Instance.CanMove = false;
            BlockSpeedController.Instance.BlockSpeed = 0;
            yield return new WaitForSeconds(1.7f);

            UImanager.Instance.CanMove = false;
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

                case "NextBlockSpawnTrig":
                    DecideBlockSpawnTrig(other, LaneDirection.forward);
                    break;

                case "RightBlockSpawnTrig":
                    DecideBlockSpawnTrig(other, LaneDirection.right);
                    UImanager.Instance.CanMove = false;
                    CanRightMove = true;
                    break;

                case "LeftBlockSpawnTrig":
                    DecideBlockSpawnTrig(other, LaneDirection.left);
                    UImanager.Instance.CanMove = false;
                    CanRightMove = false;
                    break;

            }
        }
        void DecideBlockSpawnTrig(Collider collider, LaneDirection direction)
        {
            blockProperties = collider.gameObject.GetComponentInParent<BlockProperties>();
            blockProperties._LaneDirection = direction;
            Debug.Log("SpawnPos ::" + blockProperties.SpawnPos.localPosition +"Name : " +blockProperties.gameObject.name);
            StartCoroutine(DisableCollider(collider));
            OnBlockSpawn?.Invoke(blockProperties.SpawnPos);
        }

        IEnumerator DisableCollider(Collider collider)
        {
            collider.gameObject.SetActive(false);
            yield return new WaitForSeconds(1.5f);
            collider.gameObject.SetActive(true);
        }

        private void OnTriggerExit(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "NextBlockSpawnTrig":
                    break;
                case "LeftBlockSpawnTrig":
                    break;
                case "RightBlockSpawnTrig":
                    break;
                case "BlockDisableTrig":
                    blockProperties = other.gameObject.GetComponentInParent<BlockProperties>();
                    StartCoroutine(DisableBlock(blockProperties.gameObject, blockProperties.blockType));
                    break;
                default:
                    break;
            }
        }

        IEnumerator DisableBlock(GameObject block, PoolObjectType poolObjectType)
        {
            yield return new WaitForSeconds(1f);
            ObjectPool.OnReturningToPool(block, poolObjectType);
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