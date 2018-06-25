using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private Vector3 _direc;
    [SerializeField] private string up, down, left, right, forward, back;
    
    private Rigidbody _rigidbody;
    //private Input _forward, _back, _left, _right, _up, _down;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Space"))
        {
            print("oef");
        }
        Move(_direc, _speed, up, down, left, right, forward, back);
    }

    private void Move(Vector3 direction,  float speed, string inputUp, string inputDown, string inputLeft, string inputRight, string inputForward, string inputBack)
    {
        if (Input.GetButton(inputUp))
        {
            direction.y = 1;
        }
        else if (Input.GetButton(inputDown))
        {
            direction.y = -1;
        }
        else
        {
            direction.y = 0;
        }
        
        if (Input.GetButton(inputForward))
        {
            direction.x = 1;
        }
        else if (Input.GetButton(inputBack))
        {
            direction.x = -1;
        }
        else
        {
            direction.x = 0;
        }
        
        if (Input.GetButton(inputUp))
        {
            direction.z = 1;
        }
        else if (Input.GetButton(inputDown))
        {
            direction.z = -1;
        }
        else
        {
            direction.z = 0;
        }
        
        _rigidbody.AddRelativeForce(direction * speed);
    }
}
