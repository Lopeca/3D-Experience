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
    private Camera _camera;

    private void Start()
    {
        _camera = Camera.main;
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
        Ray ray = _camera.ScreenPointToRay(new Vector3((float)Screen.width / 2, (float)Screen.height / 2));
        RaycastHit hit;

        if (Physics.SphereCast(ray, sphereRayRadius,out hit, checkDistance, layerMask))
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