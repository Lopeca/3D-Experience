using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 mouseDelta;

    [Header("Movement")]
    [SerializeField] Transform cameraContainer;
    [SerializeField] float movePower;
    [SerializeField] float lookSensitivity;

    private float upDownLookAngle;
    private Vector3 targetLookAngle;
    
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        targetLookAngle = transform.forward;
    }

    private void FixedUpdate()
    {
        HandleInput();
    }

    private void LateUpdate()
    {
        Look();
    }

   

    private void HandleInput()
    {
        if (IsGrounded())
        {
            Move();
        }
    }

    private bool IsGrounded()
    {
        return true;
    }

    private void Move()
    {
        Vector3 direction = cameraContainer.forward * movementInput.y + cameraContainer.right * movementInput.x;
        direction.z = 0;    // 정면을 볼 때 제일 빠른 그런 느낌이 좋다면 이 줄에 주석
        direction = direction.normalized;
        rb.AddForce(direction * movePower);
    }
    private void Look()
    {
        Vector3 cameraAngle = cameraContainer.localEulerAngles;

        upDownLookAngle -= mouseDelta.y * lookSensitivity;
        upDownLookAngle = Mathf.Clamp(upDownLookAngle, -90f, 90f);
        
        targetLookAngle.x = AngleCalculator.NormalizeAngle(upDownLookAngle);
        targetLookAngle.y = AngleCalculator.NormalizeAngle(targetLookAngle.y + mouseDelta.x * lookSensitivity);

        cameraAngle.x = Mathf.LerpAngle(cameraAngle.x, targetLookAngle.x, 0.1f);
        cameraAngle.y = Mathf.LerpAngle(cameraAngle.y, targetLookAngle.y, 0.1f);

        cameraContainer.localEulerAngles = cameraAngle;
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // 라이더가 교정해준 신 문법
        movementInput = context.phase switch
        {
            InputActionPhase.Performed => context.ReadValue<Vector2>(),
            InputActionPhase.Canceled => Vector2.zero,
            _ => movementInput
        };
    }

    public void OnLookInput(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }
}
