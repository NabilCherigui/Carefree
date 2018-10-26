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
    
    [SerializeField] private float _speed, _gravity;
    
    [SerializeField] private string _horizontalAxis =  "Horizontal", _verticalAxis = "Vertical";
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (gravityMode == GravityMode.On)
        {
            Move(_speed, _horizontalAxis, _verticalAxis, _gravity);
        }
        else
        {
            Move(_speed, _horizontalAxis, _verticalAxis);
        }
        
    }
    
    private void Move(float speed, string horizontal, string vertical)
    {
        var direction = new Vector3();
        direction.x = Input.GetAxis(horizontal);
        direction.z = Input.GetAxis(vertical);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
    }
    
    private void Move(float speed, string horizontal, string vertical, float gravity)
    {
        var direction = new Vector3();
        direction.x = Input.GetAxis(horizontal) * speed;
        direction.y = gravity;
        direction.z = Input.GetAxis(vertical) * speed;

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction, ForceMode.VelocityChange);
    }

}
