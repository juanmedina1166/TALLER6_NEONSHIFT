using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private PlayerMovement movement;
    private PlayerManager manager;

    // Variables para swipe
    private Vector2 startTouchPosition;
    private Vector2 currentPosition;
    private bool stopTouch = false;

    [Header("Swipe Config")]
    public float swipeRange = 50f;
    public float tapRange = 10f;

    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (!PlayerManager.instance.isAlive) return;

        HandleKeyboard();
        HandleSwipe();
    }

    // --- INPUT TECLADO ---
    void HandleKeyboard()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            movement.MoveLeft();

        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            movement.MoveRight();

        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            movement.Jump();

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            movement.Slide();
    }

    // --- INPUT SWIPE (TOUCH) ---
    void HandleSwipe()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startTouchPosition = touch.position;
                    stopTouch = false;
                    break;

                case TouchPhase.Moved:
                    currentPosition = touch.position;
                    Vector2 distance = currentPosition - startTouchPosition;

                    if (!stopTouch)
                    {
                        if (Mathf.Abs(distance.x) > swipeRange)
                        {
                            if (distance.x < 0)
                                movement.MoveLeft();
                            else
                                movement.MoveRight();

                            stopTouch = true;
                        }
                        else if (Mathf.Abs(distance.y) > swipeRange)
                        {
                            if (distance.y > 0)
                                movement.Jump();
                            else
                                movement.Slide();

                            stopTouch = true;
                        }
                    }
                    break;

                case TouchPhase.Ended:
                    stopTouch = true;
                    break;
            }
        }
    }
}
