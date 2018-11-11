using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed, _gravity, _forwardSpeed, _backwardSpeed, _strafingSpeed, _jumpSpeed, _runSpeed;

    [SerializeField] private string _horizontalAxis = "Horizontal", _verticalAxis = "Vertical", _jump = "Jump", _run = "Run";
    
    [SerializeField] private Transform _sphereCastTransform;
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _sphereCastMaxDistance;
    
    private Rigidbody _rigidbody;

    private RaycastHit _raycastHit;

    private Vector3 _direction;
    private float _timeFromFalling;
    private bool _falling;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        Move(_horizontalAxis, _verticalAxis, _jump, _run, _gravity, _forwardSpeed, _backwardSpeed, _strafingSpeed, _jumpSpeed, _runSpeed);
    }
    
    //With gravity and separate speeds(forward, backward, strafing, jumping, running)
    private void Move(string horizontal, string vertical, string jump, string run, float gravity, float forwardSpeed, float backwardSpeed, float strafingSpeed, float jumpSpeed, float runSpeed)
    {
        Vector3 horizontalDirection = new Vector3();
        Vector3 verticalDirection = new Vector3();
        Vector3 gravityDirection = new Vector3();

        if (_falling == false)
        {
            if (Input.GetAxis(vertical) < 0)
            {
                verticalDirection = transform.forward * Input.GetAxis(vertical) * backwardSpeed;
            }
            else if(Input.GetButton(run))
            {
                verticalDirection = transform.forward * Input.GetAxis(vertical) * runSpeed;
            }
            else
            {
                verticalDirection = transform.forward * Input.GetAxis(vertical) * forwardSpeed;
            }

            horizontalDirection = transform.right * Input.GetAxis(horizontal) * strafingSpeed;
        }
        
        if (Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit,
                _sphereCastMaxDistance) && Input.GetButtonDown(jump))
        {
            gravityDirection = transform.up * jumpSpeed;
        }

        if (Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit, _sphereCastMaxDistance) == false)
        {
            if (_timeFromFalling == 0)
            {
                _timeFromFalling = Time.time;
            }

            _falling = true;
            gravityDirection = transform.up * (gravity * -1) * (Time.time - _timeFromFalling);
        }
        else
        {
            _falling = false;
            _timeFromFalling = 0;
        }
        
        _direction = verticalDirection + horizontalDirection + gravityDirection;

        if (_rigidbody.velocity.magnitude > _direction.magnitude && _falling == false)
        {
            if (_direction.Equals(Vector3.zero))
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.Sleep();
                _rigidbody.WakeUp();
             }
            else if(_direction.Equals(Vector3.zero) == false)
            {
                _rigidbody.velocity = _direction;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.AddForce(_direction);
            }
        }
        else
        {
            _rigidbody.AddForce(_direction);
        }
    }
    
}