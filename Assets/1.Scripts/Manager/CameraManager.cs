using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    P1,
    P3
}
public class CameraManager : MonoBehaviour
{
    private CameraState state;

    [SerializeField]
    private Camera mainCamera;
    [SerializeField]
    private Transform target;
    
    private Vector3 targetPosition;
    private readonly Vector3 thirdPerspectivePos = new Vector3(0, 0.2f, -5);
    private const float SmoothSpeed = 100f;
    private const float ToggleSpeed = 10f;
    void LateUpdate()
    {
            transform.position = target.position;
            
            if (state == CameraState.P1)
            {
                mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, Vector3.zero, Time.deltaTime * ToggleSpeed);
            }
            else
            {
                mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, thirdPerspectivePos, Time.deltaTime * ToggleSpeed);
            }
    }

    public void ToggleState()
    {
        state = state == CameraState.P1 ? CameraState.P3 : CameraState.P1;
        
    }

}
