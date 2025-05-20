using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector2 movementInput;
    private Vector2 mouseDelta;

    [Header("Movement")]
    [SerializeField]
    Transform cameraContainer;
    [SerializeField] float movePower;
    [SerializeField] float jumpImpulsePower;
    [SerializeField]
    float lookSensitivity;

    private float upDownLookAngle;
    private Vector3 targetLookAngle;

    [SerializeField] bool isGrounded;
    private Vector3 accumulatedJumpDirection;
    //private int contactCount;
    private void Awake()
    {
        rb = GetComponentInChildren<Rigidbody>();
        targetLookAngle = transform.forward;
    }

    private void FixedUpdate()
    {
        ResetInfoForJump();
        HandleInputTypePerformed();
    }

    private void ResetInfoForJump()
    {
        accumulatedJumpDirection = Vector3.zero;
        // contactCount = 0; 
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
        direction.y = 0; // 정면을 볼 때 제일 빠른 그런 느낌이 좋다면 이 줄에 주석
        direction = direction.normalized;
        rb.AddForce(direction * movePower);
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
        Vector3 jumpVector = accumulatedJumpDirection.normalized;
        
        
        rb.AddForce(jumpVector * jumpImpulsePower, ForceMode.Impulse);
        
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.contacts.Length == 0) return;

        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                if (Vector3.Angle(contact.normal, Vector3.up) > 60) continue;
 
                isGrounded = true;
                accumulatedJumpDirection += contact.normal;
                //contactCount++;
            }
        }
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

    #endregion
}