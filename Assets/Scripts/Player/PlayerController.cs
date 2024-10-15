//@Author Sabyasachi Thakur
using System;
using System.Collections;
using UnityEngine;

namespace RedApple.SubwaySurfer
{
    public class PlayerController : MonoBehaviour
    {
        [Space(10)]
        [SerializeField] private float JumpForce;
        [SerializeField] private float SlideTime;
        [SerializeField] private float JumpTime;

        [Space(10)]
        [SerializeField] private Rigidbody rb;
        [SerializeField] private CapsuleCollider PlayerColliderNormal;
        [SerializeField] private BoxCollider PlayercolliderSlide;
        [SerializeField] private Animator Anim;
        [SerializeField] private PlayerLaneSystem Playerlanes;
        [SerializeField] private GameSettings gameSettings;

        [Space(10)]
        [SerializeField] private GameObject followCam;
        [SerializeField] private GameObject introCam;
        [SerializeField] private GameObject playerShadow;

        private float totalDistance = 0;
        private float time;

        private bool JumpInput = false;
        private bool canJump = true;
        private bool TakeMousePos = true;
        private bool CanSlide = true;

        private Vector2 firstPos;   //First touch position
        private Vector2 lastPos;   //Last touch position

        public Action GoLeft;
        public Action GoRight;
        public static Action OnStart;


        private void Start()
        {
            introCam.SetActive(true);
            followCam.SetActive(false);
            playerShadow.SetActive(false);
            PlayerColliderNormal = GetComponent<CapsuleCollider>();
            PlayercolliderSlide = GetComponent<BoxCollider>();
        }

        void StartRun()
        {
            UImanager.CanMove = true;
            Anim.SetTrigger("Run");
            playerShadow.SetActive(true);
            followCam.SetActive(true);
            introCam.SetActive(false);
        }
        private void Update()
        {
            if (UImanager.CanMove)
            {
                time += Time.deltaTime;
                SwipeInput();
                JumpInput = Input.GetKey(KeyCode.Space);
                totalDistance = MathF.Round((time * BlockSpeedController.Instance.BlockSpeed));// - BlockSpeedController.Instance.BlockSpeed);
                UpdateDistance(totalDistance);
            }

        }

        public void UpdateDistance(float _distance)
        {
            UImanager.Instance.TotalDistance = _distance;
            UImanager.Instance.Distance.text = _distance.ToString();
        }

        private void FixedUpdate()
        {

            if (JumpInput)
            {
                Jump();
            }
        }

        private void SwipeInput()
        {
            if (Input.GetKey(KeyCode.S))
            {
                Slide();
            }

            if (Input.touchCount >= 1)
            {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    TakeMousePos = true;
                    firstPos = touch.position;
                    lastPos = touch.position;
                }

                else if (touch.phase == TouchPhase.Moved && TakeMousePos)
                {

                    lastPos = touch.position;
                    if (Mathf.Abs(lastPos.x - firstPos.x) > Mathf.Abs(lastPos.y - firstPos.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lastPos.x > firstPos.x))
                        {   //Right swipe

                            Anim.SetTrigger("Lean Right");
                            GoRight?.Invoke();

                        }
                        else if ((lastPos.x < firstPos.x))
                        {   //Left swipe

                            Anim.SetTrigger("Lean Left");
                            GoLeft?.Invoke();
                        }
                        TakeMousePos = false;
                    }
                    else if (Mathf.Abs(lastPos.y - firstPos.y) > Mathf.Abs(lastPos.x - firstPos.x))
                    {   //the vertical movement is greater than the horizontal movement
                        if (lastPos.y > firstPos.y)  //If the movement was up
                        {   //Up swipe

                            Jump();
                        }
                        else if (lastPos.y < firstPos.y)
                        {   //Down swipe

                            Slide();
                        }
                        TakeMousePos = false;
                    }
                    else
                    {
                        if (lastPos.x > firstPos.x)
                        {   //Right swipe

                            Anim.SetTrigger("Lean Right");
                            GoRight?.Invoke();

                        }
                        else if (lastPos.x < firstPos.x)
                        {   //Left swipe

                            Anim.SetTrigger("Lean Left");
                            GoLeft?.Invoke();
                        }
                        else if (lastPos.y > firstPos.y)  //If the movement was up
                        {   //Up swipe

                            Jump();
                        }
                        else if (lastPos.y < firstPos.y)
                        {   //Down swipe

                            Slide();
                        }
                        TakeMousePos = false;
                    }
                }

                if (touch.phase == TouchPhase.Ended)
                {

                    firstPos = Vector2.zero;
                    lastPos = Vector2.zero;
                }
            }
        }

        private void Jump()
        {
            //if (canJump && CanSlide)
            if (canJump)
            {
                Anim.SetBool("Jump", true);
                rb.AddForce(Vector3.up * JumpForce, ForceMode.Impulse);
                canJump = false;
                StartCoroutine(JumpDelay());
            }
        }

        private IEnumerator JumpDelay()
        {
            yield return new WaitForSeconds(JumpTime);
            Anim.SetBool("Jump", false);
        }

        private void Slide()
        {
            // if (CanSlide && canJump)
            if (CanSlide)
            {
                CanSlide = false;
                Anim.SetBool("Slide", true);
                PlayerColliderNormal.enabled = false;
                PlayercolliderSlide.enabled = true;
                StartCoroutine(SlideDelay());
            }
        }


        private IEnumerator SlideDelay()
        {
            yield return new WaitForSeconds(SlideTime);
            CanSlide = true;
            Anim.SetBool("Slide", false);
            yield return new WaitForSeconds(1);
            PlayerColliderNormal.enabled = true;
            PlayercolliderSlide.enabled = false;

        }

        void DisableJump()
        {
            canJump = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "PlatformSurface")
            {
                canJump = true;
            }
        }

        private void OnEnable()
        {
            PlayerManager.PlayerFly += DisableJump;
            OnStart += StartRun;
        }

        private void OnDisable()
        {
            PlayerManager.PlayerFly -= DisableJump;
            OnStart -= StartRun;
        }
    }
}
