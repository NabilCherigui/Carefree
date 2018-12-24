using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _forwardMaxSpeed, _backwardMaxSpeed, _strafingMaxSpeed, _runMaxSpeed, _jumpSpeed;
    [SerializeField] private float _forwardTimeToMax, _backwardTimeToMax, _strafingTimeToMax, _runTimeToMax;
    private float _forwardSpeed, _backwardSpeed, _strafingSpeed, _runSpeed;

    [SerializeField] private Vector3 _gravity, _terminalVelocity;

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
        Vector3 straight = new Vector3();
        Vector3 side = new Vector3();
        Vector3 vertical = new Vector3();

        _grounded = Physics.SphereCast(_sphereCastTransform.position, _sphereCastRadius, Vector3.down, out _raycastHit, _sphereCastMaxDistance);

        if (_grounded == false)
        {
            _timeFromFalling += Time.deltaTime;
            _forwardSpeed = 1;
            _backwardSpeed = 1;
            _strafingSpeed = 1;
            _runSpeed = 1;

            Gravity(_gravity, _terminalVelocity, _timeFromFalling);
        }
        if (_grounded)
        {
            _timeFromFalling = 0;

            if (Input.GetAxis(_verticalAxis) < 0)
            {
                _backwardSpeed = Accelerate(_backwardSpeed, _backwardTimeToMax, _backwardMaxSpeed);
                straight = transform.forward * Input.GetAxis(_verticalAxis) * _backwardSpeed;
            }
            else if (Input.GetButton(_run))
            {
                _runSpeed = Accelerate(_runSpeed, _runTimeToMax, _runMaxSpeed);
                straight = transform.forward * Input.GetAxis(_verticalAxis) * _runSpeed;

            }
            else if(Input.GetAxis(_verticalAxis) > 0)
            {
                _forwardSpeed = Accelerate(_forwardSpeed, _forwardTimeToMax, _forwardMaxSpeed);
                straight = transform.forward * Input.GetAxis(_verticalAxis) * _forwardSpeed;
            }
            else
            {
                _forwardSpeed = 1;
                _backwardSpeed = 1;
                _runSpeed = 1;
            }

            if (Input.GetButton(_horizontalAxis))
            {
                _strafingSpeed = Accelerate(_strafingSpeed, _strafingTimeToMax, _strafingMaxSpeed);
                side = transform.right * Input.GetAxis(_horizontalAxis) * _strafingSpeed;
            }
            else
            {
                _strafingSpeed = 0;
            }

            vertical = transform.up *Input.GetAxis(_jump) * _jumpSpeed;

            Move(straight + side + vertical);
        }
    }

    private void Move(Vector3 direction)
    {
        _rigidbody.velocity = direction;
    }
    
    private void Gravity(Vector3 direction, Vector3 terminalVelocity, float time)
    {
        if((direction * time).magnitude < terminalVelocity.magnitude)
        {
            _rigidbody.AddForce(direction * time, ForceMode.Acceleration);
        }
        else
        {
            _rigidbody.AddForce(terminalVelocity, ForceMode.Acceleration);
        }
    }

    private float Accelerate(float speed, float time, float maxSpeed)
    {
        speed += maxSpeed / time * Time.deltaTime;

        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }

        return speed;
    }
}
