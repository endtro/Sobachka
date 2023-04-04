using UnityEngine;

public abstract class Weapon : Module
{
    [SerializeField] private protected int Damage;
    [SerializeField] private protected Projectile ProjectilePrefab;
    [SerializeField] private protected float ShootDelay;
    [SerializeField] private protected float PowerPerShot;

    public abstract float ShootFrontWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPosition);

    // ¬место двух режимов стрельбы следовало сделать два различных класса под каждый тип оружи€. Ёто привело к трудност€м в написании магазина.
    // Ќичего переделывать €, конечно, не буду. Ќазовем это "урок на будущее".
    public abstract float ShootRearWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPositionLeft, Vector3 weaponPositionRight);
}
