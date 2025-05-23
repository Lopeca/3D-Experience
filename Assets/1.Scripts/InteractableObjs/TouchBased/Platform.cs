using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private float moveSpeed;
    Vector3 prevPosition;
    private Vector3 deltaPosition;

    readonly HashSet<GameObject> ridingObjs = new(); // 플랫폼에 타는 모든 충돌체들 처리


    public void Init(float speed)
    {
        moveSpeed = speed;
    }
    public void MoveRidersTogether()
    {
        deltaPosition = transform.position - prevPosition;
        prevPosition = transform.position;

        foreach (GameObject obj in ridingObjs)
        {
            obj.transform.position += deltaPosition;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        ridingObjs.Add(other.gameObject);
    }

    private void OnCollisionExit(Collision other)
    {
        ridingObjs.Remove(other.gameObject);
        
    }
}
