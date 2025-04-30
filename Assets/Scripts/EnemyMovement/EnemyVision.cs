using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyVision : MonoBehaviour
{
    [SerializeField] private EnemyMovement _enemyMovement;
    private CircleCollider2D _circleCollider2D;
    private void Start()
    {
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        var playerDirection = other.transform.position - transform.position;
        var layer = -(1 << LayerMask.NameToLayer("Ignore Raycast"));
        var rayCast = Physics2D.Raycast(transform.position, playerDirection, 4, layer);
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