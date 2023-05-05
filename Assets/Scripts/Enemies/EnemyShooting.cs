using System.Collections;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private int _damage;
    [SerializeField] private float _shootDelay;

    private float _nextShootTime;
    private WaitForSeconds _shootDelayWaitForSeconds;

    private void Awake()
    {
        _shootDelayWaitForSeconds = new WaitForSeconds(_shootDelay);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public IEnumerator Shoot()
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

}
