using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleObstacle : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private void OnCollisionStay(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Player player = collision.collider.GetComponent<Player>(); 
            
            player.TakeDamage(damage);
        }
    }
}
