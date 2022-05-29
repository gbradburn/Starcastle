using UnityEngine;

public class RingSegment : MonoBehaviour, IDamageable
{
    [SerializeField] int _points = 10;

    public void TakeDamage(int damage)
    {
        ScoreManager.Instance.AddScore(_points);
        gameObject.SetActive(false);
    }

}
