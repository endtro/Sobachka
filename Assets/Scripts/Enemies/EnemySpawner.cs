using UnityEditor;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Enemy _prefab;
    [SerializeField] private Vector3[] _path;

    private Enemy _enemy;

    public Enemy Spawn(Level level)
    {
        _enemy = ObjectPooling<Enemy>.Get(_prefab);

        _enemy.Initialize(ref _path, level);
        _enemy.transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        return _enemy;
    }

    private void OnDrawGizmos()
    {
        if (EditorApplication.isPlaying == false)
        {
            if (_path.Length > 0)
            {
                Gizmos.DrawLine(transform.position, _path[0]);

                for (int i = 0; i < _path.Length - 1; i++)
                {
                    Gizmos.DrawLine(_path[i], _path[i + 1]);
                }
            }
        }
    }
}
