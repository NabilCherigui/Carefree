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
        Vector3 horizontalDirection = new Vector3();
        Vector3 verticalDirection = new Vector3();
        Vector3 gravityDirection = new Vector3();

        //Movement when the player is not falling
        if (_falling == false)
        {
            if (Input.GetAxis(_verticalAxis) < 0)
            {
                verticalDirection = transform.forward * Input.GetAxis(_verticalAxis) * _backwardSpeed;
            }
            else if (Input.GetButton(_run))
            {
                verticalDirection = transform.forward * Input.GetAxis(_verticalAxis) * _runSpeed;
            }
            else
            {
                verticalDirection = transform.forward * Input.GetAxis(_verticalAxis) * _forwardSpeed;
            }

            horizontalDirection = transform.right * Input.GetAxis(_horizontalAxis) * _strafingSpeed;
        }

        //Checking if the player is grounded so the player can jump
        if (Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit,
                _sphereCastMaxDistance) && Input.GetButtonDown(_jump))
        {
            gravityDirection = transform.up * _jumpSpeed;
        }

        //If the player is not grounded gravity is calculated to increase the speed at which the player is falling
        if (Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit, _sphereCastMaxDistance) == false)
        {
            if (_timeFromFalling == 0)
            {
                _timeFromFalling = Time.time;
            }

            _falling = true;
            gravityDirection = transform.up * (_gravity * -1) * (Time.time - _timeFromFalling);
        }
        else
        {
            _falling = false;
            _timeFromFalling = 0;
        }

        _direction = verticalDirection + horizontalDirection + gravityDirection;

        //If there is no input the player is forced to stop. If there is input the player never exceeds his maximum allowed speed.
        if (_rigidbody.velocity.magnitude > _direction.magnitude && _falling == false)
        {
            if (_direction.Equals(Vector3.zero))
            {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                _rigidbody.Sleep();
                _rigidbody.WakeUp();
            }
            else if (_direction.Equals(Vector3.zero) == false)
            {
                _rigidbody.velocity = _direction;
                _rigidbody.angularVelocity = Vector3.zero;
                Move();
            }
        }
        else
        {
            Move();
        }
        
    }
    
    private void Move()
    {
        _rigidbody.AddForce(_direction);
    } 
}