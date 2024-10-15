using UnityEngine;
using UnityEngine.UI;
namespace RedApple.SubwaySurfer
{
    public class PlayerLaneSystem : MonoBehaviour
    {
        [SerializeField] private Lanes PlayerLanes;
        [SerializeField] private Hights PlayerHights;
        [SerializeField] private PlayerController controller;
        [SerializeField] private Animator Anim;

        [Space(10)]
        [SerializeField] float MidLanePos;
        [SerializeField] float LeftLanePos;
        [SerializeField] float RightLanePos;
        [SerializeField] float LaneChangeSpeed;

        [Space(10)]
        [SerializeField] float DownHight;
        [SerializeField] float UpHight;
        [SerializeField] float HightChangeSpeed;

        [Space(10)]
        private bool LeftInput = false;
        private bool RightInput = false;


        private void Start()
        {
            PlayerLanes = Lanes.Middle;
            PlayerHights = Hights.Down;
        }

        private void Update()
        {
            if (UImanager.CanMove)
            {
                PlayerLaneHandler();
                InputForPlayerLaneChange();
               
                PlayerHightHandler();
            }
        }

        #region PlayerLanesHandelling
        void PlayerLaneHandler()
        {
            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    MoveToNextLane(MidLanePos);
                   
                    break;
                case Lanes.Left:
                    MoveToNextLane(LeftLanePos);
                   
                    break;
                        
                case Lanes.Right:
                    MoveToNextLane(RightLanePos);
                  
                    break;
            }
        }


        void InputForPlayerLaneChange()
        {
            LeftInput = Input.GetKeyDown(KeyCode.A);
            RightInput = Input.GetKeyDown(KeyCode.D);
            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    if (LeftInput)
                    {
                        PlayerLanes = Lanes.Left;
                    }
                    if (RightInput)
                    {
                        PlayerLanes = Lanes.Right;
                    }
                    break;
                case Lanes.Left:
                    if (RightInput)
                    {
                        PlayerLanes = Lanes.Middle;
                    }
                    break;
                case Lanes.Right:
                    if (LeftInput)
                    {
                        PlayerLanes = Lanes.Middle;
                    }
                    break;
            }
        }

       
        void MoveToNextLane(float LanePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, LanePos), LaneChangeSpeed*Time.deltaTime);
        }

        void GoLeft()
        {
          
            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    PlayerLanes = Lanes.Left;
                    break;
                case Lanes.Right:
                    PlayerLanes = Lanes.Middle;
                    break;
            }
        }

        void GoRight()
        {
           
            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    PlayerLanes = Lanes.Right;
                    break;
                case Lanes.Left:
                    PlayerLanes = Lanes.Middle;
                    break;
            }
        }

        #endregion

        #region Player Hight Handelling
        void PlayerHightHandler()
        {
            switch (PlayerHights)
            {
                case Hights.Down:
                    break;
                case Hights.Up:
                    MoveToNextHight(UpHight);
                    break;
            }
        }

        void MoveToNextHight(float Hight)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, Hight, transform.position.z), HightChangeSpeed);
        }

        void MovetoFlyLane()
        {
            PlayerHights = Hights.Up;
        }

        void MoveToSurfaceLane()
        {
            PlayerHights= Hights.Down;
        }
        #endregion

        private void OnEnable()
        {
            PlayerManager.PlayerFly += MovetoFlyLane;
            PlayerManager.PlayerBackToSurface += MoveToSurfaceLane;
            controller.GoLeft += GoLeft;
            controller.GoRight += GoRight;
        }

        private void OnDisable()
        {
            PlayerManager.PlayerFly -= MovetoFlyLane;
            PlayerManager.PlayerBackToSurface-= MoveToSurfaceLane;
            controller.GoLeft -= GoLeft;
            controller.GoRight -= GoRight;
        }
    }
}