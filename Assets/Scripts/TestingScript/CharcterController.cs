using UnityEngine;

public class CharcterController : MonoBehaviour
{
    private Vector2 firstPressPos;
    private Vector2 lastPressPos;
    private Vector2 currentSwipe;

    [SerializeField] private float playerSpeed = 10;
    [SerializeField] private float LaneChangeSpeed;

    [SerializeField] private float MidLanePos;
    [SerializeField] private float LeftLanePos;
    [SerializeField] private float RightLanePos;
    [SerializeField] private Lanes PlayerLanes;


    private void Start()
    {
        PlayerLanes = Lanes.Middle;
    }

    private void Update()
    {
        Movement();
        SwipeControl();
        PlayerLaneHandler();
    }

    private void Movement()
    {
        transform.Translate(Vector3.left * Time.deltaTime * playerSpeed);
    }

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
            currentSwipe.Normalize();

            if (currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("left swipe");
                GoLeft();
            }

            if (currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
            {
                Debug.Log("right swipe");
                GoRight();
            }
        }
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

    void MoveToNextLane(float LanePos)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, transform.position.y, LanePos), LaneChangeSpeed * Time.deltaTime);
    }
}

public enum Lanes
{
    Left,
    Middle,
    Right,
}
