using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private HashSet<string> _items = new HashSet<string>();

    public event Action<string> OnItemCollected;

    public void PickUp(IInventoryItem item)
    {
        if (_items.Add(item.ID))
        {
            Debug.Log("Picked up: " + item.ID);
            OnItemCollected?.Invoke(item.ID);
        }
    }

    public bool HasItem(string id) => _items.Contains(id);
}