using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class RunnerBot : MonoBehaviour
    {
        [SerializeField] private float MaxViewDistance;
        [SerializeField] private float FollowSpeed;
        [SerializeField] private float MoveTowardsSpeed;
        [SerializeField] private PlayerManager Manager;
        [Tooltip("Start position behind the player")]
        [SerializeField] private Vector3 GameStartPosition;
        [Tooltip("Vector3 offset from player when player crush into things")]
        [SerializeField] private Vector3 OffsetOnPlayerCrushed;
        [SerializeField] BotStates States;
        #region Unity Monobehaviour
        private void Start()
        {
            States = BotStates.Standby;
        }
        private void Update()
        {
            StateHandler();
        }
        private void OnEnable()
        {
            //PlayerManager.GameStarted += OnGameStarted;
            //PlayerManager.PlayerCrushed += OnPlayerCrushed;
        }
        private void OnDisable()
        {
            //PlayerManager.GameStarted -= OnGameStarted;
            //PlayerManager.PlayerCrushed -= OnPlayerCrushed;
        }
        #endregion
        private void StateHandler()
        {
            switch (States)
            {
                case BotStates.Standby:
                    break;
                case BotStates.FollowingPlayer:
                    RunForward();
                    FollowPlayer(Manager.transform.position);
                    CheckForOutOfScreen();
                    break;
                case BotStates.MovingTowardsPlayer:
                    MoveToPlayer();
                    break;
                case BotStates.OutOfScreen:
                    break;
                case BotStates.AttackingPlayer:
                    Attack();
                    break;
            }
        }

        #region State Behaviours
        private void MoveToPlayer()
        {
            transform.position = Vector3.MoveTowards(transform.position, Manager.transform.position, MoveTowardsSpeed * Time.deltaTime);
            if (transform.position == Manager.transform.position)
            {
                States = BotStates.AttackingPlayer;
            }
        }
        private void FollowPlayer(Vector3 PlayerPosition)
        {
            transform.position = new Vector3(transform.position.x, PlayerPosition.y, PlayerPosition.z);
        }
        private void RunForward()
        {
            transform.Translate(Vector3.forward * Time.deltaTime * FollowSpeed);
        }
        private void CheckForOutOfScreen()
        {
            float distFromPlayer = Vector3.Distance(transform.position,Manager.transform.position);
            if (distFromPlayer > MaxViewDistance)
            {
                States= BotStates.OutOfScreen;
            }
        }
        private void Attack()
        {
            this.GetComponent<Animator>().SetBool("PlayerCrushed", true);
        }
        #endregion

        #region On Event Functions
        private void OnPlayerCrushed()
        {
            if(States == BotStates.OutOfScreen)
            {
                transform.position = Manager.transform.position + OffsetOnPlayerCrushed;
                Debug.Log("Called");
            }
            States = BotStates.MovingTowardsPlayer;
            this.GetComponent<CapsuleCollider>().enabled= false;
        }
        private void OnGameStarted()
        {
            States = BotStates.FollowingPlayer;
            transform.position = GameStartPosition;
        }
        #endregion
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, MaxViewDistance);
        }
        
    }
    public enum BotStates
    {
        Standby,
        FollowingPlayer,
        MovingTowardsPlayer,
        OutOfScreen,
        AttackingPlayer
    }
}