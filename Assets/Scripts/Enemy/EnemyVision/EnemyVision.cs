using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyVision : MonoBehaviour
{
    public EnemyController enemyController;
    public EnemyMeleeAttack enemyMeleeAttack;
    public EnemyRangedAttack enemyRangedAttack;

    private void Start()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;

        if (enemyController == null)
            enemyController = GetComponentInParent<EnemyController>();
        if (enemyMeleeAttack == null)
            enemyMeleeAttack = GetComponentInParent<EnemyMeleeAttack>();
        if (enemyRangedAttack == null)
            enemyRangedAttack = GetComponentInParent<EnemyRangedAttack>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (enemyController.currentTarget == null)
            {
                enemyController?.OnTargetEnterVision(other.transform);
                if (enemyMeleeAttack != null)
                    enemyMeleeAttack.SetTarget(other.transform);
                if (enemyRangedAttack != null)
                    enemyRangedAttack.SetTarget(other.transform);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            enemyController?.OnTargetExitVision();
            if (enemyMeleeAttack != null)
                enemyMeleeAttack.SetTarget(null);
            if (enemyRangedAttack != null)
                enemyRangedAttack.SetTarget(null);
        }
    }
}