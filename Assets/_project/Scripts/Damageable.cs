using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour, IDamageable
{
    [SerializeField] int _maxHealth = 1;
    [SerializeField] GameObject _explosionPrefab = null;
    [SerializeField] AudioClip _explosionSound = null;

    int _health;


    private void OnEnable()
    {
        _health = _maxHealth;
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            DestroyMe();
        }
    }

    private void DestroyMe()
    {
        SoundManager.Instance?.PlaySoundEffect(_explosionSound, 0.5f);
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
