using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHealth : HealthSystem
{
    private EnemyMovement _movement;
    private AnimationControllerHandler _animHandler;
    private EnemyDamageFeedbackHandler _feedbackHandler;
    private EnemyMeleeAttack _meleeAttack;
    private Rigidbody2D _rb;
    private NavMeshAgent _agent;

    private bool _isDead = false;
    private bool _isInvulnerable = false;

    protected override void Start()
    {
        base.Start();
        _movement = GetComponent<EnemyMovement>();
        _animHandler = GetComponent<AnimationControllerHandler>();
        _feedbackHandler = GetComponent<EnemyDamageFeedbackHandler>();
        _meleeAttack = GetComponent<EnemyMeleeAttack>();
        _rb = GetComponent<Rigidbody2D>();
        _agent = GetComponent<NavMeshAgent>();
    }

    public override void TakeDamage(int amount)
    {
        if (_isDead || _isInvulnerable)
            return;

        base.TakeDamage(amount);

        if (!_isDead && _feedbackHandler != null)
        {
            StartCoroutine(DamageFeedbackCoroutine());
        }
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        _isInvulnerable = true;

        if (_movement != null) _movement.enabled = false;
        if (_meleeAttack != null) _meleeAttack.enabled = false;

        yield return StartCoroutine(_feedbackHandler.PlayFeedback());

        if (!_isDead)
        {
            if (_movement != null) _movement.enabled = true;
            if (_meleeAttack != null) _meleeAttack.enabled = true;
        }

        _isInvulnerable = false;
    }

    protected override void Die()
    {
        _isDead = true;
        _isInvulnerable = false;


        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }

        if (_rb != null)
        {
            _rb.velocity = Vector2.zero;
        }

        if (_movement != null) _movement.enabled = false;
        if (_meleeAttack != null) _meleeAttack.enabled = false;

        Transform target = _movement != null ? _movement.CurrentTarget : null;
        Vector2 direction = Vector2.zero;
        if (target != null)
        {
            direction = ((Vector2)(target.position - transform.position)).normalized;
        }

        _animHandler.SetBool("isDead", true);
        _animHandler.SetFloat("LastX", direction.x);
        _animHandler.SetFloat("LastY", direction.y);

        StartCoroutine(ReturnToPoolAfterDelay(1.5f));
    }

    private IEnumerator ReturnToPoolAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PoolManager.Instance.ReturnToPool(gameObject);
    }
}