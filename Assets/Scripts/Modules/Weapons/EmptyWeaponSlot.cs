using UnityEngine;

[CreateAssetMenu(fileName = "NewEmptyWeaponSlot", menuName = "Module/Weapon/Empty Weapon Slot", order = 51)]
public class EmptyWeaponSlot : Weapon
{
    public override float ShootFrontWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPosition)
    {
        return nextShootTime;
    }

    public override float ShootRearWeapon(Player player, string ownerTag, float nextShootTime, Vector3 weaponPositionLeft, Vector3 weaponPositionRight)
    {
        return nextShootTime;
    }
}
