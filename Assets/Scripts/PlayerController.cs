using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float LANE_DISTANCE = 3f;
    private const float TURN_SPEED = 0.05f;

    //Movement
    private CharacterController _playerController;

    private float _jumpForce = 8f;
    private float _gravity = 12f;
    private float _verticalVelocity;
    private float _velocity = 7f;

    private int _desiredLane = 1; //0 = Left, 1 = Middle, 2 = Right

    private Vector3 _targetPosition;
    private Vector3 _moveVector;

    void Start()
    {
        _playerController = GetComponent<CharacterController>();

    }

    void Update()
    {
        Inputs();
        Position();
        MoveThePlayer();
    }

    private void Position()
    {

        _targetPosition = transform.position.z * Vector3.forward;
        if (_desiredLane == 0)
        {
            _targetPosition += Vector3.left * LANE_DISTANCE;
        }
        else if (_desiredLane == 2)
        {
            _targetPosition += Vector3.right * LANE_DISTANCE;
        }
    }

    private void MoveThePlayer()
    {
        //Calculate move delta
        _moveVector = Vector3.zero;
        _moveVector.x = (_targetPosition - transform.position).normalized.x * _velocity;

        bool isGrounded = IsPlayerGrounded();
        //anim.SetBool("Grounded", isGrounded); pt/3

        //Calculate Y axis
        if (isGrounded)
        {
            _verticalVelocity = -0.1f;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                //Jump
                //anim.SetTrigger("Jump");
                _verticalVelocity = _jumpForce;
            }
        }

        else
        {
            _verticalVelocity -= (_gravity * Time.deltaTime);

            //Fast falling mechanic
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _verticalVelocity = -_jumpForce;
            }
        }


        _moveVector.y = _verticalVelocity;
        _moveVector.z = _velocity;

        _playerController.Move(_moveVector * Time.deltaTime);

        Vector3 dir = _playerController.velocity;
        dir.y = 0;
        transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }

    private void Inputs()
    {
        bool pressingRightKey = (Input.GetKeyDown(KeyCode.RightArrow));
        bool pressingLeftKey = (Input.GetKeyDown(KeyCode.LeftArrow));

        //Gather inputs on which lane we should be on
        if (pressingRightKey)
            MoveLane(true);
        else if (pressingLeftKey)
            MoveLane(false);
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
}
