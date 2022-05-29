using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileLauncher : MonoBehaviour
{
    [SerializeField] Transform _muzzle;
    [SerializeField] [Range(1f,5f)] float _fireDelay = 5f;
    [SerializeField] GameObject _missilePrefab;
    [SerializeField] AudioClip _launchSound;

    private float _coolDown;
    private Transform _target;
    private int _playerMask;

    bool CanFire
    {
        get
        {
            _coolDown -= Time.deltaTime;
            if (_coolDown > 0f) return false;
            return Physics2D.Raycast(transform.position, transform.up, 15f, _playerMask);
        }
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
        if (gameState == GameManager.GameStates.Playing)
        {
            _target = FindObjectOfType<PlayerShip>(true)?.transform;
            _playerMask = 1 << LayerMask.NameToLayer("Player");
            _coolDown = _fireDelay;
        }
    }



    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying)
        {
            return;
        }

        if (CanFire)
        {
            LaunchMissile();
        }
    }

    private void LaunchMissile()
    {
        _coolDown = _fireDelay;
        var missile = Instantiate(_missilePrefab, _muzzle.position, transform.rotation);
        missile.GetComponent<SeekingMissile>()?.SetTarget(_target);
        SoundManager.Instance.PlaySoundEffect(_launchSound);
    }
}
