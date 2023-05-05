using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyShooting))]
public class Enemy : PoolObject, IDestructible
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private int _scoreReward;

    private EnemyMovement _enemyMovement;
    private EnemyShooting _enemyShooting;
    private SpriteRenderer _spriteRenderer;
    private Level _level;
    private float _health;

    public Vector3 SpriteExtents => _spriteRenderer.sprite.bounds.extents;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _enemyMovement = GetComponent<EnemyMovement>();
        _enemyShooting = GetComponent<EnemyShooting>();
    }

    private void OnDisable()
    {
        _level.RemoveActiveEnemy(this);
        StopAllCoroutines();
    }

    public void Initialize(ref Vector3[] path, Level level)
    {
        _health = _maxHealth;

        _level = level;

        StartCoroutine(_enemyMovement.Move(path));
        StartCoroutine(_enemyShooting.Shoot());
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
}
