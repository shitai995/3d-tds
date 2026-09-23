// ========================================================
// 作者：娇娇 
// 创建时间：2026-07-06 15:18:04
// 版本：V1.1
// 描述：
// ========================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController characterController;

    [Header("移动")]
    [SerializeField] private float walkSpeed;
    private Vector3 movementDirection;
    [SerializeField] private float gravityScale = 9.81f;
    private float verticalVelocity;

    [Header("Aim Info")]
    [SerializeField] private Transform aim;
    [SerializeField] private LayerMask aimLayerMask;
    private Vector3 lookingDirection;

    private Vector2 moveInput;
    private Vector2 aimInput;


    private void Awake()
    {
        controls = new PlayerControls();

        controls.Character.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Character.Movement.canceled += ctx => moveInput = Vector2.zero;

        controls.Character.Aim.performed += ctx => aimInput = ctx.ReadValue<Vector2>();
        controls.Character.Aim.canceled += ctx => aimInput = Vector2.zero;
    }

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }
    private void Update()
    {
        ApplyMovement();

        AimTowardMouse();
    }



    private void AimTowardMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(aimInput);

        if (Physics.Raycast(ray, out var hitInfo, Mathf.Infinity, aimLayerMask))
        {
            lookingDirection = hitInfo.point - transform.position;
            lookingDirection.y = 0f;
            lookingDirection.Normalize();

            transform.forward = lookingDirection;

            aim.position = new Vector3(hitInfo.point.x,transform.position.y,hitInfo.point.z);
        }
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);
        ApplyGravity();

        if (movementDirection.magnitude > 0)
        {
            characterController.Move(movementDirection * Time.deltaTime * walkSpeed);
        }
    }

    private void ApplyGravity()
    {
        if (characterController.isGrounded == false)
        {
            verticalVelocity -= gravityScale * Time.deltaTime;
            movementDirection.y = verticalVelocity;
        }
        else
            verticalVelocity = -.5f;
    }

    private void OnEnable()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Disable();
        Debug.Log("test");
    }

}
