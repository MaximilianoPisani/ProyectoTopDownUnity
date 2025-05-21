using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemyVision : MonoBehaviour
{
    public EnemyController enemyController;
    public EnemyMeleeAttack enemyMeleeAttack;

    private void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;

        if (enemyController == null)
            enemyController = GetComponentInParent<EnemyController>();

        if (enemyMeleeAttack == null)
            enemyMeleeAttack = GetComponentInParent<EnemyMeleeAttack>();
    }

    private void OnTriggerEnter2D(Collider2D other) // Called when another collider enters the trigger collider attached to this object.
    {
        if (other.CompareTag("Player"))
        {
            enemyController.OnTargetEnterVision(other.transform);
            enemyMeleeAttack.SetTarget(other.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other) // Called when another collider exits the trigger collider attached to this object.
    {
        if (other.CompareTag("Player"))
        {
            enemyController.OnTargetExitVision();
            enemyMeleeAttack.SetTarget(null);
        }
    }
}