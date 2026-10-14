// ========================================================
// 作者：娇娇 
// 创建时间：2026-10-11 14:51:25
// 版本：V1.1
// 描述：
// ========================================================
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}
public enum ShootType
{
    Single,
    Auto
}

[System.Serializable]
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting apesifics")]
    public ShootType shootType;
    public int bulletsPerShot;
    public float defaultFireRate;
    public float fireRate = 1;
    private float lastShotTime;

    [Header("Burst fire")]
    public bool burstAvalible;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1, 5)]
    public float reloadSpeed = 1;
    [Range(1, 5)]
    public float equipmentSpeed = 1;
    [Range(2, 12)]
    public float gunDistance = 4;

    [Header("Spread ")]
    public float baseSpread = 1;
    public float maximumSpread = 3;
    private float currentSpread = 2;

    public float spreadIncreaseRate = .15f;

    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1f;

    #region Spread methods
    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();
        float randomizedValue = Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }
    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();
        lastSpreadUpdateTime = Time.time;
    }
    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }
    #endregion

    #region Burst methods

    public bool BurstActivated()
    {
        if(weaponType == WeaponType.Shotgun)
        { 
            burstFireDelay = 0;
            return true;
        }

        return burstActive;
    } 
 
    public void ToggleBurst()
    {
        if (burstAvalible == false)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion
    public bool CanShoot() => HaveEnoughBullets() && ReadToFire();

    private bool ReadToFire()
    {
        if (Time.time > lastShotTime + 1 / fireRate)
        {
            lastShotTime = Time.time;
            return true;
        }
        return false;
    }


    #region Reload methods
    public bool CanReload()
    {
        if (bulletsInMagazine == magazineCapacity)
            return false;

        if (totalReserveAmmo > 0)
            return true;

        return false;
    }
    public void RefillBullets()
    {
        int bulletToReload = magazineCapacity;

        if (bulletToReload > totalReserveAmmo)
            bulletToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletToReload;
        bulletsInMagazine = bulletToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
    private bool HaveEnoughBullets() => bulletsInMagazine > 0;
    #endregion
}
