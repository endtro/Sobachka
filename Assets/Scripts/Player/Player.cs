using UnityEngine;
using UnityEngine.Events;

// ¬озможно, имело бы смысл разбить этот класс на два - на "игрока" и "компоненты самолета"
[RequireComponent(typeof(Movement))]
[RequireComponent(typeof(Shooting))]
public class Player : MonoBehaviour, IDestructible
{
    [SerializeField] private Spaceplane _spaceplane;
    [SerializeField] private Weapon _frontWeapon;
    [SerializeField] private Weapon _rearWeapon;
    [SerializeField] private Generator _generator;
    [SerializeField] private Shield _shield;

    private static Player _instance;

    private Movement _movement;
    private Shooting _shooting;
    private GameObject _visuals;
    private float _health;
    private float _energy;
    private float _power;
    [SerializeField] private int _score;
    private UnityEvent<float> _healthChanged = new UnityEvent<float>();
    private UnityEvent<float> _maxHealthChanged = new UnityEvent<float>();
    private UnityEvent<float> _energyChanged = new UnityEvent<float>();
    private UnityEvent<float> _maxEnergyChanged = new UnityEvent<float>();
    private UnityEvent<float> _powerChanged = new UnityEvent<float>();
    private UnityEvent<float> _maxPowerChanged = new UnityEvent<float>();
    private UnityEvent<int> _scoreChanged = new UnityEvent<int>();

    public static Player Instance => _instance;

    public bool ShootingEnabled => _shooting.enabled;
    public Spaceplane Spaceplane => _spaceplane;
    public Weapon FrontWeapon => _frontWeapon;
    public Weapon RearWeapon => _rearWeapon;
    public Generator Generator => _generator;
    public Shield Shield => _shield;
    public float Health => _health;
    public float MaxHealth => _spaceplane.MaxHealth;
    public float Energy => _energy;
    public float MaxEnergy => _shield.MaxEnergy;
    public float Power => _power;
    public float MaxPower => _generator.MaxPower;
    public int Score => _score;
    public event UnityAction<float> HealthChanged
    {
        add => _healthChanged.AddListener(value);
        remove => _healthChanged.RemoveListener(value);
    }
    public event UnityAction<float> MaxHealthChanged
    {
        add => _maxHealthChanged.AddListener(value);
        remove => _maxHealthChanged.RemoveListener(value);
    }
    public event UnityAction<float> EnergyChanged
    {
        add => _energyChanged.AddListener(value);
        remove => _energyChanged.RemoveListener(value);
    }
    public event UnityAction<float> MaxEnergyChanged
    {
        add => _maxEnergyChanged.AddListener(value);
        remove => _maxEnergyChanged.RemoveListener(value);
    }
    public event UnityAction<float> PowerChanged
    {
        add => _powerChanged.AddListener(value);
        remove => _powerChanged.RemoveListener(value);
    }
    public event UnityAction<float> MaxPowerChanged
    {
        add => _maxPowerChanged.AddListener(value);
        remove => _maxPowerChanged.RemoveListener(value);
    }
    public event UnityAction<int> ScoreChanged
    {
        add => _scoreChanged.AddListener(value);
        remove => _scoreChanged.RemoveListener(value);
    }

    private void Awake()
    {
        _instance = this;

        _movement = GetComponent<Movement>();
        _shooting = GetComponent<Shooting>();
    }

    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        GenerateEnergy();
    }

    public void LoseHealth(float value)
    {
        _energy -= value;

        if (_energy < 0f)
        {
            _health += _energy;
            _energy = 0f;

            _healthChanged.Invoke(_health);
        }

        _energyChanged.Invoke(_energy);

        if (_health <= 0f)
        {
            _health = 0f;

            LevelManager.Instance.BackToShop();
        }
    }

    public bool LosePower(float value)
    {
        if (_power > value)
        {
            _power -= value;

            _powerChanged.Invoke(_power);

            return true;
        }
        else
        {
            return false;
        }
    }

    public void ChangeScore(int value)
    {
        _score += value;

        _scoreChanged.Invoke(_score);
    }

    public void SetSpaceplane(Spaceplane spaceplane)
    {
        _spaceplane = spaceplane;

        if (_visuals != null)
        {
            Destroy(_visuals);
        }

        _visuals = _spaceplane.Spawn(gameObject.transform);

        _movement.SetMoveSpeed(_spaceplane.MoveSpeed + _generator.MoveSpeed);
        _movement.SetAnimator(_visuals.GetComponent<Animator>());

        _health = _spaceplane.MaxHealth;

        _maxHealthChanged.Invoke(_spaceplane.MaxHealth);
        _healthChanged.Invoke(_health);
    }

    public void SetFrontWeapon(Weapon weapon)
    {
        _frontWeapon = weapon;
    }

    public void SetRearWeapon(Weapon weapon)
    {
        _rearWeapon = weapon;
    }

    public void SetGenerator(Generator generator)
    {
        _generator = generator;

        _movement.SetMoveSpeed(_spaceplane.MoveSpeed + _generator.MoveSpeed);

        _maxPowerChanged.Invoke(_generator.MaxPower);
        _powerChanged.Invoke(_power);
    }

    public void SetShield(Shield shield)
    {
        _shield = shield;

        _energy = _shield.MaxEnergy;

        _maxEnergyChanged.Invoke(_shield.MaxEnergy);
        _energyChanged.Invoke(_energy);
    }

    public void EnableShooting(bool enabled)
    {
        _shooting.enabled = enabled;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
    }

    private void Setup()
    {
        SetSpaceplane(_spaceplane);
        SetGenerator(_generator);
        SetShield(_shield);
    }

    private void GenerateEnergy()
    {
        if (_power < _generator.MaxPower)
        {
            _power += _generator.PowerGeneration * Time.deltaTime;

            _powerChanged.Invoke(_power);
        }
        if (_energy < _shield.MaxEnergy)
        {
            float energyFullness = 100f / _generator.MaxPower * _power * 0.01f;
            _energy += _generator.PowerGeneration * _shield.EnergyUsageEfficiency * energyFullness * Time.deltaTime;

            _energyChanged.Invoke(_energy);
        }
    }
}
