using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

  
    public GameObject GetFromPool(GameObject prefab)
    {
        string key = prefab.name;

        if (!poolDictionary.ContainsKey(key))
            poolDictionary[key] = new Queue<GameObject>();

        if (poolDictionary[key].Count > 0)
        {
            GameObject obj = poolDictionary[key].Dequeue();
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

    
    public GameObject GetFromPool(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        GameObject obj = GetFromPool(prefab);
        obj.transform.position = position;
        obj.transform.rotation = rotation;
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        string key = obj.name;

        if (!poolDictionary.ContainsKey(key))
            poolDictionary[key] = new Queue<GameObject>();

        poolDictionary[key].Enqueue(obj);
    }
}