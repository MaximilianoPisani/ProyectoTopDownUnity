using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : HealthSystem
{
    private EnemyController _enemyController;
    private AnimationControllerHandler _animHandler;
    private EnemyDamageFeedbackHandler _feedbackHandler;
    private EnemyMeleeAttack _meleeAttack;
    private NavMeshAgent _agent;

    private bool _isDead = false;
    private bool _isInvulnerable = false;

    protected override void Start()
    {
        base.Start();
        _enemyController = GetComponent<EnemyController>();
        if (_enemyController == null)
            Debug.LogWarning(" Missing EnemyController ");

        _animHandler = GetComponent<AnimationControllerHandler>();
        if (_animHandler == null)
            Debug.LogWarning(" Missing AnimationControllerHandler ");

        _feedbackHandler = GetComponent<EnemyDamageFeedbackHandler>();
        if (_feedbackHandler == null)
            Debug.LogWarning(" Missing EnemyDamageFeedbackHandler ");

        _meleeAttack = GetComponent<EnemyMeleeAttack>();
        EnemyRangedAttack rangedAttack = GetComponent<EnemyRangedAttack>();

        if (_meleeAttack == null && rangedAttack == null)
            Debug.LogWarning("Enemy has no attack script: missing both EnemyMeleeAttack and EnemyRangedAttack.");

        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
            Debug.LogWarning("Missing NavMeshAgent ");
    }


    public override void TakeDamage(int amount) // Called when the enemy takes damage
    {
        if (_isDead || _isInvulnerable) return;

        base.TakeDamage(amount);

        if (!_isDead)
        {
            if (_feedbackHandler != null)
                StartCoroutine(DamageFeedbackCoroutine());

            if (_enemyController != null)
                _enemyController.SetEnemyActive(false); 
        }
    }
    private IEnumerator DamageFeedbackCoroutine() // Coroutine to handle temporary invulnerability and feedback after taking damage
    {
        _isInvulnerable = true;

        if (_enemyController != null) _enemyController.enabled = false;
        if (_meleeAttack != null) _meleeAttack.enabled = false;

        yield return StartCoroutine(_feedbackHandler.PlayFeedback());

        if (!_isDead)
        {
            if (_enemyController != null) _enemyController.enabled = true;
            if (_meleeAttack != null) _meleeAttack.enabled = true;
        }

        _isInvulnerable = false;
    }

    protected override void Die() // Called when the enemy dies
    {
        _isDead = true;
        _isInvulnerable = false;

        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }
        if (_enemyController != null) _enemyController.enabled = false;
        if (_meleeAttack != null) _meleeAttack.enabled = false;

        Transform target = null;

        if (_enemyController != null)
        {
            target = _enemyController.currentTarget;
        }
        Vector2 direction = Vector2.zero;
        if (target != null)
            direction = ((Vector2)(target.position - transform.position)).normalized;

        _animHandler.SetBool("isDead", true);
        _animHandler.SetFloat("LastX", direction.x);
        _animHandler.SetFloat("LastY", direction.y);

        StartCoroutine(ReturnToPoolAfterDelay(1.5f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay) // Coroutine to return the enemy to the pool after a delay
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}