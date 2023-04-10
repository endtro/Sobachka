using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class Rocket : Projectile
{
    [SerializeField] private float _flightpathDeviation;
    [SerializeField] private float _startDelay;
    [SerializeField] private ParticleSystem _smokeTrail;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private float _startTime;
    private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        _spriteRenderer.enabled = true;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(OwnerTag) == false)
        {
            if (collision.TryGetComponent(out IDestructible destructible))
            {
                destructible.LoseHealth(Damage);

                StartCoroutine(Deactivate());
            }
        }
    }

    public override IEnumerator Move()
    {
        _startTime = Time.time + _startDelay;
        _smokeTrail.Stop();

        while (_startTime > Time.time)
        {
            yield return null;
        }

        // Наверно красивай было бы порезать ускорение пополам или до одной трети, но так проще.
        _rigidbody2D.velocity = Vector3.zero;
        _smokeTrail.Play();

        while (Time.time < DeathTime)
        {
            transform.Translate(MoveSpeed * Time.deltaTime * Vector3.up, Space.Self);
            transform.Rotate(0f, 0f, Random.Range(_flightpathDeviation * -1f, _flightpathDeviation) * Time.deltaTime);

            yield return _waitForFixedUpdate;
        }

        StartCoroutine(Deactivate());
    }

    public void AddForce(Vector2 force, ForceMode2D forceMode = ForceMode2D.Impulse)
    {
        _rigidbody2D.AddForce(force, forceMode);
    }

    private IEnumerator Deactivate()
    {
        _spriteRenderer.enabled = false;
        _smokeTrail.Stop();

        while (_smokeTrail.particleCount > 0)
        {
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
