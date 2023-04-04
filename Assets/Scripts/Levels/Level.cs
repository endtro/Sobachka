using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private float _scrollSpeed;
    // Здесть должно быть число клеток на карте по вертикали, за вычетом тех, что повторяют начало, для незаметной "склейки". Это - кастыль,
    // по изначальной задумке, карта вообще не должна повторяться.
    [SerializeField] private int _cellCount;

    private Camera _camera;
    private Grid _grid;
    private EnemySpawner[] _enemySpawners;
    // По-хорошему следовало бы вынести восстановление положения камеры в позицию по умолчанию в LevelManager.
    private Vector3 _cameraOriginalPosition;
    private float _gridLength;
    private float _nextGridShiftDistance;
    private float _levelProgress;
    private int _nextEnemy;
    private float _screenTopEdge;
    private List<Enemy> _activeEnemies = new List<Enemy>();

    private void Awake()
    {
        _camera = Camera.main;
        _grid = GetComponentInChildren<Grid>();

        _enemySpawners = gameObject.GetComponentsInChildren<EnemySpawner>().OrderBy(enemy => enemy.transform.position.y).ToArray();

        _gridLength = _cellCount * _grid.cellSize.y;
        _screenTopEdge = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y;
    }

    private void OnEnable()
    {
        _levelProgress = _screenTopEdge;
        _nextGridShiftDistance = _gridLength;
        _cameraOriginalPosition = Camera.main.transform.position;

        _grid.transform.position = Vector3.zero;
        FindObjectOfType<Player>().SetPosition(Vector3.zero);

        _nextEnemy = 0;

        if (_enemySpawners.Length > 0)
        {
            StartCoroutine(SpawEnemies());
        }
    }

    private void OnDisable()
    {
        DeactivateAllEnemies();

        StopAllCoroutines();

        Camera.main.transform.position = _cameraOriginalPosition;
    }

    private void Update()
    {
        float levelProgressDelta = _scrollSpeed * Time.deltaTime;
        _levelProgress += levelProgressDelta;

        Vector3 movementOffset = new Vector3(0f, levelProgressDelta, 0f);
        _camera.transform.Translate(movementOffset);

        if (_levelProgress > _nextGridShiftDistance)
        {
            _grid.transform.position = new Vector3(0f, _levelProgress - _screenTopEdge, 0f);
            _nextGridShiftDistance += _gridLength;
        }
    }

    public void RemoveActiveEnemy(Enemy enemy)
    {
        _activeEnemies.Remove(enemy);
    }

    public void DeactivateAllEnemies()
    {
        for (int i = _activeEnemies.Count - 1; i > 0; i--)
        {
            _activeEnemies[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator SpawEnemies()
    {
        while (_nextEnemy < _enemySpawners.Length)
        {
            if (_enemySpawners[_nextEnemy].transform.position.y < _levelProgress)
            {
                Enemy enemy = _enemySpawners[_nextEnemy].Spawn(this);
                _activeEnemies.Add(enemy);

                _nextEnemy++;
            }

            yield return null;
        }

        StartCoroutine(CompleteLevel());
    }

    private IEnumerator CompleteLevel()
    {
        while (_activeEnemies.Count > 0)
        {
            yield return null;
        }

        LevelManager.Instance.CompleteLevel();
    }

    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(Screen.width * -1f, _levelProgress, 0f), new Vector3(Screen.width, _levelProgress, 0f));
        }
    }
}
