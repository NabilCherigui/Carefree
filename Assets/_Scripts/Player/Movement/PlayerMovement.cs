using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private float _speed;
    
    private Rigidbody _rigidbody;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        Move(_speed, "Horizontal", "Vertical");
    }
    
    private void Move(float speed, string horizontal, string vertical)
    {
        var direction = new Vector3();
        direction.x = Input.GetAxis(horizontal);
        direction.z = Input.GetAxis(vertical);

        _rigidbody.velocity = Vector3.zero;
        _rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
    }

}
