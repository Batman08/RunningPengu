using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileInputs : MonoBehaviour
{
    private const float DEAD_ZONE = 100f;

    public static MobileInputs Instance { set; get; }

    private bool _tap, _swipeRight, _swipeLeft, _swipeUp, _swipeDown;
    private Vector2 _swipeDelta, _startTouch;

    public Vector2 SwipeDelta { get { return _swipeDelta; } }
    public bool Tap { get { return _tap; } }
    public bool SwipeRight { get { return _swipeRight; } }
    public bool SwipeLeft { get { return _swipeLeft; } }
    public bool SwipeUp { get { return _swipeUp; } }
    public bool SwipeDown { get { return _swipeDown; } }



    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        //SceneManager.LoadScene("Main_Game");
    }

    private void Update()
    {
        ResetAllSwipes();
        //MobileControlInputs(); **
        StandaloneInputs();
        CalculateDistanceOfSwipe();
    }

    private void ResetAllSwipes()
    {
        _tap = _swipeRight = _swipeLeft = _swipeDown = _swipeUp = false;
    }

    private void StandaloneInputs()
    {
        bool mouseButtonDown = (Input.GetMouseButtonDown(0));
        bool mouseButtonUp = (Input.GetMouseButtonUp(0));

        if (mouseButtonDown)
        {
            _tap = true;
            _startTouch = Input.mousePosition;
        }

        else if (mouseButtonUp)
        {
            _startTouch = _swipeDelta = Vector2.zero;
        }
    }

    private void MobileControlInputs()
    {
        bool anyTouches = (Input.touches.Length != 0);

        bool startOfTouch = (Input.touches[0].phase == TouchPhase.Began);
        bool touchIsEnded = (Input.touches[0].phase == TouchPhase.Ended);
        bool touchIsCanceled = (Input.touches[0].phase == TouchPhase.Canceled);

        if (anyTouches)
        {
            if (startOfTouch)
            {
                _tap = true;
                _startTouch = Input.mousePosition;
            }


            else if (touchIsEnded || touchIsCanceled)
            {
                _startTouch = _swipeDelta = Vector2.zero;
            }
        }
    }

    private void CalculateDistanceOfSwipe()
    {
        _swipeDelta = Vector2.zero;

        bool touchingScreen = (_startTouch != Vector2.zero);
        //Calculate distance
        if (touchingScreen)
        {
            //CalculateMobileSwipe();
            CalculateStandaloneSwipe();
        }

        bool beyondDeadZone = (_swipeDelta.magnitude > DEAD_ZONE);
        //Check if we're beyond the deadzone
        if (beyondDeadZone)
        {
            //This is a confirmed swipe
            float x = _swipeDelta.x;
            float y = _swipeDelta.y;

            bool goingRightOrLeft = (Mathf.Abs(x) > Mathf.Abs(y));
            if (goingRightOrLeft)
            {
                //Right or Left
                bool swipeInLeftDir = (x < 0);
                if (swipeInLeftDir)
                    _swipeLeft = true;
                else
                    _swipeRight = true;
            }

            else
            {
                //Up or Down
                bool swipeDown = (y < 0);
                if (swipeDown)
                    _swipeDown = true;
                else
                    _swipeUp = true;
            }

            _startTouch = _swipeDelta = Vector2.zero;
        }
    }

    private void CalculateMobileSwipe()
    {
        if (Input.touches.Length != 0)
        {
            _swipeDelta = Input.touches[0].position - _startTouch;
        }
    }

    private void CalculateStandaloneSwipe()
    {
        if (Input.GetMouseButton(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
        }
    }
}
