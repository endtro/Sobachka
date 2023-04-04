using UnityEngine;

[CreateAssetMenu(fileName = "NewSpaceplane", menuName = "Module/Spaceplane", order = 51)]
public class Spaceplane : Module
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector3 _frontWeaponPosition;
    [SerializeField] private Vector3 _rearWeaponPositionLeft;
    [SerializeField] private Vector3 _rearWeaponPositionRight;

    public float MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;
    public Vector3 FrontWeaponPosition => _frontWeaponPosition;
    public Vector3 RearWeaponPositionLeft => _rearWeaponPositionLeft;
    public Vector3 RearWeaponPositionRight => _rearWeaponPositionRight;

    public GameObject Spawn(Transform transform)
    {
        return Instantiate(_prefab, transform.position, Quaternion.identity, transform);
    }
}
