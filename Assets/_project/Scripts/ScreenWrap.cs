using UnityEngine;

public class ScreenWrap : MonoBehaviour
{
    private Camera _mainCamera;
    private Transform _transform;
    Vector3 _viewportPoint;
    Vector3 _newPosition;
    Vector3 _swappedViewportPoint;

    float SwapXAxis
    {
        get
        {
            _swappedViewportPoint = _viewportPoint;
            _swappedViewportPoint.x = _viewportPoint.x < 0 ? 0.98f : 0.02f;
            return _mainCamera.ViewportToWorldPoint(_swappedViewportPoint).x;
        }
    }

    float SwapYAxis
    {
        get
        {
            _swappedViewportPoint = _viewportPoint;
            _swappedViewportPoint.y = _viewportPoint.y < 0 ? 0.98f : 0.02f;
            return _mainCamera.ViewportToWorldPoint(_swappedViewportPoint).y;
        }
    }


    void Awake()
    {
        _mainCamera = Camera.main;
        _transform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        _newPosition = _transform.position;
        _viewportPoint = _mainCamera.WorldToViewportPoint(_transform.position);
        if (_viewportPoint.x < 0 || _viewportPoint.x > 1)
        {
            _newPosition.x = SwapXAxis;
        }
        if (_viewportPoint.y < 0 || _viewportPoint.y > 1)
        {
            _newPosition.y = SwapYAxis;
        }
        _transform.position = _newPosition;
    }
}
