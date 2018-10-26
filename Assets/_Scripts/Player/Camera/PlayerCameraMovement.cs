using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    private float _clampX;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        MoveCamera();
    }
	
    private void MoveCamera()
    {
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        var rotationX = mouseX * _sensitivity;
        var rotationY = mouseY * _sensitivity;

        _clampX -= rotationY;
        
        var cameraRotation = _camera.transform.rotation.eulerAngles;
        var bodyRotation = transform.rotation.eulerAngles;
        
        cameraRotation.x -= rotationY;
        cameraRotation.z = 0;
        bodyRotation.y += rotationX;

        if(_clampX > _maxX)
        {
            _clampX = _maxX;
            cameraRotation.x = _maxX;
        }
        else if(_clampX < _minX)
        {
            _clampX = _minX;
            cameraRotation.x = 360 + _minX;
        }

        _camera.transform.rotation = Quaternion.Euler(cameraRotation);
        transform.rotation = Quaternion.Euler(bodyRotation);
        
    }
}
