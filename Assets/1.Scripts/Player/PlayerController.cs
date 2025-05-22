using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;


public class PlayerController : MonoBehaviour
{
    private Player player;
    
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 mouseDelta;
    float sphereRadius;
    private const float rayCheckDistance = 0.1f; // 지면과의 최소 거리
    
    [Header("Movement")]
    [SerializeField]
    Transform cameraContainer;
    [SerializeField] float movePower;
    [SerializeField] float maxSpeed;
    [SerializeField] float currentSpeed;
    [SerializeField] float jumpImpulsePower;
    [SerializeField] float lookSensitivity;

    public float movePowerMultiplier = 1;

    private float upDownLookAngle;
    private Vector3 targetLookAngle;

    [SerializeField] bool isGrounded;
    private Vector3 groundNormal;
    
    private void Awake()
    {
        player = GetComponent<Player>();
        rb = GetComponentInChildren<Rigidbody>();
        targetLookAngle = transform.forward;
        sphereRadius = GetComponent<SphereCollider>().radius;
        
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded(); // isGrounded 수요가 두 군데라서 캐싱
        HandleInputTypePerformed();
        currentSpeed = rb.velocity.magnitude;   // 인스펙터 관찰용
    }
    
    private void LateUpdate()
    {
        Look();
    }

    private void HandleInputTypePerformed()
    {
        if (isGrounded)
        {
            Move();
        }
    }


    private void Move()
    {
        Vector3 direction = cameraContainer.forward * movementInput.y + cameraContainer.right * movementInput.x;
        direction.y = 0; 
        direction = direction.normalized;
        rb.AddForce(direction * (movePower * movePowerMultiplier));

        if (rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
            
        }
    }

    private void Look()
    {
        Vector3 cameraAngle = cameraContainer.localEulerAngles;

        upDownLookAngle -= mouseDelta.y * lookSensitivity;
        upDownLookAngle = Mathf.Clamp(upDownLookAngle, -90f, 90f);

        // Vector3인 targetLookAngle 안에 터무니없이 크거나 작은 값이 담기지 않기를 의도하는 정도
        // 아마도 LerpAngle에 의해 이런 360도 정제를 안 해도 작동할 것 같습니다
        targetLookAngle.x = AngleCalculator.NormalizeAngle360(upDownLookAngle);
        targetLookAngle.y = AngleCalculator.NormalizeAngle360(targetLookAngle.y + mouseDelta.x * lookSensitivity);

        cameraAngle.x = Mathf.LerpAngle(cameraAngle.x, targetLookAngle.x, 0.1f);
        cameraAngle.y = Mathf.LerpAngle(cameraAngle.y, targetLookAngle.y, 0.1f);

        cameraContainer.localEulerAngles = cameraAngle;
    }

    private void Jump()
    {
        if (!isGrounded) return;
        rb.AddForce(groundNormal * jumpImpulsePower, ForceMode.Impulse);
    }

    bool IsGrounded()
    {
        if (Physics.SphereCast(transform.position + new Vector3(0, 0.05f, 0), sphereRadius, Vector3.down, out RaycastHit hit, rayCheckDistance))
        {
            groundNormal = hit.normal;
            return true;
        }

       
        return false;
    }

    #region InputAction Functions

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

    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Jump();
        }
    }

    public void OnNumberInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            if (context.control is not KeyControl key) return;

            switch (key.keyCode)
            {
                case Key.Digit1:
                    player.itemHandler.UseItem(1);
                    break;
                case Key.Digit2:
                    player.itemHandler.UseItem(2);
                    break;
            }
            
        }
    }

    #endregion

    public void ForceMovement(float bumpForce, Vector3 contactNormal)
    {
        rb.AddForce(contactNormal * bumpForce, ForceMode.VelocityChange);
    }


    
}