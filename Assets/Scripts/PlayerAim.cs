// ========================================================
// 作者：娇娇 
// 创建时间：2026-09-29 14:41:43
// 版本：V1.1
// 描述：
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    private Player player;
    private PlayerControlls controls;

    [Header("Aim Viusal - Laser")]
    [SerializeField] private LineRenderer aimLaser;
    
 
    [Header("Aim control")]
    [SerializeField] private Transform aim;

    [SerializeField] private bool isAimingPrecisly;
    [SerializeField] private bool isLockongToTarget;
    [Header("Camera control")]
    [SerializeField] private Transform cameraTarget;
    [Range(.5f,1)]
    [SerializeField] private float minCameraDistance = 1.5f;
    [Range(1, 3f)]
    [SerializeField] private float maxCameraDistance = 4;
    [Range(3f, 5f)]
    [SerializeField] private float cameraSensetivity = 5f;

    [Space]
    [SerializeField] private LayerMask aimLayerMask;

    private Vector2 mouseInput;
    private RaycastHit lastKnownMouseHit;

    private void Start()
    {
        player = GetComponent<Player>();
        AssignInputEvents();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            isAimingPrecisly = !isAimingPrecisly;

        if(Input.GetKeyDown(KeyCode.L))
            isLockongToTarget = !isLockongToTarget;

        UpdateAimVisuals();
        UpdateAimPosition();
        UpdateCameraPosition();
    }

    private void UpdateAimVisuals()
    {
        Transform gunPoint = player.weapon.GunPoint();
        Vector3 laserDirection = player.weapon.BulletDirection();

        float laserTipLength = .5f;
        float gunDistance = 4f;

        Vector3 endPoint = gunPoint.position + laserDirection * gunDistance;

        if(Physics.Raycast(gunPoint.position,laserDirection,out RaycastHit hit, gunDistance))
        {
            endPoint = hit.point;
            laserTipLength = 0;
        }

        aimLaser.SetPosition(0, gunPoint.position);
        aimLaser.SetPosition(1, endPoint);
        aimLaser.SetPosition(2, endPoint + laserDirection * laserTipLength);
    }
    private void UpdateAimPosition()
    {
        Transform target = Target();

        if(target != null && isLockongToTarget)
        {
            if (target.GetComponent<Renderer>()!= null)
                aim.position = target.GetComponent<Renderer>().bounds.center;
            else
                aim.position = target.position;
            return;
        }

        aim.position = GetMouseHitInfo().point;
        if (!isAimingPrecisly)
            aim.position = new Vector3(aim.position.x, transform.position.y + 1, aim.position.z);
    }
    public Transform Target()
    {
        Transform target = null;

        if(GetMouseHitInfo().transform.GetComponent<Target>() != null)
        {
            target = GetMouseHitInfo().transform;
        }

        return target;
    }
    public Transform Aim() => aim;
    public bool CanAimPrecisly() => isAimingPrecisly;   
    public RaycastHit GetMouseHitInfo()
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseInput);

        if(Physics.Raycast(ray,out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lastKnownMouseHit = hitInfo;
            return hitInfo;
        }

        return lastKnownMouseHit;
    }

    #region Camera Region
    private void UpdateCameraPosition()
    {
        cameraTarget.position =
                    Vector3.Lerp(cameraTarget.position, DesieredCameraPosition(), cameraSensetivity * Time.deltaTime);
    }
    private Vector3 DesieredCameraPosition()
    {
        float actualMaxCameraDistance = player.movement.moveInput.y < -.5f ? minCameraDistance : maxCameraDistance;

        Vector3 desiredCameraPosition = GetMouseHitInfo().point;
        Vector3 aimDirection = (desiredCameraPosition - transform.position).normalized;

        float distanceToDesierdPositon = Vector3.Distance(transform.position, desiredCameraPosition);
        float clampedDidtance = Mathf.Clamp(distanceToDesierdPositon, minCameraDistance,actualMaxCameraDistance);

        desiredCameraPosition = transform.position + aimDirection * clampedDidtance;
        desiredCameraPosition.y = transform.position.y + 1;

        return desiredCameraPosition;
    }

    #endregion

    private void AssignInputEvents()
    {
        controls = player.controls;

        controls.Character.Aim.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => mouseInput = Vector2.zero;
    }
}
