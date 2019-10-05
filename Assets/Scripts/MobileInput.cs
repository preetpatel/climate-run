using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileInput : MonoBehaviour
{
	public static MobileInput Instance { get; set; }
	private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
	private Vector2 swipeDelta, startTouch;
	private const int DEATH_ZONE = 100;

	public bool Tap { get { return tap; } }
	public Vector2 SwipeDelta { get { return swipeDelta; } }

	public bool SwipeLeft { get { return swipeLeft; } }
	public bool SwipeRight { get { return swipeRight; } }
	public bool SwipeUp { get { return swipeUp; } }
	public bool SwipeDown { get { return swipeDown; } }


	// Start is called before the first frame update
	void Start()
    {
        
    }

	private void Awake()
	{
		Instance = this;
	}

	// Update is called once per frame
	void Update()
    {
		// Reset all booleans to false
		tap = swipeDown = swipeLeft = swipeRight = swipeUp = false;
		// Check for inputs
		#region Standalone Inputs
		if (Input.GetMouseButtonDown(0))
		{
			tap = true;
			startTouch = Input.mousePosition;
		}
		else if (Input.GetMouseButtonUp(0))
		{
			startTouch = swipeDelta = Vector2.zero;
		}
		#endregion

		#region Mobile Inputs
		if (Input.touches.Length != 0)
		{
			if (Input.touches[0].phase == TouchPhase.Began)
			{
				tap = true;
				startTouch = Input.mousePosition;
			}
			else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
			{
				startTouch = swipeDelta = Vector2.zero;
			}

		}
		#endregion

		// Calculate distance between start touch and now
		swipeDelta = Vector2.zero;
		if (startTouch != Vector2.zero)
		{
			if (Input.touches.Length != 0)
			{
				swipeDelta = Input.touches[0].position - startTouch;
			}
			else if (Input.GetMouseButton(0))
			{
				swipeDelta = (Vector2)Input.mousePosition - startTouch;
			}
		}

		// Check if beyond death zone
		if (swipeDelta.magnitude > DEATH_ZONE)
		{
			float x = swipeDelta.x;
			float y = swipeDelta.y;

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				// Left or right
				if (x < 0)
				{
					swipeLeft = true;
				}
				else
				{
					swipeRight = true;
				}
			}
			else
			{
				// Up or down
				if (y < 0)
				{
					swipeDown = true;
				}
				else
				{
					swipeUp = true;
				}

			}
			startTouch = swipeDelta = Vector2.zero;
        }
	}
}
