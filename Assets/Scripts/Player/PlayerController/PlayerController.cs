using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : BaseCharacterController
{
    private Vector2 _lastDirection = Vector2.down;
    private AnimationControllerHandler _animatorHandler;
  
    void Start()
    {
        _animatorHandler = GetComponent<AnimationControllerHandler>();
         if (_animatorHandler == null)
            Debug.LogError("Missing AnimationControllerHandler component on GameObject ");

         rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Missing Rigidbody2D component on GameObject ");

    }

    protected override void HandleMovement() // Implements abstract method from BaseCharacterController to handle player movement.
    {
        movement.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0); // Reads horizontal input: D (+1) and A (-1)
        movement.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0); // Reads vertical input: W (+1) and S (-1)

        movement = movement.normalized;

        bool isMoving = movement != Vector2.zero;

        if (isMoving)
        {
            _lastDirection = movement;
        }

        _animatorHandler.SetFloat("X", movement.x);
        _animatorHandler.SetFloat("Y", movement.y);
        _animatorHandler.SetBool("Moving", isMoving);
        _animatorHandler.SetFloat("LastX", _lastDirection.x);
        _animatorHandler.SetFloat("LastY", _lastDirection.y);
    }

    void OnCollisionEnter2D(Collision2D collision) // Called when the player collides with another 2D object.
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (rb != null)
                rb.velocity = Vector2.zero; 
        }
    }
}