using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    [SerializeField] private Vector2Int _fieldSize = new Vector2Int(10, 10);
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _exit;
    [SerializeField] private Text _dayText;
    [SerializeField] private LevelData[] _levelsData;
    [SerializeField] private GameObject[] _outerWalls;
    [SerializeField] private GameObject[] _food;
    [SerializeField] private GameObject[] _floors;
    [SerializeField] private GameObject[] _walls;
    [SerializeField] private GameObject[] _enemies;

    private GameObject _currentLevelData;
    private List<Vector3> _freePositions = new List<Vector3>();
    private Vector2 _startPosition;
    private Vector2 _endPosition;

    private int _currentLavelIndex;

    private void Awake()
    {
        Vector2 offset = (Vector2)(_fieldSize - Vector2Int.one) / 2f;
        _startPosition = -offset;
        _endPosition = offset;

        GenerateField();
        LoadLevel(0);
    }

    public void LoadNextLevel()
    {
        LoadLevel(_currentLavelIndex + 1);
    }

    public void LoadLevel(int levelIndex)
    {
        if (levelIndex >= _levelsData.Length)
            return;

        _currentLavelIndex = levelIndex;

        StartCoroutine(Heyday());


        InitFreePositions();
        Destroy(_currentLevelData);
        _currentLevelData = Instantiate(_levelsData[levelIndex]).gameObject;
        CreateObjectByFreePosition(_player, _startPosition + Vector2.one);
        CreateObjectByFreePosition(_exit, _endPosition - Vector2.one);

        FindObjectOfType<Player>().DieEventHandler += OnPlayerDie;

        GenerateObjectAtRandom(_food, _levelsData[levelIndex].FoodCount);
        GenerateObjectAtRandom(_walls, _levelsData[levelIndex].WallCount);
        GenerateObjectAtRandom(_enemies, _levelsData[levelIndex].EnemyCount);
    }

    private void InitFreePositions()
    {
        _freePositions.Clear();

        for (float x = _startPosition.x + 1; x <= _endPosition.x - 1; x++)
            for (float y = _startPosition.y + 1; y <= _endPosition.y - 1; y++)
                _freePositions.Add(new Vector2(x, y));
    }

    private void GenerateOuterWallAndFloors()
    {
        for (float x = _startPosition.x; x <= _endPosition.x; x++)
        {
            for (float y = _startPosition.y; y <= _endPosition.y; y++)
            {
                GameObject unit = null;

                if (x == _startPosition.x || x == _endPosition.x || y == _startPosition.y || y == _endPosition.y)
                    unit = _outerWalls[Random.Range(0, _outerWalls.Length)];
                else
                    unit = _floors[Random.Range(0, _floors.Length)];

                Instantiate(unit, new Vector2(x, y), Quaternion.identity);//.transform.SetParent(_currentLevelEditorObj.transform);
            }
        }
    }

    void CreateObjectByFreePosition(GameObject obj, int freePositionIndex)
    {
        if (freePositionIndex < 0 || freePositionIndex >= _freePositions.Count)
            return;

        Instantiate(obj, _freePositions[freePositionIndex], Quaternion.identity).transform.SetParent(_currentLevelData.transform);
        _freePositions.RemoveAt(freePositionIndex);
    }

    void CreateObjectByFreePosition(GameObject obj, Vector2 freePosition)
    {
        CreateObjectByFreePosition(obj, _freePositions.IndexOf(freePosition));
    }

    private void GenerateObjectAtRandom(GameObject[] array, int count)
    {
        for (int i = 0; i < count; i++)
        {
            int randIndex = Random.Range(0, array.Length);
            int pointRandIndex = Random.Range(0, _freePositions.Count);
            CreateObjectByFreePosition(array[randIndex], pointRandIndex);
        }
    }

    private void GenerateField()
    {
        GenerateOuterWallAndFloors();
    }

    private IEnumerator Heyday()
    {
        _dayText.text = $"Day {_currentLavelIndex + 1}";
        _dayText.transform.parent.gameObject.SetActive(true);

        float time = 0;
        while(time < 2f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _dayText.transform.parent.gameObject.SetActive(false);
    }

    private void OnPlayerDie()
    {
        StartCoroutine(GameOver());
    }

    private IEnumerator GameOver()
    {
        _dayText.text = "Game over";
        _dayText.transform.parent.gameObject.SetActive(true);

        float time = 0;
        while (time < 1f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _dayText.transform.parent.gameObject.SetActive(false);

        LoadLevel(0);
    }

    

}
