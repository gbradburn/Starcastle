using UnityEngine;

public class Starcastle : MonoBehaviour, IDamageable
{
    [SerializeField] GameObject _explosionPrefab;
    [SerializeField] AudioClip _explosionSound;

    private void DestroyStarCastle()
    {
        Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        SoundManager.Instance.PlaySoundEffect(_explosionSound);
        Destroy(gameObject);
        GameManager.Instance.StarcastleDestroyed();
    }

    public void TakeDamage(int damage)
    {
        DestroyStarCastle();
    }
}
