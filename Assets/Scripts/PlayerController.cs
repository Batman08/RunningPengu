using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 1.5f; //2.25
    private const float TURN_SPEED = 0.1f;

    public Animator anim;

    //Movement
    private CharacterController _playerController;

    private bool _isRunning = false;

    private float _jumpForce = 8f;
    private float _gravity = 16f;
    private float _verticalVelocity;


    //Speed Modifier
    private float _originalSpeed = 4f;
    private float _speed = 4f;
    private float _speedIncreaseTick;
    private float _speedIncreaseTime = 2.5f;
    private float _speedIncreaseAmount = 0.1f;

    private int _desiredLane = 1; //0 = Left, 1 = Middle, 2 = Right

    private Vector3 _targetPosition;
    private Vector3 _moveVector;

    private bool swipeRight, swipeLeft, swipeUp, swipeDown;

    private void Start()
    {
        _playerController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        _speed = _originalSpeed;
    }

    public void SetSwipeBools()
    {
        swipeRight = MobileInputs.Instance.SwipeRight;
        swipeLeft = MobileInputs.Instance.SwipeLeft;
        swipeUp = MobileInputs.Instance.SwipeUp;
        swipeDown = MobileInputs.Instance.SwipeDown;
    }

    private void Update()
    {
        bool gameHasNotStarted = (!_isRunning);
        if (gameHasNotStarted)
            return;

        SpeedIncreasementOverTime();
        Inputs();
        Position();
        MoveThePlayer();
    }

    private void SpeedIncreasementOverTime()
    {
        if (Time.time - _speedIncreaseTick > _speedIncreaseTime)
        {
            _speedIncreaseTick = Time.time;
            _speed += _speedIncreaseAmount;
            GameManager.Instance.UpdateModifier(_speed - _originalSpeed);
        }
    }

    private void Inputs()
    {
        Debug.Log(MobileInputs.Instance.SwipeDelta);

        //Gather inputs on which lane we should be on
        if (swipeRight)
            MoveLane(true);
        else if (swipeLeft)
            MoveLane(false);
    }

    private void Position()
    {
        _targetPosition = transform.position.z * Vector3.forward;
        if (_desiredLane == 0)
        {
            _targetPosition += Vector3.left * LANE_DISTANCE;
            CameraController.Instance.PlayerXOffset = new Vector3(-0.65f, 0, 0);
        }
        else if (_desiredLane == 2)
        {
            _targetPosition += Vector3.right * LANE_DISTANCE;
            CameraController.Instance.PlayerXOffset = new Vector3(0.65f, 0, 0);
        }
        else if (_desiredLane == 1)
        {
            CameraController.Instance.PlayerXOffset = new Vector3(0, 0, 0);
        }
    }

    private void MoveThePlayer()
    {
        //Calculate move delta
        _moveVector = Vector3.zero;
        _moveVector.x = (_targetPosition - transform.position).normalized.x * _speed;

        bool isGrounded = IsPlayerGrounded();
        anim.SetBool("Grounded", isGrounded);

        //Calculate Y axis
        if (isGrounded)
        {
            _verticalVelocity = -0.1f;

            if (swipeUp)
            {
                //Jump
                anim.SetTrigger("Jump");
                _verticalVelocity = _jumpForce;
            }

            else if (swipeDown)
            {
                //Slide
                StartSliding();
                Invoke("StopSliding", 1f);
            }
        }

        else
        {
            _verticalVelocity -= (_gravity * Time.deltaTime);

            //Fast falling mechanic

            if (swipeDown)
            {
                _verticalVelocity = -_jumpForce;
            }
        }


        _moveVector.y = _verticalVelocity;
        _moveVector.z = _speed;

        _playerController.Move(_moveVector * Time.deltaTime);

        Vector3 dir = _playerController.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }

    private void StartSliding()
    {

        float colliderCenterX = _playerController.center.x;
        float colliderCenterY = _playerController.center.y / 2;
        float colliderCenterZ = _playerController.center.z;

        anim.SetBool("Sliding", true);

        _playerController.height /= 2;
        _playerController.center = new Vector3(colliderCenterX, colliderCenterY, colliderCenterZ);
        Debug.Log("Sliding");
    }

    private void StopSliding()
    {
        float colliderCenterX = _playerController.center.x;
        float colliderCenterY = _playerController.center.y * 2;
        float colliderCenterZ = _playerController.center.z;

        anim.SetBool("Sliding", false);

        _playerController.height *= 2;
        _playerController.center = new Vector3(colliderCenterX, colliderCenterY, colliderCenterZ);
    }

    private void MoveLane(bool isGoingRight)
    {
        _desiredLane += (isGoingRight) ? 1 : -1;
        _desiredLane = Mathf.Clamp(_desiredLane, 0, 2);
    }

    private bool IsPlayerGrounded()
    {
        Ray groundRay = new Ray(
            new Vector3(
             _playerController.bounds.center.x,
            (_playerController.bounds.center.y - _playerController.bounds.extents.y)
            + 0.2f,
            _playerController.bounds.center.z),
            Vector3.down);

        Debug.DrawRay(groundRay.origin, groundRay.direction, Color.cyan, 1.5f);

        return Physics.Raycast(groundRay, 0.2f + 0.1f);

    }

    public void StartRunning()
    {
        _isRunning = true;
        anim.SetTrigger("StartRunning");
        //CameraController.Instance.SnapToPlayerOnStart();
    }

    private void Crash()
    {
        anim.SetTrigger("Death");
        _isRunning = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch (hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
                break;
        }
    }
}
