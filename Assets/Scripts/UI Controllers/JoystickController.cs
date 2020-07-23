using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;

public class JoystickController : MonoBehaviour
{
    public GameObject thumb;
    public GameObject background;
    private Vector2 currTouchPos;
    private Rect backgroundRect;

    private float dragRadius = 100f;
    private int touchCount = 0;

    // vector for movement, values range [-1,1] with fixed magnitude of 1
    public static Vector2 moveVector;

    public void Start()
    {
        thumb = GameObject.FindWithTag("Joystick_Thumb");
        background = GameObject.FindWithTag("Joystick_Background");

        // set rect for the background of the joystick in world coords
        setRect();

        moveVector = Vector2.zero;
        currTouchPos = Vector2.zero;
    }

    void Update()
    {
        if (Input.touches.Length == 0)
        {
            // no touch so snap to center
            thumb.transform.position = backgroundRect.center;
            setMoveVector(Vector2.zero, false);
        }

        touchCount = 1;
        foreach (Touch touch in Input.touches)
        {
            // player touching dash button
            if (touch.position.x >= 1000.0f) { return; }
            currTouchPos = touch.position;
            
            // first touch
            if (touch.phase == TouchPhase.Began)
            {
                //if (backgroundRect.Contains(currTouchPos))
                if (touchInRange(currTouchPos))
                {
                    // touch in range so snap to touch
                    thumb.transform.position = currTouchPos;
                    setMoveVector(currTouchPos, true);
                }
                else
                {
                    thumb.transform.position = backgroundRect.center;
                    setMoveVector(currTouchPos, false);
                }
            }
            // moving touch
            else if (touch.phase == TouchPhase.Moved)
            {
                if (touchInRange(currTouchPos))
                {
                    // touch in range so snap to touch
                    thumb.transform.position = currTouchPos;
                }
                else
                {
                    thumb.transform.position = adjustToEdge(currTouchPos, dragRadius);
                }
                setMoveVector(currTouchPos, true);
            }
            // end touch
            else if (touch.phase == TouchPhase.Ended)
            {
                thumb.transform.position = backgroundRect.center;
                setMoveVector(currTouchPos, false);
            }
            touchCount += 1;
        }
    }

    public void setMoveVector(Vector2 vect, bool moving)
    {
        if (!moving)
        {
            moveVector = Vector2.zero;
            return;
        }
        Vector2 tempVector = adjustToEdge(vect, 1.0f);
        moveVector = new Vector2 (tempVector.x - backgroundRect.center.x, tempVector.y - backgroundRect.center.y);
    }

    public bool touchInRange(Vector2 touch)
    {
        float magnitude = getMagnitude(touch);
        return magnitude <= dragRadius;
    }

    public Vector2 adjustToEdge(Vector2 touch, float radius)
    {
        float deltaX = touch.x - backgroundRect.center.x;
        float deltaY = touch.y - backgroundRect.center.y;
        float magnitude = getMagnitude(touch);

        float factor = magnitude / radius;
        float newX = backgroundRect.center.x + (deltaX / factor);
        float newY = backgroundRect.center.y + (deltaY / factor);

        return new Vector2(newX, newY);
    }

    public float getMagnitude(Vector2 touch)
    {
        float deltaX = touch.x - backgroundRect.center.x;
        float deltaY = touch.y - backgroundRect.center.y;
        return Mathf.Sqrt( Mathf.Pow(deltaX, 2) + Mathf.Pow(deltaY, 2) );
    }

    public void setRect()
    {
        backgroundRect = new Rect(background.transform.position.x - dragRadius, background.transform.position.y - dragRadius, dragRadius * 2, dragRadius * 2);
    }
}
