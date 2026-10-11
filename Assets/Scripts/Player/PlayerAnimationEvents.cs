// ========================================================
// 作者：娇娇 
// 创建时间：2026-09-28 17:07:25
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    private PlayerWeaponVisuals visualcontroller;
    private PlayerWeaponControlls weaponControlls;  

    private void Start()
    {
        visualcontroller = GetComponentInParent<PlayerWeaponVisuals>();      
        weaponControlls = GetComponentInParent<PlayerWeaponControlls>();
    }

    public void ReloadIsOver()
    {
        visualcontroller.MaximazeRigWeight();
        weaponControlls.CurrentWeapon().RefillBullets();
    }
    public void ReturnRig()
    {
        visualcontroller.MaximazeRigWeight();
        visualcontroller.MaximazeLeftHandWeight();
    }
    public void WeaponGrabIsOver()
    {
        
        visualcontroller.SetBusyGrabbingWeaponTo(false);
    }
}
