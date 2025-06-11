using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour, IInventoryItem
{
    [SerializeField] private string _keyID = "goldenKey";
    public string ID => _keyID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerInventory inventory = other.GetComponent<PlayerInventory>();
            if (inventory != null)
            {
                inventory.PickUp(this);
                Debug.Log("Key collected: " + _keyID);

                if (PoolManager.Instance != null)
                    PoolManager.Instance.ReturnToPool(gameObject);
                else
                    Destroy(gameObject);
            }
        }
    }
    public void SetID(string newID)
    {
        _keyID = newID;
    }
}