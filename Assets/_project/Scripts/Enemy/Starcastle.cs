using UnityEngine;

public class Starcastle : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] AudioClip _explosionSound;
    [SerializeField] int _points = 100;

    private void DestroyStarCastle()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundEffect(_explosionSound);
        Destroy(gameObject);
        GameManager.Instance.StarcastleDestroyed();
    }

    public void TakeDamage(int damage)
    {
        int points = _points + (GameManager.Instance.LevelManager.Level * _points);
        ScoreManager.Instance.AddScore(points);
        DestroyStarCastle();
    }
}
