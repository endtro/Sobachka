using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Player))]
public class Shooting : MonoBehaviour
{
    private Player _player;
    private float _frontWeaponNextShootTime;
    private float _rearWeaponNextShootTime;

    private void Awake()
    {
        _player = GetComponent<Player>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        _frontWeaponNextShootTime = _player.FrontWeapon.ShootFrontWeapon(_player, gameObject.tag, _frontWeaponNextShootTime,
            transform.position + _player.Spaceplane.FrontWeaponPosition);
        _rearWeaponNextShootTime = _player.RearWeapon.ShootRearWeapon(_player, gameObject.tag, _rearWeaponNextShootTime,
            transform.position + _player.Spaceplane.RearWeaponPositionLeft, transform.position + _player.Spaceplane.RearWeaponPositionRight);
    }
}
