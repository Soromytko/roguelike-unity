using UnityEngine;

public class Wall : MonoBehaviour, IDamageable
{
    public int Health => _health;

    [SerializeField] private int _health = 5;

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if (_health <= 0)
            Destroy(gameObject);
    }
}
