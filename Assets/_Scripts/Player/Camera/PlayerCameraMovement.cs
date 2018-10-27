using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{

    [SerializeField] private Camera _camera;
    [SerializeField] private float _sensitivity;
    [SerializeField] private float _minX;
    [SerializeField] private float _maxX;
    [SerializeField] private string _xAxis = "Mouse X", _yAxis = "Mouse Y";
   
    private float _clampX;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    private void Update()
    {
        MoveCamera(_xAxis, _yAxis);
    }
	
    private void MoveCamera(string xAxis, string yAxis)
    {
        float mouseX = Input.GetAxis(xAxis);
        float mouseY = Input.GetAxis(yAxis);

        float rotationX = mouseX * _sensitivity;
        float rotationY = mouseY * _sensitivity;

        _clampX -= rotationY;
        
        Vector3 cameraRotation = _camera.transform.rotation.eulerAngles;
        Vector3 bodyRotation = transform.rotation.eulerAngles;
        
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
