// ========================================================
// 作者：娇娇 
// 创建时间：2026-10-13 00:50:51
// 版本：V1.1
// 描述：
// ========================================================

using UnityEngine;

public enum EquipType { SideEquipAnimation, BackEquipAnimation  };
public enum HoldType { CommonHold = 1, LowHold, HighHold }
public class WeaponModel : MonoBehaviour
{
    public WeaponType weaponType;
    public EquipType equipAnimationType;
    public HoldType holdType;

    public Transform gunPoint;
    public Transform holdPoint;
}
 