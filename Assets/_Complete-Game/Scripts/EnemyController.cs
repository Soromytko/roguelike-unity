using UnityEngine;

[RequireComponent(typeof(Entity))]
public class EnemyController : MonoBehaviour
{
    private Entity _enemy;
    private Player _player;

    private void Awake()
    {
        _enemy = GetComponent<Entity>();
        _player = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _player.MoveEventHandler += OnPlayerMove;
    }

    private void OnDisable()
    {
        _player.MoveEventHandler -= OnPlayerMove;
    }

    private void OnPlayerMove()
    {
        int randDir = Random.Range(0, 2) == 0 ? -1 : 1;
        if (Random.Range(0, 2) == 0)
            _enemy.MoveX(randDir);
        else
            _enemy.MoveY(randDir);
    }

}
