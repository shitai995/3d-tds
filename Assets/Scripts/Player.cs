// ========================================================
// 作者：娇娇 
// 创建时间：2026-09-27 16:11:26
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerControlls controls {  get; private set; }
    public PlayerAim aim {  get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponControlls weapon { get; private set; }
    private void Awake()
    {
        controls = new PlayerControlls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weapon = GetComponent<PlayerWeaponControlls>();
    }
    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
    }
}
