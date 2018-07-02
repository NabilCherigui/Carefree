using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed;
    [SerializeField] private string up, down, left, right, forward, back;
    
    private Rigidbody _rigidbody;
    //private Input _forward, _back, _left, _right, _up, _down;
    
    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetButton("Space"))
        {
            print("oef");
        }
        Move(_speed, up, down, left, right, forward, back);
    }

    private void Move(float speed, string inputUp, string inputDown, string inputLeft, string inputRight, string inputForward, string inputBack)
    {
        var direction = new Vector3();
        direction.y = (Input.GetButton(inputUp)) ? 1 : (Input.GetButton(inputDown)) ? -1 : 0;
        direction.x = (Input.GetButton(inputForward)) ? 1 : (Input.GetButton(inputBack)) ? -1 : 0;
        direction.z = (Input.GetButton(inputLeft)) ? 1 : (Input.GetButton(inputRight)) ? -1 : 0;
        
        _rigidbody.AddRelativeForce(direction * speed);
    }
}
