using UnityEngine;

[CreateAssetMenu(fileName = "NewRocketLauncher", menuName = "Module/Weapon/Rocket Launcher", order = 51)]
public class RocketLauncher : Weapon
{
    [SerializeField] private float _spread;
    [SerializeField] private int _projectileCount;
    [SerializeField] private float _launchForce;

    public override float ShootFrontWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPosition)
    {
        if (nextShootTime < Time.time && player.Power > PowerPerShot)
        {
            for (int i = 0; i < _projectileCount; i++)
            {
                Vector2 force = new Vector2(Random.Range(_spread * -1f, _spread), 1f) * _launchForce;
                SpawnRocket(ownerTag, weaponPosition, force);
            }

            player.LosePower(PowerPerShot);

            nextShootTime = Time.time + ShootDelay;
        }

        return nextShootTime;
    }

    public override float ShootRearWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPositionLeft, Vector3 weaponPositionRight)
    {
        if (nextShootTime < Time.time && player.Power > PowerPerShot)
        {
            for (int i = 0; i < _projectileCount; i += 2)
            {
                Vector2 force = new Vector2(-1f, Random.Range(_spread * -1f, _spread)) * _launchForce;
                SpawnRocket(ownerTag, weaponPositionLeft, force);

                force = new Vector2(1f, Random.Range(_spread * -1f, _spread)) * _launchForce;
                SpawnRocket(ownerTag, weaponPositionRight, force);
            }

            player.LosePower(PowerPerShot);

            nextShootTime = Time.time + ShootDelay;
        }

        return nextShootTime;
    }

    private Rocket SpawnRocket(string ownerTag, Vector3 position, Vector2 force)
    {
        // Проблема
        Rocket projectile = (Rocket)ObjectPooling<Projectile>.Get(ProjectilePrefab);

        projectile.transform.position = position;
        projectile.transform.rotation = Quaternion.identity;

        projectile.Setup(ownerTag, Damage);
        projectile.AddForce(force);

        return projectile;
    }
}
