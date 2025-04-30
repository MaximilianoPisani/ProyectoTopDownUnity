using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    private CircleCollider2D _circleCollider2D;
    private const float Buffer = 1.1f;
    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerDirection = other.transform.position - transform.position;
        var layer = ~(1 << LayerMask.NameToLayer("Ignore Raycast") |  1 << LayerMask.NameToLayer("Enemy"));
        var rayCast = Physics2D.Raycast(transform.position, playerDirection, _circleCollider2D.radius * Buffer, layer);
        Debug.DrawRay(transform.position, playerDirection.normalized * _circleCollider2D.radius * Buffer);
        if (rayCast.collider == null) return;




        if (rayCast.collider.CompareTag("Player"))
        {
            _enemyMovement.SetTarget(other.transform);

        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) return;
        {
            _enemyMovement.SetTarget(null);
        }
    }
}