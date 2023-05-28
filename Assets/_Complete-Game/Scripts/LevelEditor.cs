using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public Vector2Int FieldSize => _fieldSize;
    [SerializeField] private Vector2Int _fieldSize = new Vector2Int(10, 10);

    [SerializeField] private Vector2Int _playerPoint;
    [SerializeField] private Vector2Int[] _exitPoints;
    [SerializeField] private Vector2Int[] _wallPoints;

    private void Draw(Vector2Int[] array, Color color)
    {
        if (array == null)
            return;

        Gizmos.color = color;
        for (int i = 0; i < array.Length; i++)
            Gizmos.DrawCube((Vector2)array[i] - Vector2.one / 2, Vector2.one);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        for (int i = 0; i < _fieldSize.x; i++)
        {
            for (int j = 0; j < _fieldSize.y; j++)
            {
                Vector3 position = new Vector2(i, j) - (Vector2)(_fieldSize - Vector2Int.one) / 2;

                if (i == 0 || i == _fieldSize.x - 1 || j == 0 || j == _fieldSize.y - 1)
                    Gizmos.DrawCube(position, Vector2.one);
                else
                    Gizmos.DrawWireCube(position, Vector2.one);
            }
        }

        Gizmos.color = Color.white;
            Gizmos.DrawCube((Vector2)_playerPoint - Vector2.one / 2, Vector2.one);

        Draw(_exitPoints, Color.green);
        Draw(_wallPoints, Color.red);
        
    }
}
