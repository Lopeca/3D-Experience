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
    private IInteractable detectedInteractable;

    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
    }

    void Update()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3((float)Screen.width / 2, (float)Screen.height / 2));
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRayRadius,out hit, checkDistance, layerMask))
        {
            if (hit.collider.gameObject != detectedObject)
            {
                detectedInteractable?.OnTargetLost();
                detectedObject = hit.collider.gameObject;
                detectedInteractable = detectedObject.GetComponent<IInteractable>();
                detectedInteractable.OnTargeted();
            }
        }
        else
        {
            detectedObject = null;
            detectedInteractable?.OnTargetLost();
        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && detectedInteractable != null)
        {
            detectedInteractable.Interact();
            detectedInteractable = null;
            detectedObject = null;
        }
    }
}