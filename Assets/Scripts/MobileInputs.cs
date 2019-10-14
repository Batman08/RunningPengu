using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MobileInputs : MonoBehaviour
{
    private const float DEAD_ZONE = 40f;
    private const int LeftMouseButton = 0;
    private bool _isTap, _isSwipeRight, _isSwipeLeft, _isSwipeUp, _isSwipeDown;
    private Vector2 _swipeDelta, _startTouch;

    public static MobileInputs Instance { set; get; }
    public Vector2 SwipeDelta { get { return _swipeDelta; } }
    public bool IsTap { get { return _isTap; } }
    public bool IsSwipeRight { get { return _isSwipeRight; } }
    public bool IsSwipeLeft { get { return _isSwipeLeft; } }
    public bool IsSwipeUp { get { return _isSwipeUp; } }
    public bool IsSwipeDown { get { return _isSwipeDown; } }



    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this);
        //SceneManager.LoadScene("Main_Game");
    }

    private void Update()
    {
        ResetAllSwipes();
        //MobileControlInputs(); //todo: uncomment when using mobile
        StandaloneInputs(); //todo: uncomment when using desktop
        CalculateDistanceOfSwipe();
    }

    private void ResetAllSwipes()
    {
        _isTap = _isSwipeRight = _isSwipeLeft = _isSwipeDown = _isSwipeUp = false;
    }

    private void StandaloneInputs()
    {
        bool isMouseButtonDown = Input.GetMouseButtonDown(LeftMouseButton);
        bool isMouseButtonUp = Input.GetMouseButtonUp(LeftMouseButton);

        if (isMouseButtonDown)
        {
            _isTap = true;
            _startTouch = Input.mousePosition;
        }
        else if (isMouseButtonUp)
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
                _isTap = true;
                _startTouch = Input.mousePosition;
            }


            else if (touchIsEnded || touchIsCanceled)
            {
                _startTouch = _swipeDelta = Vector2.zero;
            }
        }
    }//**

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
                    _isSwipeLeft = true;
                else
                    _isSwipeRight = true;
            }

            else
            {
                //Up or Down
                bool swipeDown = (y < 0);
                if (swipeDown)
                    _isSwipeDown = true;
                else
                    _isSwipeUp = true;
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
    }//**

    private void CalculateStandaloneSwipe()
    {
        if (Input.GetMouseButton(0))
        {
            _swipeDelta = (Vector2)Input.mousePosition - _startTouch;
        }
    }//**
}
