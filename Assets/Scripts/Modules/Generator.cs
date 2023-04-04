using UnityEngine;

[CreateAssetMenu(fileName = "NewGenerator", menuName = "Module/Generator", order = 51)]
public class Generator : Module
{
    [SerializeField] private float _maxPower;
    [SerializeField] private float _powerGeneration;
    [SerializeField] private float _moveSpeed;

    public float MaxPower => _maxPower;
    public float PowerGeneration => _powerGeneration;
    public float MoveSpeed => _moveSpeed;
}
