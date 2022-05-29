using UnityEngine;

public class SeekingMissile : MonoBehaviour, IDamageable
{
    [SerializeField] float _speed = 2f;
    [SerializeField] float _turnSpeed = 0.75f;
    [SerializeField] float _duration = 5f;
    [SerializeField] float _levelSpeedAdjustment = 0.25f;
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] AudioClip _explosionSound;
    [SerializeField] int _points = 50;

    private Transform _target;

    private float LevelSpeedAdjustment => GameManager.Instance.LevelManager.Level * _levelSpeedAdjustment;
    private float Speed => _speed + LevelSpeedAdjustment;
    private float TurnSpeed => _turnSpeed + LevelSpeedAdjustment;
    private float Duration => _duration + GameManager.Instance.LevelManager.Level;

    private float _missileDuration;

    public void SetTarget(Transform target)
    {
        _target = target;
        Debug.Log($"misileSpeed={Speed}, turnSpeed={TurnSpeed}, Duration={Duration}");
    }

    private void OnEnable()
    {
        _missileDuration = Duration;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.GameState != GameManager.GameStates.Playing)
        {
            BlowUp();
        }
        _missileDuration -= Time.deltaTime;
        if (_missileDuration <= 0f) BlowUp();
    }

    private void FixedUpdate()
    {
        if (!GameManager.Instance.IsPlaying || _target == null)
        {
            BlowUp();
            return;
        }
        Vector3 vectorToTarget = _target.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.fixedDeltaTime * TurnSpeed);
        transform.Translate(Vector3.up * Speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(1);
        BlowUp();

    }

    public void TakeDamage(int damage)
    {
        ScoreManager.Instance.AddScore(_points);
        BlowUp();
    }

    private void BlowUp()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundEffect(_explosionSound);
        Destroy(gameObject);
    }

}
