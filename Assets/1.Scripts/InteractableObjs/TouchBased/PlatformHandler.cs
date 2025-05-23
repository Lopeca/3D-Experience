using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlatformHandler : MonoBehaviour
{
    enum State
    {
        Move1to2,
        Move2to1
    }
    // @formatter:off
    [SerializeField] GameObject platform;
    [SerializeField] private Transform p1;
    [SerializeField] private Transform p2;
    [SerializeField] private float moveSpeed;
    [SerializeField] [Range(0,2)] private float offset;
    // @formatter:on

    private Platform platformComponent;
    private Vector3 pointPosDiff;
    private float pointDistance;
    private Vector3 pointPosNormal; // p1에서 p2를 향하는 방향
    private State state;
    void Start()
    {
        platformComponent = platform.GetComponent<Platform>();
        pointPosDiff = p2.position - p1.position;
        
        pointDistance = pointPosDiff.magnitude;
        platform.transform.localPosition = ApplyOffsetToPosition();
        pointPosNormal = pointPosDiff.normalized;
    }

    void FixedUpdate()
    {
        if (state == State.Move1to2)
        {
            platform.transform.localPosition += moveSpeed * Time.fixedDeltaTime * pointPosNormal;
            
            if ((p1.localPosition - platform.transform.localPosition).magnitude > pointDistance)
            {
                platform.transform.localPosition += (p2.localPosition - platform.transform.localPosition) * 2;
                ToggleState();
            }
            
        }
        else if (state == State.Move2to1)
        {
            platform.transform.localPosition -= moveSpeed * Time.fixedDeltaTime * pointPosNormal;
            if ((p2.localPosition - platform.transform.localPosition).magnitude > pointDistance)
            {
                platform.transform.localPosition += (p1.localPosition - platform.transform.localPosition) * 2;
                ToggleState();
            }
        }
        platformComponent.MoveRidersTogether();
    }

    Vector3 ApplyOffsetToPosition()
    {
        if (offset is < 1 or 2)
        {
            state = State.Move1to2;
            return p1.localPosition + pointPosDiff * offset;
        }
        else if (offset < 2)
        {
            state = State.Move2to1;
            return p2.localPosition - pointPosDiff * (offset - 1);
        }
        else return p1.localPosition;
    }

    void ToggleState()
    {
        state = state == State.Move1to2 ? State.Move2to1 : State.Move1to2;
    }
    
}
