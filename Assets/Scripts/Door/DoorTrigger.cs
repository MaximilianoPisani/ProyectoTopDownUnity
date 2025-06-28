using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
    private Door _parentDoor;

    private void Awake()
    {
        _parentDoor = GetComponentInParent<Door>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _parentDoor.TryOpen(other.gameObject);
        }
    }
}