using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] float _speed = 30f;
    [SerializeField] float _rotationSpeed = 30f;

    Rigidbody2D _rigidBody;
    Transform _transform;
    float _rotationAmount;
    float _thrust;
    float _angle;
    Camera _mainCamera;
    bool _swappingX, _swappingY;

    private bool ShipInView
    {
        get
        {
            if (_transform.position.x < -18f ||
                _transform.position.x > 18f ||
                _transform.position.y < -14f ||
                _transform.position.y > 14)
            {
                return false;
            }
            return true;

        }
                    
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = transform;
        _mainCamera = Camera.main;
    }

    void Update()
    {
        GetRotation();
        GetThrust();
    }

    private void FixedUpdate()
    {
        if (!Mathf.Approximately(0f, _rotationAmount))
        {
            _angle = _transform.localEulerAngles.z;
            _angle += _rotationAmount * Time.fixedDeltaTime;
            _transform.localRotation = Quaternion.Euler(0, 0, _angle);
        }

        if (!Mathf.Approximately(0f, _thrust))
        {
            _rigidBody.AddForce(transform.up * _thrust * Time.fixedDeltaTime);
        }

        ScreenWrap();
    }

    private void ScreenWrap()
    {
        if (ShipInView)
        {
            _swappingX = false;
            _swappingY = false;
            return;
        }
        if (_mainCamera == null) return;
        if (_swappingX && _swappingY) return;

        Vector3 viewportPosition = _mainCamera.WorldToViewportPoint(_transform.position);
        if (!_swappingX && (viewportPosition.x > 1 || viewportPosition.x < 0))
        {
            _transform.position = new Vector3(_transform.position.x * -1, _transform.position.y, _transform.position.z);
            _swappingX = true;
        }
        if (!_swappingY && (viewportPosition.y > 1 || viewportPosition.y < 0))
        {
            _transform.position = new Vector3(_transform.position.x, _transform.position.y * -1, _transform.position.z);
            _swappingY = true;
        }
    }

    private void GetThrust()
    {
        if (Input.GetKey(KeyCode.N))
        {
            _thrust = _speed;
        }
        else
        {
            _thrust = 0;
        }
    }

    private void GetRotation()
    {
        // rotate left
        if (Input.GetKey(KeyCode.Z))
        {
            _rotationAmount = _rotationSpeed;
        }
        // rotate right
        else if (Input.GetKey(KeyCode.X))
        {
            _rotationAmount = _rotationSpeed * -1;
        }
        else
        {
            _rotationAmount = 0;
        }
    }
}
