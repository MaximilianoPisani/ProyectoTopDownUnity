using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private string _requiredKeyID = "goldenKey";
    [SerializeField] private GameObject _doorVisual; 
    [SerializeField] private Collider2D _doorCollider; 
    [SerializeField] private Collider2D _entryTrigger;

    private bool _isOpen = false;

    public void TryOpen(GameObject player)
    {
        if (_isOpen) return;

        PlayerInventory inventory = player.GetComponent<PlayerInventory>();
        if (inventory != null && inventory.HasItem(_requiredKeyID))
        {
            OpenDoor();
        }
        else
        {
            Debug.Log("The door is locked. You need the key: " + _requiredKeyID);
        }
    }

    void OpenDoor()
    {
        Debug.Log("Door opened with key: " + _requiredKeyID);
        _isOpen = true;


        _doorVisual.SetActive(false);
        _doorCollider.enabled = false;
        _entryTrigger.enabled = false; 
    }

    public void TryReappear()
    {
        if (!_isOpen) return;

        Debug.Log("Door reappeared behind the player");
        _doorVisual.SetActive(true);
        _doorCollider.enabled = true;
        _entryTrigger.enabled = true; 
        _isOpen = false;
    }
}