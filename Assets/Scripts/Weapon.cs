// ========================================================
// 作者：娇娇 
// 创建时间：2026-10-11 14:51:25
// 版本：V1.1
// 描述：
// ========================================================
public enum WeaponType 
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}


[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    public bool CanShoot()
    {
        return HaveEnoughBullets();
    }
    private bool HaveEnoughBullets()
    {
        if (bulletsInMagazine > 0)
        {
            bulletsInMagazine--;
            return true;
        }
        return false;
    }

    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;

        if(totalReserveAmmo>0)
            return true;

        return false;
    }

    public void RefillBullets()
    {
        int bulletToReload = magazineCapacity;

        if(bulletToReload > totalReserveAmmo) 
            bulletToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletToReload;
        bulletsInMagazine = bulletToReload;

        if(totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
}
