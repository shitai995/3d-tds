// ========================================================
// 作者：娇娇 
// 创建时间：2026-09-29 21:02:17
// 版本：V1.1
// 描述：
// ========================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }
}
