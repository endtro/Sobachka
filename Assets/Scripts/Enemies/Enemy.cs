using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : PoolObject<Enemy>, IDestructible
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private int _scoreReward;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private int _damage;
    [SerializeField] private float _shootDelay;

    private SpriteRenderer _spriteRenderer;
    private Level _level;
    private float _health;
    private Vector3[] _path;
    private int _pathIndex;
    private float _nextShootTime;
    private WaitForSeconds _shootDelayWaitForSeconds;

    public Vector3 SpriteExtents => _spriteRenderer.sprite.bounds.extents;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _shootDelayWaitForSeconds = new WaitForSeconds(_shootDelay);
    }

    private void OnDisable()
    {
        _pathIndex = 0;
        _level.RemoveActiveEnemy(this);
        StopAllCoroutines();
    }

    public void Initialize(ref Vector3[] path, Level level)
    {
        _path = path;
        _level = level;

        _health = _maxHealth;

        StartCoroutine(Move());
        StartCoroutine(Shoot());
    }

    public void LoseHealth(float value)
    {
        _health -= value;

        if (_health <= 0)
        {
            Player.Instance.ChangeScore(_scoreReward);

            ObjectPooling<Enemy>.Return(this);
        }
    }

    private IEnumerator Move()
    {
        while (_pathIndex < _path.Length)
        {
            transform.position = Vector3.MoveTowards(transform.position, _path[_pathIndex], _moveSpeed * Time.deltaTime);

            if (transform.position == _path[_pathIndex])
            {
                _pathIndex++;
            }

            yield return null;
        }

        gameObject.SetActive(false);
    }

    private IEnumerator Shoot()
    {
        yield return _shootDelayWaitForSeconds;

        while (gameObject.activeSelf)
        {
            if (_nextShootTime < Time.time)
            {
                Projectile projectile = ObjectPooling<Projectile>.Get(_projectile);

                projectile.transform.position = transform.position;

                Vector3 playerDirection = Player.Instance.transform.position - projectile.transform.position;
                projectile.transform.rotation = Quaternion.FromToRotation(Vector3.up, playerDirection);

                projectile.Setup(gameObject.tag, _damage);

                _nextShootTime = Time.time + _shootDelay;

                yield return _shootDelayWaitForSeconds;
            }
        }
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
