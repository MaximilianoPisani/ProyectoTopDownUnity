using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance { get; private set; } 

    public Transform currentCheckpoint;

    private void Awake()
    {
         if (Instance == null)
         {   
            Instance = this;
            DontDestroyOnLoad(gameObject);
         }
        else
        {
            Destroy(gameObject);
        }
           
    }

    public void SetCheckpoint(Transform newCheckpoint)  // Sets the current checkpoint to a new checkpoint.
    {
        currentCheckpoint = newCheckpoint;
    }

    public Vector3 GetCheckpointPosition()  // Returns the position of the current checkpoint or Vector3.zero if no checkpoint has been set.
    {
        return currentCheckpoint != null ? currentCheckpoint.position : Vector3.zero; 
    }
}