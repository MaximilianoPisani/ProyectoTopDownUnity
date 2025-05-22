using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCharacterController : MonoBehaviour
{
    public float moveSpeed = 5f;
    protected Vector2 movement;
    protected Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Missing Rigidbody2D component on GameObject ");
    }

    void Update() // Handles input and movement direction called per frame.
    {
        HandleMovement();
    }

    void FixedUpdate() // Called at a fixed interval, used for physics-related updates.
    {
        rb.velocity = movement * moveSpeed; 
    }

    protected abstract void HandleMovement(); // Abstract method that must be implemented by derived classes, used to define how the character handles movement.
}