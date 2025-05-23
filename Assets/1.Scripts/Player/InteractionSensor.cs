using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionSensor : MonoBehaviour
{
    [SerializeField] float checkDistance;
    [SerializeField] private float sphereRayRadius;
    [SerializeField] LayerMask layerMask;

    public GameObject detectedObject;
    private IInputBasedInteractable _detectedInputBasedInteractable;

    InteractionUI interactionUI;
    private CameraManager cameraManager;

    private void Start()
    {
        cameraManager = GameManager.Instance.cameraManager;
        GameManager.Instance.uiManager.interactionUI.TryGetComponent(out interactionUI);
        
        Debug.Assert(interactionUI, "UI가 없어도 일단 작동하기를 의도한다는 뜻에서 만들어본 경고문");
        interactionUI.gameObject.SetActive(false);
    }

    void Update()
    {
        Detect();
    }

    private void Detect()
    {
        float sphereRayRadiusMultiplier = cameraManager.state == CameraState.P1 ? 1 : 3;
        Vector3 rayOrigin = transform.position - cameraManager.transform.forward * (sphereRayRadius * sphereRayRadiusMultiplier);
        Ray ray = new Ray(rayOrigin, cameraManager.transform.forward);
        
        RaycastHit hit;
        bool cast = Physics.SphereCast(ray, sphereRayRadius * sphereRayRadiusMultiplier, out hit, checkDistance, layerMask);

        if (cast)
        {
            if (hit.collider.gameObject != detectedObject)
            {
                _detectedInputBasedInteractable?.OnTargetLost();
                detectedObject = hit.collider.gameObject;
                _detectedInputBasedInteractable = detectedObject.GetComponent<IInputBasedInteractable>();
                _detectedInputBasedInteractable.OnTargeted();
                
                interactionUI?.gameObject.SetActive(true);
                interactionUI?.ShowPrompt(_detectedInputBasedInteractable.Prompt);
            }
        }
        else
        {
            detectedObject = null;
            _detectedInputBasedInteractable = null;
            _detectedInputBasedInteractable?.OnTargetLost();
            interactionUI?.gameObject.SetActive(false);
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && _detectedInputBasedInteractable != null)
        {
            _detectedInputBasedInteractable.Interact();
            _detectedInputBasedInteractable = null;
            detectedObject = null;
        }
    }
}