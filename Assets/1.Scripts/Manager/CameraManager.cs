using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CameraState
{
    P1,
    P3,
    OnEvent
}
public class CameraManager : MonoBehaviour
{
    private CameraState state;

    [SerializeField]
    private Transform target;
    
    private Vector3 targetPosition;
    private const float SmoothSpeed = 30f;

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * SmoothSpeed);

    }
}
