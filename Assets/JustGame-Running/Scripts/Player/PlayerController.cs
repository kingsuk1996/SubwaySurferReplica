using System;
using System.Collections;
using UnityEngine;

namespace JustGame.SubwaySurfer
{
    public class PlayerController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private float midLanePos;
        [SerializeField] private float leftLanePos;
        [SerializeField] private float rightLanePos;
        [Space(10)]

        [Range(1, 50)]
        [SerializeField] private float laneChangeSpeed;
        [Range(1, 50)]
        [SerializeField] private float jumpForce;

        [Space(10)]
        [SerializeField] private float SlideTime;
        [SerializeField] private float JumpTime;

        [Space(10)]
        [SerializeField] private Animator playerAnim;
        [SerializeField] private Rigidbody playerRB;
        [SerializeField] private CapsuleCollider playerCollider;

        [Space(10)]
        [SerializeField] private float playerCrouchHeight = .63f;
        [SerializeField] private float playerCrouchY = .284f;

        private float playerStandHeight;
        private bool TakeMousePos;
        private Vector3 centerOfCapsuleCollider;
        private Vector2 firstPressPos;
        private Vector2 lastPressPos;
        private Vector2 currentSwipe;

        internal bool canSlide = false;
        internal bool canJump = false;

        private Lanes PlayerLanes = Lanes.Middle;

        internal void Init()
        {
            PlayerLanes = Lanes.Middle;
            playerStandHeight = playerCollider.height;
            centerOfCapsuleCollider = playerCollider.center;
        }

        private void Update()
        {
            if (GameConstants.CurrentGamePlayState == GamePlayState.Play)
            {
                MouseInput();
                KeyBoardInput();
                //TouchInput();
                PlayerLaneHandler();
            }
        }

        private void KeyBoardInput()
        {
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Goleft();
                playerAnim.SetTrigger("LeanLeft");
            }
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Goright();
                playerAnim.SetTrigger("LeanRight");
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                Jump();
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                Slide();
            }
        }

        private void MouseInput()
        {
            if (Input.mousePosition.y >= 0 && Input.mousePosition.y <= (1920 * 0.87f))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    firstPressPos = Input.mousePosition;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    lastPressPos = Input.mousePosition;
                    currentSwipe = new Vector2(lastPressPos.x - firstPressPos.x, lastPressPos.y - firstPressPos.y);

                    //normalize the 2d vector
                    currentSwipe.Normalize();

                    //swipe up
                    if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Jump();
                    }

                    //swipe down
                    if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                    {
                        Slide();
                        GoDown();
                    }

                    //swipe left
                    if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        if (PlayerLanes == Lanes.Right || PlayerLanes == Lanes.Middle)
                        {
                            Goleft();
                            playerAnim.SetTrigger("LeanLeft");
                        }
                    }

                    //swipe right
                    if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                    {
                        if (PlayerLanes == Lanes.Left || PlayerLanes == Lanes.Middle)
                        {
                            Goright();
                            playerAnim.SetTrigger("LeanRight");
                        }
                    }
                }
            }
        }

        private void TouchInput()
        {
            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    TakeMousePos = true;
                    firstPressPos = touch.position;
                    lastPressPos = touch.position;
                }

                else if (touch.phase == TouchPhase.Moved && TakeMousePos)
                {

                    lastPressPos = touch.position;
                    if (Mathf.Abs(lastPressPos.x - firstPressPos.x) > Mathf.Abs(lastPressPos.y - firstPressPos.y)) //If the horizontal movement is greater than the vertical movement...
                    {   
                        if ((lastPressPos.x > firstPressPos.x))
                        {
                            Goright();
                            playerAnim.SetTrigger("LeanRight");
                        }
                        else if ((lastPressPos.x < firstPressPos.x))
                        {  
                            Goleft();
                            playerAnim.SetTrigger("LeanLeft");
                        }
                        TakeMousePos = false;
                    }
                    else if (Mathf.Abs(lastPressPos.y - firstPressPos.y) > Mathf.Abs(lastPressPos.x - firstPressPos.x)) //the vertical movement is greater than the horizontal movement
                    {   
                        if (lastPressPos.y > firstPressPos.y)
                        {
                            Jump();
                        }
                        else if (lastPressPos.y < firstPressPos.y)
                        {
                            Slide();
                        }
                        TakeMousePos = false;
                    }
                    else
                    {
                        if (lastPressPos.x > firstPressPos.x)
                        {
                            Goright();
                            playerAnim.SetTrigger("LeanRight");
                        }
                        else if (lastPressPos.x < firstPressPos.x)
                        {
                            Goleft();
                            playerAnim.SetTrigger("LeanLeft");
                        }
                        else if (lastPressPos.y > firstPressPos.y)  //If the movement was up
                        {
                            Jump();
                        }
                        else if (lastPressPos.y < firstPressPos.y)
                        { 
                            Slide();
                        }
                        TakeMousePos = false;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {

                    firstPressPos = Vector2.zero;
                    lastPressPos = Vector2.zero;
                }
            }
        }

        private void Goleft()
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

        private void Goright()
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

        void PlayerLaneHandler()
        {
            switch (PlayerLanes)
            {
                case Lanes.Middle:
                    MoveToNextLane(midLanePos);
                    break;

                case Lanes.Left:
                    MoveToNextLane(leftLanePos);
                    break;

                case Lanes.Right:
                    MoveToNextLane(rightLanePos);
                    break;
            }
        }

        void MoveToNextLane(float _nextLanePos)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_nextLanePos, transform.position.y, transform.position.z), laneChangeSpeed * Time.deltaTime);
        }

        private void GoDown()
        {
            if (!canJump)
            {
                playerRB.velocity = (Vector3.down * jumpForce);
            }
        }

        private void Jump()
        {
            if (canJump)
            {
                playerAnim.SetBool("Jump", true);
                playerRB.velocity = (Vector3.up * jumpForce);
                canJump = false;
                //StartCoroutine(JumpDelay());
            }
        }

        private IEnumerator JumpDelay()
        {
            yield return new WaitForSeconds(JumpTime);
            playerAnim.SetBool("Jump", false);
        }

        private void Slide()
        {
            if (canSlide)
            {
                playerAnim.SetBool("Slide", true);
                canSlide = false;
                playerCollider.height = playerCrouchHeight;
                playerCollider.center = new Vector3(centerOfCapsuleCollider.x, playerCrouchY, centerOfCapsuleCollider.z);
                StartCoroutine(SlideDelay());
            }
        }

        private IEnumerator SlideDelay()
        {
            yield return new WaitForSeconds(SlideTime);
            playerAnim.SetBool("Slide", false);
           // canSlide = true;
            yield return new WaitForSeconds(.4f);
            playerCollider.height = playerStandHeight;
            playerCollider.center = centerOfCapsuleCollider;
        }
    }
}