using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurret : MonoBehaviour
{
    [SerializeField] GameObject _muzzle = null;
    [SerializeField] GameObject _projectilePrefab = null;
    [SerializeField] float _coolDown = 0.25f;
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _firingSound;
    [SerializeField] float _shootForce = 500f;

    float _fireDelay;
    Transform _transform;

    bool CanFire 
    {
        get
        {
            _fireDelay -= Time.deltaTime;
            return _fireDelay <= 0f;
        }
    }

    private void Start()
    {
        _transform = transform;
    }

    private void OnEnable()
    {
        _fireDelay = 0f;
    }

    void Update()
    {
        if (CanFire && Input.GetKey(KeyCode.M))
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        _fireDelay = _coolDown;
        GameObject projectile = Instantiate(_projectilePrefab, _muzzle.transform.position, _transform.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * _shootForce);
        _audioSource.PlayOneShot(_firingSound);
    }
}
