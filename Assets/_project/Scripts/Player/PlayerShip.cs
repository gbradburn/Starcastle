using System;
using UnityEngine;

public class PlayerShip : MonoBehaviour, IDamageable
{
    [SerializeField] float _speed = 30f;
    [SerializeField] float _rotationSpeed = 30f;
    [SerializeField] GameObject _explosionPrefab = null;
    [SerializeField] AudioClip _explosionSound = null;
    [SerializeField] GameObject _engineExhaust = null;

    Rigidbody2D _rigidBody;
    Transform _transform;
    float _rotationAmount;
    float _thrust;
    float _angle;

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _transform = transform;
    }

    private void OnEnable()
    {
        GameManager.Instance.GameStateChanged.AddListener(OnGameStateChanged);
    }

    private void OnDisable()
    {
        GameManager.Instance.GameStateChanged.RemoveListener(OnGameStateChanged);
    }

    private void OnGameStateChanged(GameManager.GameStates gameState)
    {
        if (gameState == GameManager.GameStates.StarcastleDestroyed)
        {
            _rigidBody.velocity = Vector2.zero;
            _rigidBody.angularVelocity = 0f;
            gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            _thrust = 0f;
            _rotationAmount = 0f;
            return;
        }
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
    }

    private void LateUpdate()
    {
        _engineExhaust.SetActive(_thrust > 0f);
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

    public void TakeDamage(int damage)
    {
        SoundManager.Instance.PlaySoundEffect(_explosionSound);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        GameManager.Instance.PlayerShipDestroyed();
        gameObject.SetActive(false);
    }
}
