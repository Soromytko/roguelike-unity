using UnityEngine;

public class LevelData : MonoBehaviour
{
    public int FoodCount => _foodCount;
    public int WallCount => _wallCount;
    public int EnemyCount => _enemyCount;

    [SerializeField] private int _foodCount = 3;
    [SerializeField] private int _wallCount = 3;
    [SerializeField] private int _enemyCount = 3;
}
