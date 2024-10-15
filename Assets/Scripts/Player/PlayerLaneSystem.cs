using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace RedApple.SubwaySurfer
{
    public class PlayerLaneSystem : MonoBehaviour
    {
        public Lanes PlayerLanes;
        [SerializeField] private Hights PlayerHights;
        [SerializeField] private PlayerController controller;

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
            if (UImanager.Instance.CanMove)
            {
                //PlayerLaneHandler();
                //InputForPlayerLaneChange();
                PlayerHightHandler();
            }
        }

        #region PlayerLanesHandelling
        IEnumerator PlayerLaneHandler(float thresold)
        {
            UImanager.Instance.CanMove = false;
            Vector3 targetpos = transform.position + transform.right * thresold + transform.forward;
            while (this.transform.position != targetpos)
            {
                yield return Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position,targetpos, LaneChangeSpeed*Time.deltaTime );
            }
            UImanager.Instance.CanMove = true;
        }


       /*void InputForPlayerLaneChange()
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
        }*/

        void GoLeft()
        {
            if (controller.coroutine != null)
            {
                StopCoroutine(controller.coroutine);
            }
            controller.coroutine = StartCoroutine(controller.EnableTest());

            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    PlayerLanes = Lanes.Left;
                    break;
                case Lanes.Right:
                    PlayerLanes = Lanes.Middle;
                    break;
            }
            StartCoroutine(PlayerLaneHandler(-RightLanePos));
        }

        void GoRight()
        {
            if (controller. coroutine != null)
            {
                StopCoroutine(controller.coroutine);
            }
            controller. coroutine = StartCoroutine(controller.EnableTest());

            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    PlayerLanes = Lanes.Right;
                    break;
                case Lanes.Left:
                    PlayerLanes = Lanes.Middle;
                    break;
            }
            StartCoroutine(PlayerLaneHandler(RightLanePos));
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