using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) // Called when another Collider2D enters this object's trigger area.
    {
        if (other.CompareTag("Player"))
        {
            CheckpointManager.Instance.SetCheckpoint(transform); // If the one who enters is the Player, set the current checkpoint as the new checkpoint using the CheckpointManager
        }
    }
}