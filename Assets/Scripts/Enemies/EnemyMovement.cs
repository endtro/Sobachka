using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;

    private Vector3[] _path;
    private int _pathIndex;

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Move(Vector3[] path)
    {
        _path = path;
        _pathIndex = 0;

        while (_pathIndex < path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, path[_pathIndex], _moveSpeed * Time.deltaTime);

            if (transform.position == path[_pathIndex])
            {
                _pathIndex++;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        if (_path != null)
        {
            if (_pathIndex < _path.Length)
            {
                Gizmos.DrawLine(transform.position, _path[_pathIndex]);
            }

            for (int i = _pathIndex; i < _path.Length - 1; i++)
            {
                Gizmos.DrawLine(_path[i], _path[i + 1]);
            }
        }
    }
}
