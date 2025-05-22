using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//진입 속도가 얼마였던 고정 속도로 구현한 부분은 의도입니다.
public class Bumper : MonoBehaviour
{
    [SerializeField]
    private float bumpForce;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            ContactPoint contact = collision.contacts[0];
            
            player.ForceMovement(bumpForce, -contact.normal);
        }
    }
}
