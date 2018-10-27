using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum GravityMode
    {
        On,
        Off
    }
    
    public GravityMode gravityMode = GravityMode.On;

    [SerializeField] private bool gravity, seperateSpeed;
    
    [SerializeField] private float _speed, _gravity, _forwardSpeed, _backwardSpeed, _strafingSpeed;
    
    [SerializeField] private string _horizontalAxis =  "Horizontal", _verticalAxis = "Vertical";
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {   
        Move(_horizontalAxis, _verticalAxis, _gravity, _forwardSpeed, _backwardSpeed, _strafingSpeed);
        
        /*
        if (gravityMode == GravityMode.On)
        {
            Move(_speed, _horizontalAxis, _verticalAxis, _gravity);
        }
        else
        {
            Move(_speed, _horizontalAxis, _verticalAxis);
        }*/
        
    }
    
    private void Move(float speed, string horizontal, string vertical)
    {
        Vector3 direction = (transform.forward * Input.GetAxis(vertical)) + (transform.right * Input.GetAxis(horizontal)) * speed;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
    }
    
    private void Move(float speed, string horizontal, string vertical, float gravity)
    {
        Vector3 direction = ((transform.forward * Input.GetAxis(vertical)) + (transform.right * Input.GetAxis(horizontal))) * speed;
        direction.y = gravity;
        
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
    }
    
    private void Move(string horizontal, string vertical, float gravity, float forwardSpeed, float backwardSpeed, float strafingSpeed)
    {
        Vector3 direction = new Vector3();
        Vector3 horizontalDirection = transform.right * Input.GetAxis(horizontal) * strafingSpeed;;
        Vector3 verticalDirection = transform.forward * Input.GetAxis(vertical) * forwardSpeed;
        if (Input.GetAxis(vertical) < 0)
        {
            verticalDirection = transform.forward * Input.GetAxis(vertical) * backwardSpeed;
        }
        
        direction = verticalDirection + horizontalDirection;
        //Turn gravity on when you dont touch the ground
        //direction.y = gravity;

        _rigidbody.velocity = Vector3.zero;
        print("Direction: " + direction);
        print("Velocity: " + _rigidbody.velocity);
        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
    }
    
}