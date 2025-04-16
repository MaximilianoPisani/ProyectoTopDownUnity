using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    protected Vector2 _movement;
    protected Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleMovement();
    }

    void FixedUpdate()
    {
        _rb.velocity = _movement * moveSpeed;
    }

    protected abstract void HandleMovement();
}