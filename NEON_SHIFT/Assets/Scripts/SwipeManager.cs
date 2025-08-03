using UnityEngine;

public class SwipeManager : MonoBehaviour
{
    public static bool swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 startTouch;
    private const float SWIPE_THRESHOLD = 50f;

    void Update()
    {
        swipeLeft = swipeRight = swipeUp = swipeDown = false;

        if (Input.touches.Length > 0)
        {
            Touch t = Input.touches[0];
            if (t.phase == TouchPhase.Began)
            {
                startTouch = t.position;
            }
            else if (t.phase == TouchPhase.Ended)
            {
                Vector2 delta = t.position - startTouch;

                if (delta.magnitude > SWIPE_THRESHOLD)
                {
                    float x = delta.x;
                    float y = delta.y;

                    if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x < 0) swipeLeft = true;
                        else swipeRight = true;
                    }
                    else
                    {
                        if (y > 0) swipeUp = true;
                        else swipeDown = true;
                    }
                }
            }
        }
    }
}
