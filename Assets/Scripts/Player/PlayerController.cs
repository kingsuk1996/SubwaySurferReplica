//@Author Sabyasachi Thakur
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private PlayerLaneSystem playerLaneSystem;
        [SerializeField] private GameSettings gameSettings;

        [Space(10)]
        [SerializeField] private GameObject followCam;
        [SerializeField] private GameObject introCam;
        [SerializeField] private GameObject playerShadow;

        [Space(10)]
        [SerializeField] float MidLanePos;
        [SerializeField] float LeftLanePos;
        [SerializeField] float RightLanePos;
        [SerializeField] float LaneChangeSpeed;

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
        public Action OnStart;
        public Text Uitext;
        public Coroutine coroutine;
        public Lanes PlayerLanes;


        private void Start()
        {
            introCam.SetActive(true);
            followCam.SetActive(false);
            playerShadow.SetActive(false);
            PlayerColliderNormal = GetComponent<CapsuleCollider>();
            PlayercolliderSlide = GetComponent<BoxCollider>();
            PlayerLanes = Lanes.Middle;
        }

        void StartRun()
        {
            UImanager.Instance.CanMove = true;
            Anim.SetTrigger("Run");
            playerShadow.SetActive(true);
            followCam.SetActive(true);
            introCam.SetActive(false);
        }
        private void Update()
        {
            if (UImanager.Instance.CanMove)
            {
                time += Time.deltaTime;
                //SwipeInput();
                JumpInput = Input.GetKey(KeyCode.Space);
                totalDistance = MathF.Round((time * BlockSpeedController.Instance.BlockSpeed));// - BlockSpeedController.Instance.BlockSpeed);
                UpdateDistance(totalDistance);
                SwipeControl();
            }

        }
        public IEnumerator EnableTest()
        {
            Uitext.gameObject.SetActive(true);
            yield return new WaitForSeconds(.1f);
            Uitext.gameObject.SetActive(false);

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



            if (Input.touchCount > 0)
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
                            GoRight?.Invoke();
                            Anim.SetTrigger("Lean Right");


                        }
                        else
                        {   //Left swipe
                            GoLeft?.Invoke();
                            Anim.SetTrigger("Lean Left");

                        }

                    }
                    else if (Mathf.Abs(lastPos.y - firstPos.y) > Mathf.Abs(lastPos.x - firstPos.x))
                    {   //the vertical movement is greater than the horizontal movement
                        if (lastPos.y > firstPos.y)  //If the movement was up
                        {   //Up swipe

                            Jump();
                        }
                        else
                        {   //Down swipe

                            Slide();
                        }
                        //TakeMousePos = false;
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
                        else
                        {   //Down swipe

                            Slide();
                        }
                        // TakeMousePos = false;
                    }
                    TakeMousePos = false;
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    TakeMousePos = false;
                    firstPos = Vector2.zero;
                    lastPos = Vector2.zero;
                }
            }
        }

        Vector2 firstPressPos;
        Vector2 lastPressPos;
        Vector2 currentSwipe;

        private void SwipeControl()
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

                //swipe upwards
                if (currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("up swipe");
                    Jump();
                }
                //swipe down
                if (currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
                {
                    Debug.Log("down swipe");
                    Slide();
                }
                //swipe left
                if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (PlayerLanes == Lanes.Right || PlayerLanes == Lanes.Middle)
                    {
                        Debug.Log("left swipe");
                        //GoLeft?.Invoke();
                        Goleft();
                        Anim.SetTrigger("Lean Left");
                    }
                }
                //swipe right
                if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
                {
                    if (PlayerLanes == Lanes.Left || PlayerLanes == Lanes.Middle)
                    {
                        Debug.Log("right swipe");
                        //GoRight?.Invoke();
                        Goright();
                        Anim.SetTrigger("Lean Right");
                    }
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
            StartCoroutine(PlayerLaneHandler(LeftLanePos));
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
            StartCoroutine(PlayerLaneHandler(RightLanePos));
        }


        IEnumerator PlayerLaneHandler(float thresold)
        {
            UImanager.Instance.CanMove = false;
            Vector3 targetpos = this.transform.position + transform.right * thresold + transform.forward;
            while (this.transform.position != targetpos)
            {
                yield return Time.deltaTime;
                transform.position = Vector3.MoveTowards(this.transform.position, targetpos, LaneChangeSpeed * Time.deltaTime);
            }
            UImanager.Instance.CanMove = true;
        }
        private void Jump()
        {
            //if (canJump && CanSlide)
            if (canJump)
            {
                if (coroutine != null)
                {

                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(EnableTest());
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
                if (coroutine != null)
                {

                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(EnableTest());
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
