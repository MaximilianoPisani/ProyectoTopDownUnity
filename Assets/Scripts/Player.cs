using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f; // Velocidad del personaje

    private Rigidbody2D _rb;
    private Vector2 _movement;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        
        _movement = _movement.normalized;
    }

    void FixedUpdate()
    {
        // Aplica movimiento usando Rigidbody2D
        _rb.velocity = _movement * moveSpeed;
    }
}
