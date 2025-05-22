using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCube : MonoBehaviour
{
    [SerializeField]
    private ItemData item;
    // Update is called once per frame
    void FixedUpdate()
    {
        Rotate();
    }

    void Rotate()
    {
        transform.Rotate(0, 72 * Time.fixedDeltaTime, 0);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameManager.Instance.player.itemHandler.AddItem(item);
            Destroy(gameObject);
        }
    }
}