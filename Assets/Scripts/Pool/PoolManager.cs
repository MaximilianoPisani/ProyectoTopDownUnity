using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<string, Queue<GameObject>> _poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

  
    public GameObject GetFromPool(GameObject prefab) // Gets a GameObject from the pool or instantiates a new one if none are available.
    {
        string key = prefab.name;

        if (!_poolDictionary.ContainsKey(key))
            _poolDictionary[key] = new Queue<GameObject>();

        if (_poolDictionary[key].Count > 0)
        {
            GameObject obj = _poolDictionary[key].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            GameObject newObj = Instantiate(prefab);
            newObj.name = key; 
            return newObj;
        }
    }

    
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation) // Gets a GameObject from the pool with a specified position and rotation.
    {
        GameObject obj = GetFromPool(prefab);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReturnToPool(GameObject obj) // Returns a GameObject back to the pool to be reused.
    {
        obj.SetActive(false);
        string key = obj.name;

        if (!_poolDictionary.ContainsKey(key))
            _poolDictionary[key] = new Queue<GameObject>();

        _poolDictionary[key].Enqueue(obj);
    }
}