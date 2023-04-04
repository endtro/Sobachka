using UnityEngine;

[CreateAssetMenu(fileName = "NewVolleyGun", menuName = "Module/Weapon/Volley Gun", order = 51)]
public class VolleyGun : Weapon
{
    [SerializeField] private float _coneSize;
    [SerializeField] private int _projectileCount;

    public override float ShootFrontWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPosition)
    {
        if (nextShootTime < Time.time && player.Power > PowerPerShot)
        {
            float[] shootDirections = new float[_projectileCount];

            float coneLeftEdge = _coneSize * 0.5f * -1f;
            float deltaAngle = _coneSize / (shootDirections.Length - 1);

            for (int i = 0; i < shootDirections.Length; i++)
            {
                PoolObject projectile = ObjectPooling.Get(ProjectilePrefab);

                projectile.transform.position = weaponPosition;
                projectile.transform.rotation = Quaternion.Euler(0f, 0f, coneLeftEdge + deltaAngle * i);

                projectile.GetComponent<Projectile>().Setup(ownerTag, Damage);
            }

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
