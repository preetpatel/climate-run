using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
    // Mobile Input Instance static Singleton
    private static MobileInput instance;
    public static MobileInput Instance {
        get {
            if (instance == null)
            {
                instance = FindObjectOfType<MobileInput>();
                if (instance == null)
                {
                    instance = new GameObject("Generated swipeInputController", typeof(MobileInput)).GetComponent<MobileInput>();
                }
            }

            return instance;
        }
        set { instance = value; }
    }

    private readonly float ignoreInputDelay = 5.0f;
    private readonly float doubleTapDelta = 0.5f;

    private bool tap, doubleTap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private Vector2 swipeDelta, startTouch;
    private float lastTap;
    private float inputDelaySquared;

    #region Public properties
    public bool Tap { get { return tap; } }
    public bool DoubleTap { get { return doubleTap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }
    #endregion

    private void Start()
    {
        // set up inputDelaySquared field for use later in update method
        inputDelaySquared = ignoreInputDelay * ignoreInputDelay;
    }

    private void Update()
    {
        // Reset recognitions to register new ones
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = doubleTap = false;

// At runtime, decide which device is used in order to optomise touch recognition
#if UNITY_EDITOR
        UpdateDesktop();
#else
        UpdateMobile();
#endif
    }

    private void UpdateDesktop()
    {
        bool didKeyPress = false;

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            swipeUp = true;
            didKeyPress = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            swipeDown = true;
            didKeyPress = true;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            swipeLeft = true;
            didKeyPress = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            swipeRight = true;
            didKeyPress = true;
        }


        if (didKeyPress)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            startTouch = Input.mousePosition;
            doubleTap = Time.time - lastTap < doubleTapDelta;
            lastTap = Time.time;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            startTouch = swipeDelta = Vector2.zero;
        }

        // clear swipe delta
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.GetMouseButton(0))
            swipeDelta = (Vector2)Input.mousePosition - startTouch;

        // only perform calculation if swipe delta is above the set hard delay
        if (swipeDelta.sqrMagnitude > inputDelaySquared)
        {
            // Check what type of swipe this is
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // check for left or right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // check for up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
            startTouch = swipeDelta = Vector2.zero;
        }
    }

    // Identical method to UpdateDesktop but seperated because touch getures
    // recognition gets terrible if mouse input detection is also checked
    private void UpdateMobile()
    {
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.mousePosition;
                doubleTap = Time.time - lastTap < doubleTapDelta;
                lastTap = Time.time;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                startTouch = swipeDelta = Vector2.zero;
            }
        }

        // clear swipe delta
        swipeDelta = Vector2.zero;

        if (startTouch != Vector2.zero && Input.touches.Length != 0)
            swipeDelta = Input.touches[0].position - startTouch;

        // only perform calculation if swipe delta is above the set hard delay
        if (swipeDelta.sqrMagnitude > inputDelaySquared)
        {
            // Check what type of swipe this is
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                // check for left or right
                if (x < 0)
                    swipeLeft = true;
                else
                    swipeRight = true;
            }
            else
            {
                // check for up or down
                if (y < 0)
                    swipeDown = true;
                else
                    swipeUp = true;
            }
            startTouch = swipeDelta = Vector2.zero;
        }
    }
}
