using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [SerializeField] GameObject _hitEffect = null;
    [SerializeField] AudioClip _hitSound = null;
    [SerializeField] [Range(1f, 10f)] float _duration = 5f;
    [SerializeField] int _damage = 1;

    float _lifeTime;

    private void OnEnable()
    {
        _lifeTime = _duration;
    }

    // Update is called once per frame
    void Update()
    {
        _lifeTime -= Time.deltaTime;
        if (_lifeTime <= 0f) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        SoundManager.Instance.PlaySoundEffect(_hitSound);
        Instantiate(_hitEffect, transform.position, Quaternion.identity);
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_damage);
        Destroy(gameObject);
    }
}
