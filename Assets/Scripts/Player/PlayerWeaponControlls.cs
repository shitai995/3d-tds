// ========================================================
// 作者：娇娇 
// 创建时间：2026-09-27 16:17:01
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponControlls : MonoBehaviour
{
    private Player player;
    private const float REFERENCE_BULLET_SPEED = 20;

    [SerializeField] private Weapon currentWeapon;

    [Header("Bullet details")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform gunPoint;

    [SerializeField] private Transform weaponHolder;

    [Header("Inventory")]
    [SerializeField] private int maxSlots = 2;
    [SerializeField] private List<Weapon> weaponSlots;
    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();

        currentWeapon.bulletsInMagazine = currentWeapon.totalReserveAmmo;
    }

    #region Slots managment

    private void EquipWeapon(int i)
    {
        currentWeapon = weaponSlots[i];
    }
    public void PickupWeapon(Weapon newWeapon)
    {
        if (weaponSlots.Count >= maxSlots)
            return;

        weaponSlots.Add(newWeapon);
    }
    private void DropWeapon()
    {
        if (weaponSlots.Count <= 1)
            return;

        weaponSlots.Remove(currentWeapon);

        currentWeapon = weaponSlots[0];
    }

    #endregion
    private void Shoot()
    {
        if (currentWeapon.CanShoot() == false)
            return;

        GameObject newBullet =
            Instantiate(bulletPrefab, gunPoint.position, Quaternion.LookRotation(gunPoint.forward));

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        rbNewBullet.mass = REFERENCE_BULLET_SPEED / bulletSpeed;
        rbNewBullet.velocity = BulletDirection() * bulletSpeed;

        Destroy(newBullet, 10);
        GetComponentInChildren<Animator>().SetTrigger("Fire");
    }
    public Vector3 BulletDirection()
    {
        Transform aim = player.aim.Aim();

        Vector3 direction = (aim.position - gunPoint.position).normalized;

        if (player.aim.CanAimPrecisly() == false && player.aim.Target() == null)
            direction.y = 0;

        //weaponHolder.LookAt(aim);
        //gunPoint.LookAt(aim);

        return direction;
    }
    public Weapon CurrentWeapon() => currentWeapon;
    public Transform GunPoint() => gunPoint;

    #region Input Events
    private void AssignInputEvents()
    {
        PlayerControlls controls = player.controls;
        controls.Character.Fire.performed += ctx => Shoot();

        controls.Character.EquipSlot1.performed += ctx => EquipWeapon(0);
        controls.Character.EquipSlot2.performed += ctx => EquipWeapon(1);

        controls.Character.DropCurrentWeapon.performed += ctx => DropWeapon();

        controls.Character.Reload.performed += ctx =>
        {
            if (currentWeapon.CanReload())
                player.weaponVisuals.PlayReloadAnimation();
        };
    }
    #endregion
     
}
