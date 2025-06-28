using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
   public Transform destinationPoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
   
        if (other.CompareTag("Player"))
        {
            other.transform.position = destinationPoint.position;
        }
    }
}