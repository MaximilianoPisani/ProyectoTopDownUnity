using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : CharacterMovement
{
    private AnimationControllerHandler _animatorHandler;
    private Vector2 _lastDirection = Vector2.down;

    void Start()
    {
        _animatorHandler = GetComponent<AnimationControllerHandler>();
    }

    protected override void HandleMovement()
    {
        _movement.x = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
        _movement.y = (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0);

        _movement = _movement.normalized;

        bool isMoving = _movement != Vector2.zero;

        
        if (isMoving)
        {

            _lastDirection = _movement;

        }

        
        _animatorHandler.SetFloat("X", _movement.x);
        _animatorHandler.SetFloat("Y", _movement.y);
        _animatorHandler.SetBool("Moving", isMoving);
        _animatorHandler.SetFloat("LastX", _lastDirection.x);
        _animatorHandler.SetFloat("LastY", _lastDirection.y);
    }
}