using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyVision : MonoBehaviour
{
    public Transform target; 
    public EnemyMovement enemyMovement;
    public EnemyMeleeAttack enemyMeleeAttack;

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        if (enemyMovement == null)
        {
            enemyMovement = GetComponentInParent<EnemyMovement>();
        }

        if (enemyMeleeAttack == null)
        {
            enemyMeleeAttack = GetComponentInParent<EnemyMeleeAttack>();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyMovement.SetTarget(target);  
            enemyMeleeAttack.SetTarget(other.transform);  
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            
            enemyMovement.RemoveTarget();  
            enemyMeleeAttack.SetTarget(null);  
        }
    }
}