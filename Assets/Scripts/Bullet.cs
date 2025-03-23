using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f; 
    public int damage = 10;   
    public float lifetime = 3f; 

    void Start()
    {
       
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Enemy"))
        {
           
            Destroy(gameObject);
        }
        
        else if (!collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}