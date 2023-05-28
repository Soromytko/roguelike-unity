using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Player : MonoBehaviour
{
    public event Action MoveEventHandler;
    public event Action AttackEventHandler;
    public event Action TakeDamageEventHandler;
    public event Action DieEventHandler;
    public event Action<int> TakeFoodEventHandler;
    public event Action<int> FoodCountChanged;

    public int FoodCount { get => _foodCount; private set { _foodCount = value; FoodCountChanged(value); } }
    [SerializeField] private int _foodCount = 50;
    [SerializeField] private float _moveSpeed = 2f;
    [SerializeField] private float _attackTimeout = 0.5f;
    [SerializeField] private int _stepCost = 1;
    [SerializeField] private int _attackCost = 1;

    private Animator _animator;

    private bool _isMoving;
    private float _attackCurrentTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        FoodCount = _foodCount; //for MoveEventHandler Invoke
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Food food))
        {
            TakeFood(food.Value);
            food.Destroy();
        }
    }

    public void MoveX(float stepValue) => Move(new Vector2(stepValue, 0));

    public void MoveY(float stepValue) => Move(new Vector2(0, stepValue));

    public void TakeFood(int value)
    {
        FoodCount += value;
        TakeFoodEventHandler(value);
    }

    public void TakesDamage(int damage)
    {
        DigestFood(damage);
        TakeDamageEventHandler?.Invoke();
    }

    private void Move(Vector2 direction)
    {
        if (direction == Vector2.zero)
            return;

        if (_isMoving)
            return;

        LayerMask mask = LayerMask.GetMask("BlockingLayer") | LayerMask.GetMask("Enemy");
        Collider2D nearCollider = GetNearCollider(direction, mask);

        if (nearCollider == null)
        {
            StartCoroutine(Moving(direction));
        }
        else if (nearCollider.TryGetComponent(out IDamageable dest))
        {
            Attack(dest);
            return;
        }
           

        
    }

    private void Attack(IDamageable obj)
    {
        if (obj == null)
            return;

        if (Time.time - _attackCurrentTime < _attackTimeout)
            return;

        obj.TakeDamage(1);

        _attackCurrentTime = Time.time;
        DigestFood(_attackCost);
        _animator.Play("Attack");
        AttackEventHandler?.Invoke();
    }

    private void DigestFood(int value)
    {
        FoodCount -= value;
        if (_foodCount < 0)
        {
            DieEventHandler?.Invoke();
        }
    }

    private Collider2D GetNearCollider(Vector2 direction, int mask)
    {
        return Physics2D.OverlapCircle((Vector2)transform.position + direction, 0.1f, mask);
    }

    private IEnumerator Moving(Vector2 direction)
    {
        DigestFood(_stepCost);
        MoveEventHandler?.Invoke();

        _isMoving = true;
        Vector3 targetPosition = transform.position + (Vector3)direction;
        while(transform.position != targetPosition)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * _moveSpeed);
            yield return null;
        }
        _isMoving = false;
    }





}
