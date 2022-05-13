using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : MonoBehaviour
{
    Transform _target;
    [SerializeField] [Range(0, 3f)] float _rotateSpeed = 0.5f;
    [SerializeField] [Range(0, 5f)] float _initialDelay = 5f;
    [SerializeField] float _shootForce = 1000f;
    [SerializeField] GameObject _projectilePrefab = null;
    [SerializeField] AudioClip _fireSound = null;
    [SerializeField] Transform _muzzle;

    float _coolDown;

    bool CanFire
    {
        get
        {
            _coolDown -= Time.deltaTime;
            return _coolDown <= 0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _target = FindObjectOfType<PlayerShip>(true).transform;
    }

    private void OnEnable()
    {
        _coolDown = _initialDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsPlaying) return;
        Vector3 vectorToTarget = _target.position - transform.position;
        float angle = (Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg) - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * _rotateSpeed);

        if (CanFire)
        {
            FireProjectile();
        }
    }

    private void FireProjectile()
    {
        _coolDown = _initialDelay;
        GameObject projectile = Instantiate(_projectilePrefab, _muzzle.position, transform.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(transform.up * _shootForce);
        SoundManager.Instance.PlaySoundEffect(_fireSound);
    }
}
