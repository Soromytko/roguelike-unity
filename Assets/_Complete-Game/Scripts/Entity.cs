using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour, IDamageable
{
    public event Action MoveEventHandler;
    public event Action AttackEventHandler;
    public event Action TakeDamageEventHandler;

    [SerializeField] private int _health = 3;
    [SerializeField] private int _damage = 5;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private int _attackTimeout = 2;

    private Animator _animator;

    private bool _isMoving;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void MoveX(float stepValue) => Move(new Vector2(stepValue, 0));

    public void MoveY(float stepValue) => Move(new Vector2(0, stepValue));

    public void Attack()
    {
        _attackTimeout = (_attackTimeout + 1) % 2;

        if (_attackTimeout == 0)
            return;

        Player player = FindObjectOfType<Player>();
        _animator.Play("Attack");
        player.TakesDamage(_damage);
    }

    public void TakeDamage(int damage)
    {
        TakeDamageEventHandler?.Invoke();
        _health -= damage;
        if (_health <= 0)
            Destroy(gameObject);

        Attack();
    }

    private void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        if (_isMoving)
            return;

        int mask = LayerMask.GetMask("BlockingLayer");// | LayerMask.GetMask("Entity");
        Collider2D nearCollider = GetNearCollider(direction, mask);

        if (nearCollider == null)
        {
            StartCoroutine(Moving(direction));
            return;
        }

        Collider2D[] nearest = {
            Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f),
            Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f),
            Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f),
            Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f)
        };

        foreach (Collider2D col in nearest)
        {
            if (col.TryGetComponent(out Player player))
            {
                player.TakesDamage(_damage);
                _animator.Play("Attack");
                return;
            }
        }

    }

    private Collider2D GetNearCollider(Vector2 direction, int mask)
    {
        return Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f, mask);
    }

    private IEnumerator Moving(Vector2 direction)
    {
        MoveEventHandler?.Invoke();

        _isMoving = true;
        Vector3 targetPosition = transform.position + (Vector3)direction;
        while (transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
            yield return null;
        }
        _isMoving = false;
    }



}
