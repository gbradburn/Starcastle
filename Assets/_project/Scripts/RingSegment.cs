using UnityEngine;

public class RingSegment : MonoBehaviour, IDamageable
{
    [SerializeField] int _points = 10;

    public void TakeDamage(int damage)
    {
        // tell game manager to add points
        Debug.Log($"Ring segment took {damage} damage.");
        gameObject.SetActive(false);
    }

}
