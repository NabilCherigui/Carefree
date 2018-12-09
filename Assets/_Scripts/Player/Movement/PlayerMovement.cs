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
    private bool _grounded;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float dirX = new float();
        float dirY = new float();
        float dirZ = new float();

        if (Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit,
                _sphereCastMaxDistance))
        {
            print("Grounded");
            _grounded = true;
            _timeFromFalling = 0;
        }
        else
        {
            print("air");
            _grounded = false;
        }

        //Movement when the player is not falling
        if (_grounded)
        {
            if (Input.GetAxis(_verticalAxis) < 0)
            {
                dirX = Input.GetAxis(_verticalAxis) * _backwardSpeed;
            }
            else if (Input.GetButton(_run))
            {
                dirX = Input.GetAxis(_verticalAxis) * _runSpeed;
            }
            else
            {
                dirX = Input.GetAxis(_verticalAxis) * _forwardSpeed;
            }

            dirY = Input.GetAxis(_horizontalAxis) * _strafingSpeed;
        }

        //Checking if the player is grounded so the player can jump
        if (_grounded && Input.GetButtonDown(_jump))
        {
            dirZ = _jumpSpeed;
        }

        //If the player is not grounded gravity is calculated to increase the speed at which the player is falling
        if (_grounded == false)
        {
            if(_timeFromFalling == 0)
            {
                _timeFromFalling = Time.time;
            }
            _direction = Move(dirX, dirY, dirZ) + Gravity(Time.time - _timeFromFalling, _gravity);
        }
        else
        {
            _direction = Move(dirX, dirY, dirZ);
        }
        
        //If there is no input the player is forced to stop. If there is input the player never exceeds his maximum allowed speed.
        if (_grounded)
        {
            if (_rigidbody.velocity.magnitude > _direction.magnitude)
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
                }
            }
        }

        _rigidbody.AddForce(_direction, ForceMode.Acceleration);
    }

    private Vector3 Move(float dirX, float dirY, float dirZ)
    {
        return transform.forward * dirX + transform.right * dirY + transform.up * dirZ;
    }

    private Vector3 Gravity(float time, float gravity)
    {
        return transform.up * (gravity * -1) * time;
    }
}