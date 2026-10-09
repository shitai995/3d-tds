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
    [SerializeField] private GameObject bulletImpactFx;
    private Rigidbody rb => GetComponent<Rigidbody>();

    private void OnCollisionEnter(Collision collision)
    {
        CreateImpactFx(collision);
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        Destroy(gameObject);
    }

    private void CreateImpactFx(Collision collision)
    {
        if(collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];

            GameObject newImpactFx =
                Instantiate(bulletImpactFx, contact.point, Quaternion.LookRotation(contact.normal));

            Destroy(newImpactFx, 1f);
        }
    }
}
