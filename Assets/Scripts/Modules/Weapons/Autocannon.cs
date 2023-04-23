using UnityEngine;

[CreateAssetMenu(fileName = "NewAutocannon", menuName = "Module/Weapon/Autocannon", order = 51)]
public class Autocannon : Weapon
{
    [SerializeField] private float _spread;

    public override float ShootFrontWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPosition)
    {
        if (nextShootTime < Time.time && player.Power > PowerPerShot)
        {
            float spreadHalved = _spread * 0.5f;
            Quaternion spread = Quaternion.Euler(0f, 0f, Random.Range(spreadHalved * -1f, spreadHalved));

            Projectile projectile = ObjectPooling<Projectile>.Get(ProjectilePrefab);

            projectile.transform.position = weaponPosition;
            projectile.transform.rotation = spread;

            projectile.Setup(ownerTag, Damage);

            player.LosePower(PowerPerShot);

            nextShootTime = Time.time + ShootDelay;
        }

        return nextShootTime;
    }

    public override float ShootRearWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPositionLeft, Vector3 weaponPositionRight)
    {
        return nextShootTime;
    }
}
