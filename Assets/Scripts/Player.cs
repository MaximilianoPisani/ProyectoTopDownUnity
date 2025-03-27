using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    public int maxHealth = 5;
    private int _currentHealth;
    private bool _canTakeDamage = true;
    public float damageCooldown = 1.5f;

    private Rigidbody2D _rb;
    private Vector2 _movement;
    private Animator _animator;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentHealth = maxHealth;
    }

    void Update()
    {

        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");


        _movement = _movement.normalized;


        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);


        bool isWalking = _movement.x != 0 || _movement.y != 0;
        _animator.SetBool("IsWalking", isWalking);
    }

    void FixedUpdate()
    {

        _rb.velocity = _movement * moveSpeed;
    }
}